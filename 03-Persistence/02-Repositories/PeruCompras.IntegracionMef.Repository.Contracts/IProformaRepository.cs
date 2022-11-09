using PeruCompras.IntegracionMef.DataModel;

namespace PeruCompras.IntegracionMef.Repository.Contracts
{
    public interface IProformaRepository
    {
        void ActualizarEstadoProcesamientoCotizacion(ProformaDTO proforma, int estadoProcesamiento);
        void ActualizarCantidadReprocesos(ProformaDTO proforma);        
        RespuestaProformaDTO GenerarProformas(ProformaDTO proforma);
        RespuestaProformaDTO ActualizarReservaRequerimiento(ProformaDTO proforma);
        IEnumerable<CreditoPresupuestarioDTO> ConsultarCertificadoPresupuestal(ProformaDTO proforma);
        void RegistrarLogError(LogServiceDTO logService);
        void ReversarProcesoCotizacion(ProformaDTO proforma);
        IEnumerable<MailDTO> ListarCorreoInformeError(string proceso);
        EntidadDTO ObtenerDatosEntidad(string IdPersonaJuridica);
        RespuestaMEFDTO ObtenerReprocesamientoRespuestaMEF(string codigoRespuestaMEF);
        ConfiguracionDTO ObtenerConfiguracion(string Key);
    }
}