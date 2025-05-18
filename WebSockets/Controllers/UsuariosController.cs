using ApiBussines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using System.Threading.Tasks;

namespace WebSockets.Controllers
{
    [Authorize]
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        private Bussiness Bussiness;
        private readonly IConfiguration _configuration;

        //Constructor
        public UsuariosController(IConfiguration conf) {

            _configuration = conf;
            string connectionString;

            // Verifica el entorno
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "Production")
            {
                // En producción, usa la variable de entorno
                connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string not found in environment variables");
            }
            else
            {
                // En desarrollo, usa appsettings.Development.json
                connectionString = conf.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string not found in configuration");
            }

            Bussiness = new Bussiness(connectionString);
        }

        /// <summary>
        /// Testeo de conexion 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(Usuario usuario)
        {
            try
            {
                //1. Validacion de conexion con base de datos
                 dynamic response = await Bussiness.CrearActualizarUsuario(usuario);
                //2. Notificacion de exito
                return Ok(new { status = false, msj = "usuario - operacion exitosa ✅"});
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, new { status =false, msj = ex.Message });

            }
           
        }

        /// <summary>
        /// usuarios operaciones
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("operaciones")]
        public async Task<dynamic> UsuariosOperations(UsuarioOperation usuario)
        {
            try
            {
                //1. Validacion de conexion con base de datos
                dynamic response = await Bussiness.UsuariosOperations(usuario);
                //2. Notificacion de exito
                return Ok(new { status = true, msj = response });
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, new { status = false, msj = ex.Message });

            }
        }

    }
}
