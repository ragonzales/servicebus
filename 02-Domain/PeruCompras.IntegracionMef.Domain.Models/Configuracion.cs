namespace PeruCompras.IntegracionMef.Domain.Models
{
    public class Configuracion
    {
        public int IdTableConfig { get; set; }
        public string Skey { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}