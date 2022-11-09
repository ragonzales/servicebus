namespace PeruCompras.IntegracionMef.DataModel
{
    public class RespuestaMEFDTO
    {
        public string IdRespuestaServicioMEF { get; set; }
        public string CodigoError { get; set; }
        public string Descripcion { get; set; }
        public bool Reproceso { get; set; }
    }
}