using ApiDal;
using Models;

namespace ApiBussines
{
    public class Bussiness
    {
        private DalService dal;

        /// <summary>
        /// Constructor de la clase Bussiness
        /// </summary>
        /// <param name="_conexion_string"></param>
        public Bussiness(string _conexion_string) { dal = new DalService(_conexion_string); }

        /// <summary>
        /// Operaciones de estadisticas del servidor
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<dynamic> toolsOperationsAsync(ToolsDto t)
        {
            return await dal.toolsOperations(t);
        }

        /// <summary>
        /// Actulizar o crear usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<dynamic> CrearActualizarUsuario(Usuario usuario)
        {
            return await dal.CrearActualizarUsuario(usuario);
        }


        /// <summary>
        /// Actulizar o crear usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<dynamic> UsuariosOperations(UsuarioOperation usuario)
        {
            return await dal.UsuariosOperations(usuario);
        }

        /// <summary>
        /// Operaciones relacionadas con mensajes
        /// </summary>
        /// <param name="mensaje">Objeto que representa el mensaje</param>
        /// <returns>Resultado de la operación</returns>
        public async Task<dynamic> MensajesOperations(MensajeDto mensaje)
        {
            return await dal.MensajesOperations(mensaje);
        }



    }
}
