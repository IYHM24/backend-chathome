using Microsoft.AspNetCore.Mvc;
using ApiBussines;
using Models;
using Microsoft.AspNetCore.SignalR;
using WebSockets.Hubs;

namespace WebSockets.Controllers
{
    [ApiController]
    [Route("mensajes")]
    public class MensajesController : ControllerBase
    {
        private readonly Bussiness _bussiness;
        private IHubContext<UserHub> _userHubContext;

        public MensajesController(IConfiguration configuration, IHubContext<UserHub> hubContext)
        {
            _bussiness = new Bussiness(configuration.GetConnectionString("DefaultConnection")!);
            _userHubContext = hubContext;
        }

        /// <summary>
        /// Operaciones relacionadas con mensajes.
        /// </summary>
        /// <param name="mensaje">Objeto que representa el mensaje.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("operaciones")]
        public async Task<IActionResult> MensajesOperations(ChatEntry c)
        {
            dynamic responseMensajes;
            string responseUsuarios;

            try
            {
                // Validación del mensaje
                if (c.mensaje == null)
                {
                    return BadRequest("El mensaje no puede ser nulo.");
                }

                if(c.mensaje.proceso == "actualizar o crear")
                {
                    // Guardar el mensaje
                    responseMensajes = await _bussiness.MensajesOperations(c.mensaje);
                    responseUsuarios = "";
                    // Guardar los usuarios
                    if (c.ui_usuarios!.Count > 0)
                    {
                        foreach (var usuario in c.ui_usuarios)
                        {
                            MensajeDto data = new MensajeDto
                            {
                                proceso = "crear usuarios",
                                chat_id = c.mensaje.chat_id,
                                ui_usuario_remitente = usuario,
                            };
                            responseUsuarios = await _bussiness.MensajesOperations(data);
                        }
                    }

                } else
                {
                    responseMensajes = await _bussiness.MensajesOperations(c.mensaje); ;
                    responseUsuarios = "";
                }
                //Notificar cambios
                await _userHubContext!.Clients.All.SendAsync("updateSidebar", "actualizar");

                return Ok(new { status = true, msj = new { responseMensajes, responseUsuarios } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, msj = ex.Message });
            }
        }
    }
}