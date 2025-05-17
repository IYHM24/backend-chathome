using ApiBussines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace WebSockets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToolsController : ControllerBase
    {
        private Bussiness Bussiness;

        //Constructor
        public ToolsController(IConfiguration conf) {

            Bussiness = new Bussiness(conf.GetConnectionString("DefaultConnection")!.ToString());
        }

        /// <summary>
        /// Testeo de conexion 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //1. Validacion de conexion con base de datos
                 dynamic response = await Bussiness.toolsOperationsAsync(new ToolsDto() { proceso = "test" });

                //2. Notificacion de exito
                return Ok(new { status = false, msj = "conexion exitosa ✅ respuesta: " + response });
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, new { status =false, msj = ex.Message });

            }
           
        }

    }
}
