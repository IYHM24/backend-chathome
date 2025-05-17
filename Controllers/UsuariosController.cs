using ApiBussines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        //Constructor
        public UsuariosController(IConfiguration conf) {

            Bussiness = new Bussiness(conf.GetConnectionString("DefaultConnection")!.ToString());
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
