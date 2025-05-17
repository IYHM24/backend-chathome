namespace Models
{

    /// <summary>
    /// El usuario que envia el mensaje o recibe el mensaje
    /// </summary>
    public class UsuarioOperation
    {
        public int id { get; set; }
        public string ? nombre { get; set; }
        public string ? apellido { get; set; }
        public string ? nombre_usuario { get; set; }
        public bool online { get; set; }
        public string ? token_google { get; set; }
        public string ? uuid_google { get; set; }
        public string ? correo { get; set; }
        public string ? foto_perfil { get; set; }
        public string ? sesion { get; set; }
        public string ? contextId { get; set; }
        public DateTime? fecha_ultima_conexion { get; set; }
        public string ? proceso { get; set; }
    }
}
