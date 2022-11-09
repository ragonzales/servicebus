namespace PeruCompras.IntegracionMef.DataModel
{
    public class CreditoPresupuestarioDTO
    {
        public long? N_Expediente { get; set; }
        public long? N_Numeroexpediente { get; set; }
        public int? N_Secuencia { get; set; }
        public string C_Rubro { get; set; }
        public string C_Numerodocumento { get; set; }
        public string A_Creacionfecha { get; set; }
        public string C_Moneda { get; set; }
        public decimal? N_Reservamonto { get; set; }
    }
}