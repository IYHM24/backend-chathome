using Dal;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDal
{
    public class DalService : DapperService
    {
        /// <summary>
        /// Constructor de la clase DalService
        /// </summary>
        /// <param name="_conexion_string"></param>
        public DalService(string _conexion_string) : base(_conexion_string) { } //base: instancia la clase padre 

        /// <summary>
        /// Operaciones de estadisticas del servidor
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<dynamic> toolsOperations(ToolsDto t)
        {
            return await ListQueryAsync<ToolsDto, dynamic>("[dbo].[Tools]", t);
        } 

        /// <summary>
        /// Actulizar o crear usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<dynamic> CrearActualizarUsuario(Usuario usuario)
        {
            return await ListQueryAsync<Usuario, dynamic>("[dbo].[CreateUser]", usuario);
        }

        /// <summary>
        /// Actulizar o crear usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<dynamic> UsuariosOperations(UsuarioOperation usuario)
        {
            return await ListQueryAsync<UsuarioOperation, dynamic>("[dbo].[UsuariosOperation]", usuario);
        }

        /// <summary>
        /// Operaciones de mensajes
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<dynamic> MensajesOperations(MensajeDto m)
        {
            return await ListQueryAsync<MensajeDto, dynamic>("[dbo].[MensajesOperations]", m);
        }

    }
}