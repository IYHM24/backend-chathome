using ApiBussines;
using Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;

namespace WebSockets.Hubs
{
    public class UserHub: Hub
    {
        private Bussiness Bussiness;

        // Lista en memoria de usuarios conectados (usuario -> lista de conexiones)
        private static readonly ConcurrentDictionary<string, HashSet<string>> _connectedUsers = new ConcurrentDictionary<string, HashSet<string>>();

        // Constructor
        public UserHub(IConfiguration conf)
        {
            Bussiness = new Bussiness(conf.GetConnectionString("DefaultConnection")!.ToString());
        }

        //Get User By conection
        public static string? GetUserByConnectionId(string connectionId)
        {
            foreach (var kvp in _connectedUsers)
            {
                if (kvp.Value.Contains(connectionId))
                {
                    return kvp.Key; // Este es el username
                }
            }
            return null; // No encontrado
        }

        // Al conectar
        public override async Task<Task> OnConnectedAsync()
        {
            var username = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Context.ConnectionId;

            _connectedUsers.AddOrUpdate(username,
                _ => new HashSet<string> { Context.ConnectionId },
                (_, existing) =>
                {
                    existing.Add(Context.ConnectionId);
                    return existing;
                });

            try
            {
                //Actualizar estado del usuario
                dynamic response_user = await Bussiness.UsuariosOperations(new UsuarioOperation
                {
                    contextId = Context.ConnectionId,
                    uuid_google = username,
                    proceso = "set Online"
                });
                string json = JsonSerializer.Serialize((object)response_user);
                //Enviar notificación a todos los clientes conectados
                await Clients.All.SendAsync("UserConnected", "[]");
                Console.WriteLine("🟢 - Usuarios conectado: " + GetOnlineUsers());
                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        
        // Al desconectar
        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var username = GetUserByConnectionId(Context.ConnectionId)!;
            if (_connectedUsers.TryGetValue(username, out var connections))
            { 
                connections!.Remove(Context.ConnectionId);

                if (connections.Count == 0)
                {
                    _connectedUsers.TryRemove(username, out _);
                    //Actualizar estado del usuario
                    var response_user = await Bussiness.UsuariosOperations(new UsuarioOperation
                    {
                        contextId = Context.ConnectionId,
                        proceso = "set Offline"
                    });
                    string json = JsonSerializer.Serialize((object)response_user);
                    await Clients.All.SendAsync("UserDisconnected", json);
                }
            }
            return base.OnDisconnectedAsync(exception);
        }

        // Método opcional para obtener la lista de usuarios conectados
        public Task<List<string>> GetOnlineUsers()
        {
            return Task.FromResult(_connectedUsers.Keys.ToList());
        }

       
    }
}

