namespace PeruCompras.IntegracionMef.DataModel
{
    public class RespuestaProformaDTO
    {
        public bool Respuesta { get; set; }
        public int Cod_rpta { get; set; }
        public string Mensaje_rpta { get; set; }
        public int N_AplicaSiaf { get; set; }
        public int N_EnviadoMef { get; set; }
    }
}