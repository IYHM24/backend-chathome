namespace Models
{
    /// <summary>
    /// Si el mensaje contiene algun adjunto
    /// </summary>
    public class Archivo
    {
        public int id_mensaje { get; set; }
        public int id_usuario { get; set; }
        public string? nombre { get; set; }
        public string? tipo { get; set; }
        public string? file { get; set; }
        public double tamaño { get; set; }
        public bool es_foto_perfil { get; set; }
        public DateTime fecha_de_carga { get; set; }
    }
}
