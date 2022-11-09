using Dapper;
using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Repository.Contracts;
using System.Data;
using System.Data.SqlClient;

namespace PeruCompras.IntegracionMef.Repository.SqlServer
{
    public class ProformaRepository : Repository, IProformaRepository
    {
        public ProformaRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction, int timeOut)
        {
            this._connection = sqlConnection;
            this._transaction = sqlTransaction;
            this._timeOut = timeOut;
        }

        public RespuestaProformaDTO GenerarProformas(ProformaDTO proforma)
        {
            var query = "PA_GeneraCotizacion";
            var queryParams = new DynamicParameters();
            queryParams.Add("@N_Requerimiento", proforma.N_Requerimiento);
            queryParams.Add("@N_CeamRequerimiento", proforma.N_CeamRequerimiento);
            queryParams.Add("@N_RequerimientoItem", proforma.N_RequerimientoItem);
            queryParams.Add("@N_AnioEjecucion", proforma.N_AnioEjecucion);
            queryParams.Add("@N_SecuenciaEjecutora", proforma.N_SecuenciaEjecutora);
            queryParams.Add("@Usuario", proforma.Usuario);
            queryParams.Add("@Cliente", proforma.Cliente);
            var result = _connection.Query<RespuestaProformaDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public RespuestaProformaDTO ActualizarReservaRequerimiento(ProformaDTO proforma)
        {
            proforma.N_FechaActualiza = 1;

            var query = "PA_Certificado_RegistraReserva";
            var queryParams = new DynamicParameters();
            queryParams.Add("@N_AnioEjecucion", proforma.N_AnioEjecucion);
            queryParams.Add("@N_SecuenciaEjecutora", proforma.N_SecuenciaEjecutora);
            queryParams.Add("@N_Requerimiento", proforma.N_Requerimiento);
            queryParams.Add("@N_FechaActualiza", 1);
            queryParams.Add("@A_CreacionUsuario", proforma.Usuario);
            var respuesta = _connection.Query<RespuestaProformaDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return respuesta;
        }

        public IEnumerable<CreditoPresupuestarioDTO> ConsultarCertificadoPresupuestal(ProformaDTO proforma)
        {
            var query = "PA_Certificado_Consultas";
            var queryParams = new DynamicParameters();
            queryParams.Add("@n_tipoConsulta", 1);
            queryParams.Add("@n_anioejecucion", proforma.N_AnioEjecucion);
            queryParams.Add("@n_secuenciaejecutora", proforma.N_SecuenciaEjecutora);
            queryParams.Add("@n_ceamrequerimiento", proforma.N_CeamRequerimiento);
            queryParams.Add("@n_expediente", 0);
            var results = _connection.Query<CreditoPresupuestarioDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).ToList();
            return results;
        }

        public void RegistrarLogError(LogServiceDTO logService)
        {
            var query = "PA_RegistrarLog_ServiceBus";
            var queryParams = new DynamicParameters();
            queryParams.Add("@N_CodigoServicio", logService.CodigoServicio);
            queryParams.Add("@C_NombreServicio", logService.NombreServicio);
            queryParams.Add("@C_NombreMetodo", logService.NombreMetodo);
            queryParams.Add("@C_Data", logService.Data);
            queryParams.Add("@C_Log", logService.Log);
            queryParams.Add("@A_CreacionUsuario", logService.CreacionUsuario);
            queryParams.Add("@A_HostCreacion", logService.HostCreacion);
            _connection.Execute(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<MailDTO> ListarCorreoInformeError(string proceso)
        {
            var query = "PA_ObtenerCorreosErrores_ServiceBus";
            var queryParams = new DynamicParameters();
            queryParams.Add("@Proceso", proceso);
            var results = _connection.Query<MailDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).ToList();
            return results;
        }

        public EntidadDTO ObtenerDatosEntidad(string IdPersonaJuridica)
        {
            var query = "PA_ObtenerCorreoEntidad";
            var queryParams = new DynamicParameters();
            queryParams.Add("@IdPersonaJuridica", IdPersonaJuridica);
            var result = _connection.Query<EntidadDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public RespuestaMEFDTO ObtenerReprocesamientoRespuestaMEF(string codigoRespuestaMEF)
        {
            var query = "PA_ObtenerRespuestMEF";
            var queryParams = new DynamicParameters();
            queryParams.Add("@CodigoRespuestaMEF", codigoRespuestaMEF);
            var result = _connection.Query<RespuestaMEFDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public void ReversarProcesoCotizacion(ProformaDTO proforma)
        {
            var query = "PA_ReversarProcesoCotizacion_ProcesoColas";
            var queryParams = new DynamicParameters();
            queryParams.Add("@N_Requerimiento", proforma.N_Requerimiento);
            queryParams.Add("@N_RequerimientoItem", proforma.N_RequerimientoItem);
            _connection.Execute(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure);
        }

        public ConfiguracionDTO ObtenerConfiguracion(string Key)
        {
            var query = "PA_ObtenerTableConfig";
            var queryParams = new DynamicParameters();
            queryParams.Add("@Skey", Key);
            var result = _connection.Query<ConfiguracionDTO>(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public void ActualizarEstadoProcesamientoCotizacion(ProformaDTO proforma, int estadoProcesamiento)
        {
            var query = "PA_ActualizarEstadoProcesamientoCotizacion";
            var queryParams = new DynamicParameters();
            queryParams.Add("@N_Requerimiento", proforma.N_Requerimiento);
            queryParams.Add("@N_RequerimientoItem", proforma.N_RequerimientoItem);
            queryParams.Add("@N_EstadoSeguimiento", estadoProcesamiento);
            _connection.Execute(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure);
        }

        public void ActualizarCantidadReprocesos(ProformaDTO proforma)
        {
            var query = "ActualizarCantidadReprocesos";
            var queryParams = new DynamicParameters();
            queryParams.Add("@N_Requerimiento", proforma.N_Requerimiento);
            queryParams.Add("@N_RequerimientoItem", proforma.N_RequerimientoItem);
            _connection.Execute(query, param: queryParams, transaction: _transaction, commandTimeout: _timeOut, commandType: CommandType.StoredProcedure);
        }
    }
}