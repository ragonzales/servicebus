using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;
using PeruCompras.IntegracionMef.Domain.Services.Contracts;
using PeruCompras.IntegracionMef.Transversal.Common;
using PeruCompras.IntegracionMef.Transversal.Common.Models;
using PeruCompras.IntegracionMef.Transversal.Common.Resources;
using PeruCompras.IntegracionMef.UnitOfWork.Contracts;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using static ServiceMEF.WsmefperucomprasWsClient;

namespace PeruCompras.IntegracionMef.Domain.Services
{
    public class ProformaService : IProformaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProformaService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task GenerarProformas(Proforma proforma)
        {
            
            var proformaDTO = _mapper.Map<ProformaDTO>(proforma);
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            

            //public const int ESTADOCOTIZACION_ENVIOCOLA = 1;
            //public const int ESTADOCOTIZACION_PROCESANDO = 2;
            //public const int ESTADOCOTIZACION_FINALIZADO_OK = 3;
            //public const int ESTADOCOTIZACION_FINALIZADO_ERROR = 4;

            var lstDestinatarioErrores = context.UnitOfWorkRepository.ProformaRepository.ListarCorreoInformeError(Constantes.PROCESO_CORREO_CATALOGO);
            var entidad = context.UnitOfWorkRepository.ProformaRepository.ObtenerDatosEntidad(proformaDTO.IdPersonaJuridica);
            string numeroRequerimiento = $"{Constantes.INCIAL_REQUERIMIENTO}-{proformaDTO.N_AnioEjecucion}-{proformaDTO.N_SecuenciaEjecutora}-{proformaDTO.N_CeamRequerimiento}";

            string mensajeErrorProcesoTransacion = string.Empty;
            RespuestaProforma response = new();
            var dataService = string.Empty;
            var reenviarCola = false;

            try
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                ServiceMEF.reservaCcpOutputForm requestWS_Respuesta = new();                
                ServiceMEF.reservaCcpInputForm requestWS = new();                
                var msgService = string.Empty;

                context.UnitOfWorkRepository.ProformaRepository.ActualizarEstadoProcesamientoCotizacion(proformaDTO, Constantes.ESTADOCOTIZACION_PROCESANDO);

                var resultGeneracionProformas = context.UnitOfWorkRepository.ProformaRepository.GenerarProformas(proformaDTO);
                if (resultGeneracionProformas.Cod_rpta == Constantes.GENERACION_OK_PROFORMAS && resultGeneracionProformas.N_AplicaSiaf == Constantes.SI_APLICA_SIAF && resultGeneracionProformas.N_EnviadoMef == Constantes.NO_ENVIADO_MEF)
                {
                    var resultActualizar_Inicio = context.UnitOfWorkRepository.ProformaRepository.ActualizarReservaRequerimiento(proformaDTO);
                    if (resultActualizar_Inicio.Cod_rpta == Constantes.ACTUALIZAR_RESERVA_PROFORMAS)
                    {
                        var lstCertificadoPresupuesalDTO = context.UnitOfWorkRepository.ProformaRepository.ConsultarCertificadoPresupuestal(proformaDTO);
                        var lstCertificadoPresupuesal = _mapper.Map<IEnumerable<CreditoPresupuestario>>(lstCertificadoPresupuesalDTO);
                        try
                        {
                            ServiceMEF.WsmefperucomprasWsClient ws = new(EndpointConfiguration.WsmefperucomprasWsImplPort, _configuration.GetSection(Constantes.SERVICEMEF_URLSERVICE).Value);
                            GenerarEnpointBinding(ref ws);

                            requestWS = GenerarRequestServiceMEF(proformaDTO, lstCertificadoPresupuesal);
                            Task<ServiceMEF.reservaCcpOutputForm> task;
                            using (new OperationContextScope(ws.InnerChannel))
                            {
                                HttpRequestMessageProperty requestMessage = new();
                                requestMessage.Headers[Constantes.HEADER_SERVICE_USER] = _configuration.GetSection(Constantes.SERVICEMEF_USER).Value;
                                requestMessage.Headers[Constantes.HEADER_SERVICE_PASSWORD] = _configuration.GetSection(Constantes.SERVICEMEF_PASSWORD).Value;
                                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                                task = ws.reservaCCPAsync(requestWS);
                            }

                            requestWS_Respuesta = await task;
                            if (requestWS_Respuesta == null)
                            {
                                response.Cod_rpta = Constantes.CODIGO_ERROR_SERVICIO;
                                response.Mensaje_rpta = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_MESSAGE_ERROR_DESCONOCIDO).Valor;
                            }
                            else
                            {
                                if (requestWS_Respuesta.codRpta != Constantes.CODIGO_RESPUESTA_OK_SERVICIO)
                                {
                                    var respuestaMefError = context.UnitOfWorkRepository.ProformaRepository.ObtenerReprocesamientoRespuestaMEF(requestWS_Respuesta.codRpta);
                                    reenviarCola = respuestaMefError.Reproceso;
                                    response.Cod_rpta = Constantes.CODIGO_ERROR_SERVICIO;
                                    response.Mensaje_rpta = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_MESSAGE_ERROR_SERVICIO).Valor.Replace(Constantes.SECCION_CODIGOMENSAJERESPUESTA, requestWS_Respuesta.codRpta).Replace(Constantes.SECCION_MESSAGE, requestWS_Respuesta.mensajeRpta);

                                    if (!reenviarCola)
                                    {
                                        mensajeErrorProcesoTransacion = requestWS_Respuesta.mensajeRpta;
                                        response.Cod_rpta = Constantes.CODIGO_ERROR_VALIDACION_NEGOCIO;
                                    }
                                }
                            }
                        }
                        catch (TimeoutException exTimeOut)
                        {
                            response.Cod_rpta = Constantes.CODIGO_CONTINUAR_PROCESO;
                            response.Mensaje_rpta = ObtenerMensajeError(exTimeOut, context);
                        }
                        catch (Exception ex)
                        {
                            response.Cod_rpta = Constantes.CODIGO_ERROR_SERVICIO;
                            response.Mensaje_rpta = ObtenerMensajeError(ex, context);
                            reenviarCola = true;
                        }
                        dataService = JsonConvert.SerializeObject(requestWS);
                        msgService = response.Mensaje_rpta;
                    }
                    else
                    {
                        response.Cod_rpta = Constantes.CODIGO_ERROR_VALIDACION_NEGOCIO;
                        mensajeErrorProcesoTransacion = resultActualizar_Inicio.Mensaje_rpta;
                    }
                }
                else
                {
                    if (resultGeneracionProformas.Cod_rpta != Constantes.GENERACION_OK_PROFORMAS)
                    {
                        response.Cod_rpta = Constantes.CODIGO_ERROR_VALIDACION_NEGOCIO;
                        mensajeErrorProcesoTransacion = resultGeneracionProformas.Mensaje_rpta;
                    }
                }

                var listaPara = new List<string>();
                if (!string.IsNullOrEmpty(entidad.Email) && !entidad.Email.Trim().Equals(string.Empty)) listaPara.Add(entidad.Email);

                switch (response.Cod_rpta)
                {
                    case Constantes.CODIGO_ERROR_SERVICIO:
                        context.Rollback();
                        GenerarLogServicio(dataService, response.Mensaje_rpta, proforma.Usuario, proforma.Cliente);
                        context.UnitOfWorkRepository.ProformaRepository.ActualizarEstadoProcesamientoCotizacion(proformaDTO, Constantes.ESTADOCOTIZACION_FINALIZADO_ERROR);
                        var correoHtmlErrorServicio = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_CUERPOHTML_PROCESOCOLAS).Valor;
                        correoHtmlErrorServicio = ArmarCuerpoCorreoError(proforma, correoHtmlErrorServicio,response.Mensaje_rpta);
                        EnviarRespuestaElectronico(ObtenerListaDestinatorio(lstDestinatarioErrores), ObtenerAsuntoError(numeroRequerimiento, context), correoHtmlErrorServicio);                        
                        break;

                    case Constantes.CODIGO_ERROR_VALIDACION_NEGOCIO:
                        context.Rollback();
                        ReversarProcesoCotizacion(proformaDTO);
                        var asunto = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_ASUNTO_OK).Valor.Replace(Constantes.SECCION_REQUERIMIENTO, numeroRequerimiento);
                        var contenidoCorreo = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_CONTENIDOMENSAJE_INCORRECTO).Valor.Replace(Constantes.SECCION_REQUERIMIENTO, numeroRequerimiento).Replace(Constantes.SECCION_ENTIDAD, proforma.RazonSocial).Replace(Constantes.SECCION_MENSAJE_ERROR, mensajeErrorProcesoTransacion);
                        var correoHtml = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_CUERPOHTML_PROCESOCOLAS).Valor.Replace(Constantes.SECCION_MENSAJE_CONTENIDO_CORREO_OK, contenidoCorreo).Replace(Constantes.SECCION_NOMBRE_USUARIO, textInfo.ToTitleCase(entidad.NombreUsuario.ToLower()));
                        EnviarRespuestaElectronico(listaPara, asunto, correoHtml);
                        break;

                    default:
                        context.UnitOfWorkRepository.ProformaRepository.ActualizarEstadoProcesamientoCotizacion(proformaDTO, Constantes.ESTADOCOTIZACION_FINALIZADO_OK);
                        context.SaveChanges();
                        var asuntoOK = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_ASUNTO_OK).Valor.Replace(Constantes.SECCION_REQUERIMIENTO, numeroRequerimiento);
                        var contenidoCorreoOK = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_CONTENIDOMENSAJE_CORRECTO).Valor.Replace(Constantes.SECCION_REQUERIMIENTO, numeroRequerimiento).Replace(Constantes.SECCION_ENTIDAD, proforma.RazonSocial);
                        var correoHtmlOK = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_CUERPOHTML_PROCESOCOLAS).Valor.Replace(Constantes.SECCION_MENSAJE_CONTENIDO_CORREO_OK, contenidoCorreoOK).Replace(Constantes.SECCION_NOMBRE_USUARIO, textInfo.ToTitleCase(entidad.NombreUsuario.ToLower()));
                        EnviarRespuestaElectronico(listaPara, asuntoOK, correoHtmlOK);
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Rollback();
                response.Mensaje_rpta = ObtenerMensajeError(ex, context);
                GenerarLogServicio(dataService, response.Mensaje_rpta, proforma.Usuario, proforma.Cliente);
                var correoHtmlErrorNoControlado = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_CUERPOHTML_PROCESOCOLAS).Valor;
                correoHtmlErrorNoControlado = ArmarCuerpoCorreoError(proforma, correoHtmlErrorNoControlado, response.Mensaje_rpta);
                EnviarRespuestaElectronico(ObtenerListaDestinatorio(lstDestinatarioErrores), ObtenerAsuntoError(numeroRequerimiento, context), correoHtmlErrorNoControlado);
                reenviarCola = true;
            }

            if (reenviarCola)
            {
                ActualizarCantidadReprocesos(proformaDTO);
                await EnviarMensajeServiceBus(proforma);
            }
        }
        private void ActualizarCantidadReprocesos(ProformaDTO proformaDTO)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            context.UnitOfWorkRepository.ProformaRepository.ActualizarCantidadReprocesos(proformaDTO);
            context.SaveChanges();
        }

        private void ReversarProcesoCotizacion(ProformaDTO proformaDTO)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            context.UnitOfWorkRepository.ProformaRepository.ActualizarEstadoProcesamientoCotizacion(proformaDTO, Constantes.ESTADOCOTIZACION_FINALIZADO_ERROR);
            context.UnitOfWorkRepository.ProformaRepository.ReversarProcesoCotizacion(proformaDTO);
            context.SaveChanges();
        }

        private static string ObtenerMensajeError(Exception ex, IUnitOfWorkAdapter context)
        {
            var mensajeError = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_MESSAGE_ERROR_NOCONTROLADO).Valor.Replace(Constantes.SECCION_MESSAGE, ex.Message).Replace(Constantes.SECCION_SOURCE, ex.Source).Replace(Constantes.SECCION_STACKTRACE, ex.StackTrace);
            return mensajeError;
        }

        private static string ObtenerAsuntoError(string requerimiento, IUnitOfWorkAdapter context)
        {
            string procesamiento = $"{Constantes.HORA_PROCESAMIENTO} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            var asuntoError = context.UnitOfWorkRepository.ProformaRepository.ObtenerConfiguracion(Constantes.CONFIGURACION_ASUNTO_ERROR).Valor.Replace(Constantes.SECCION_REQUERIMIENTO, requerimiento).Replace(Constantes.SECCION_PROCESAMIENTO, procesamiento);
            return asuntoError;
        }

        private static List<string> ObtenerListaDestinatorio(IEnumerable<MailDTO> listaMails)
        {
            List<string> listDestinatarios = new();
            foreach (MailDTO destinatario in listaMails)
            {
                listDestinatarios.Add(destinatario.Correo);
            }
            return listDestinatarios;
        }

        private static ServiceMEF.reservaCcpInputForm GenerarRequestServiceMEF(ProformaDTO proforma, IEnumerable<CreditoPresupuestario> lstCertificadoPresupuesal)
        {
            var requestWS = new ServiceMEF.reservaCcpInputForm()
            {
                anoEje = proforma.N_AnioEjecucion.ToString(),
                ceamRequerimiento = proforma.N_CeamRequerimiento.ToString(),
                detalle = GenerarDetalleReserva(lstCertificadoPresupuesal).ToArray(),
                secEjec = proforma.N_SecuenciaEjecutora.ToString(),
                tipoReserva = proforma.Tipo
            };
            return requestWS;
        }

        private void GenerarEnpointBinding(ref ServiceMEF.WsmefperucomprasWsClient wsClient)
        {
            int horas = Convert.ToInt32(_configuration.GetSection(Constantes.SERVICEMEF_TIMEOUT_HORAS).Value);
            int minutos = Convert.ToInt32(_configuration.GetSection(Constantes.SERVICEMEF_TIMEOUT_MINUTOS).Value);
            int segundos = Convert.ToInt32(_configuration.GetSection(Constantes.SERVICEMEF_TIMEOUT_SEGUNDOS).Value);
            var tiempo = new TimeSpan(horas, minutos, segundos);

            wsClient.Endpoint.Binding.OpenTimeout = tiempo;
            wsClient.Endpoint.Binding.CloseTimeout = tiempo;
            wsClient.Endpoint.Binding.ReceiveTimeout = tiempo;
            wsClient.Endpoint.Binding.SendTimeout = tiempo;
        }

        private static List<ServiceMEF.reservaCcpDto> GenerarDetalleReserva(IEnumerable<CreditoPresupuestario> lstCertificadoPresupuesal)
        {
            List<ServiceMEF.reservaCcpDto> detalleClasificador = new();
            foreach (CreditoPresupuestario item in lstCertificadoPresupuesal)
            {
                var reserva = new ServiceMEF.reservaCcpDto()
                {
                    certificado = item.N_Numeroexpediente.Value,
                    certificadoSpecified = true,
                    montoReserva = item.N_Reservamonto.Value,
                    montoReservaSpecified = true,
                    secuencia = item.N_Secuencia.Value,
                    secuenciaSpecified = true
                };
                detalleClasificador.Add(reserva);
            }
            return detalleClasificador;
        }

        private void GenerarLogServicio(string dataService, string msgRespuesta, string usuario, string ipCliente)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            LogServiceDTO logDTO = new()
            {
                CodigoServicio = Constantes.LOG_CODIGOSERVICIO,
                NombreServicio = Constantes.LOG_NOMBRESERVICIO,
                NombreMetodo = Constantes.LOG_NOMBREMETODO,
                Data = dataService,
                Log = msgRespuesta,
                CreacionUsuario = usuario,
                HostCreacion = ipCliente
            };

            context.UnitOfWorkRepository.ProformaRepository.RegistrarLogError(logDTO);
            context.SaveChanges();
        }

        private static string ArmarCuerpoCorreoError(Proforma proforma,string formatoHtml, string mensajeError)
        {
            string cuerpoHtml = formatoHtml;
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_REQUERIMIENTO, proforma.N_Requerimiento.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_CEAMREQUERIMIENTO, proforma.N_CeamRequerimiento.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_REQUERIMIENTOITEM, proforma.N_RequerimientoItem.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_ANIO_EJECUCION, proforma.N_AnioEjecucion.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_SECUENCIAEJECUTORA, proforma.N_SecuenciaEjecutora.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_NOMBRE_USUARIO, proforma.Usuario.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_NOMBRE_CLIENTE, proforma.Cliente.ToString());
            cuerpoHtml = cuerpoHtml.Replace(Constantes.SECCION_MENSAJE_ERROR, mensajeError);
            return cuerpoHtml;
        }

        private void EnviarRespuestaElectronico(List<string> listaPara, string asunto, string cuerpoMensaje)
        {
            var enviaCorreo = false;
            Email email = new()
            {
                Subject = asunto,
                Message = cuerpoMensaje,
                From = _configuration.GetSection(Constantes.SMTP_SENDGRIDNET_FROM).Value,
                Host = _configuration.GetSection(Constantes.SMTP_SENDGRIDNET_HOST).Value,
                Alias = _configuration.GetSection(Constantes.SMTP_SENDGRIDNET_ALIAS).Value,
                User = _configuration.GetSection(Constantes.SMTP_SENDGRIDNET_USERNAME).Value,
                Password = _configuration.GetSection(Constantes.SMTP_SENDGRIDNET_PASSWORD).Value,
                Port = Convert.ToInt32(_configuration.GetSection(Constantes.SMTP_SENDGRIDNET_PORT).Value),
                IsBodyHtml = true
            };

            foreach (string para in listaPara)
            {
                enviaCorreo = true;
                email.To ??= new List<string>();
                email.To.Add(para);
            }

            if (enviaCorreo) EmailHelper.SendEmail(email);
        }

        private async Task EnviarMensajeServiceBus(Proforma request)
        {
            var cadenaConexionSB = KeyVault.ObtenerKeyVault(_configuration.GetSection(Constantes.SERVICEBUS_KEYVAULTURI).Value, _configuration.GetSection(Constantes.SERVICEBUS_KEYVAULTSECRET).Value);
            var clientOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
            var client = new ServiceBusClient(cadenaConexionSB, clientOptions);
            var sender = client.CreateSender(_configuration.GetSection(Constantes.SERVICEBUS_KEYCOLA).Value);
            var json = JsonConvert.SerializeObject(request);
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            messageBatch.TryAddMessage(new ServiceBusMessage(json));
            try
            {
                await sender.SendMessagesAsync(messageBatch);
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}