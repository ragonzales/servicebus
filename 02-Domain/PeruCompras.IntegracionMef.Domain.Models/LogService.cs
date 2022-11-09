namespace PeruCompras.IntegracionMef.Domain.Models
{
    public class LogService
    {
        public long? CodigoServicio { get; set; }
        public string NombreServicio { get; set; }
        public string NombreMetodo { get; set; }
        public string Data { get; set; }
        public string Log { get; set; }
        public string CreacionUsuario { get; set; }
        public string HostCreacion { get; set; }
    }
}