namespace Models
{
    /// <summary>
    /// Clase que representa el cartero de mensajes. 
    /// Para mas especificos las estadisticas del mensaje
    /// </summary>
    public class Cartero
    {
        public int id { get; set; }
        public int id_mensaje { get; set; }
        public int id_archivo { get; set; }
        public bool visto { get; set; }
        public bool entregado { get; set; }
        public DateTime fecha_entrega { get; set; }
        public TimeOnly hora_entrega { get; set; }
    }
}
