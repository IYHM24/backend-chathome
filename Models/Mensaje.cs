namespace Models
{
    /// <summary>
    /// Clase que representa un mensaje.
    /// </summary>
    public class MensajeDto
    {
        public int? id { get; set; }
        public string? texto { get; set; }
        public int? mensajes_sin_leer { get; set; }
        public string? chat_id { get; set; }
        public int time { get; set; }
        public string? type { get; set; }
        public string? id_ultimo_mensaje { get; set; }
        public string? proceso { get; set; }
        public string? ui_usuario_remitente { get; set; }
    }
}
