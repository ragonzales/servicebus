namespace PeruCompras.IntegracionMef.DataBinding.Models
{
    public class ProformaBindingModel
    {
        public long? N_Requerimiento { get; set; }
        public int? N_CeamRequerimiento { get; set; }
        public long? N_RequerimientoItem { get; set; }
        public int? N_AnioEjecucion { get; set; }
        public int? N_SecuenciaEjecutora { get; set; }
        public int? N_FechaActualiza { get; set; }
        public string Tipo { get; set; }
        public string Usuario { get; set; }
        public string Cliente { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string IdPersonaJuridica { get; set; }
    }
}