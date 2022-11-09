namespace PeruCompras.IntegracionMef.Transversal.Common
{
    public static class Constantes
    {
        //CadenaConexion        
        public const string BD_TIMEOUT = "CadenaConexionDB:TimeOutBD";
        public const string BD_KEYVAULTURI = "CadenaConexionDB:KeyVaultURI";
        public const string BD_KEYVAULTSECRET = "CadenaConexionDB:KeyVaultSecret";

        //CadenaConexion SERVICEBUS
        public const string SERVICEBUS_KEYVAULTURI = "ServiceBus:KeyVaultURI";
        public const string SERVICEBUS_KEYCOLA = "ServiceBus:KeyCola";
        public const string SERVICEBUS_KEYVAULTSECRET = "ServiceBus:KeyVaultSecret";

        //Service MEF 
        public const string SERVICEMEF_URLSERVICE = "ServiceMEF:UrlService";
        public const string SERVICEMEF_USER = "ServiceMEF:Usuario";
        public const string SERVICEMEF_PASSWORD = "ServiceMEF:Password";
        public const string SERVICEMEF_TIMEOUT_HORAS = "ServiceMEF:TimeOut_Horas";
        public const string SERVICEMEF_TIMEOUT_MINUTOS = "ServiceMEF:TimeOut_Minutos";
        public const string SERVICEMEF_TIMEOUT_SEGUNDOS = "ServiceMEF:TimeOut_Segundos";

        //Header Service
        public const string HEADER_SERVICE_USER = "Usuario";
        public const string HEADER_SERVICE_PASSWORD = "Clave";

        //SMTP Service : SENDGRID.NET 
        public const string SMTP_SENDGRIDNET_FROM = "SMTP_SENDGRIDNET:From";
        public const string SMTP_SENDGRIDNET_HOST = "SMTP_SENDGRIDNET:Host";
        public const string SMTP_SENDGRIDNET_PORT = "SMTP_SENDGRIDNET:Port";
        public const string SMTP_SENDGRIDNET_ALIAS = "SMTP_SENDGRIDNET:Alias";
        public const string SMTP_SENDGRIDNET_USERNAME = "SMTP_SENDGRIDNET:UserName";
        public const string SMTP_SENDGRIDNET_PASSWORD = "SMTP_SENDGRIDNET:Password";

        //Variables del sitema
        public const string PROCESO_CORREO_CATALOGO = "CATALOGO-SERVICEBUS-MEF";
        public const string HORA_PROCESAMIENTO = "[HORA PROCESAMIENTO]";
        public const string CODIGO_RESPUESTA_OK_SERVICIO = "0";
        public const string INCIAL_REQUERIMIENTO = "REQ";
        public const int ACTUALIZAR_RESERVA_PROFORMAS = 0;
        public const int RESPUESTA_OK_SERVICIO_MEF = 0;
        public const int GENERACION_OK_PROFORMAS = 0;
        public const int NO_ENVIADO_MEF = 0;
        public const int SI_APLICA_SIAF = 1;
        public const int CODIGO_ERROR_SERVICIO = -1;
        public const int CODIGO_ERROR_VALIDACION_NEGOCIO = -2;
        public const int CODIGO_CONTINUAR_PROCESO = 0;

        //Varible codigos de seguimiento del proceso
        public const int CODIGO_SEGUIMIENTO_ENVIOACOLA = 1;
        public const int CODIGO_SEGUIMIENTO_PROCESANDO = 2;
        public const int CODIGO_SEGUIMIENTO_PROCESADO_OK = 3;
        public const int CODIGO_SEGUIMIENTO_PROCESADO_ERROR = 4;
        public const int CODIGO_SEGUIMIENTO_PROCESADO_ENVIAR_REPROCESAR = 5;

        //Variables para envio de correos(error y correcto)
        public const string CONFIGURACION_CONTENIDOMENSAJE_INCORRECTO = "CONTENIDOMENSAJE_INCORRECTO";
        public const string CONFIGURACION_MESSAGE_ERROR_NOCONTROLADO = "MESSAGE_ERROR_NOCONTROLADO";
        public const string CONFIGURACION_CONTENIDOMENSAJE_CORRECTO = "CONTENIDOMENSAJE_CORRECTO";
        public const string CONFIGURACION_MESSAGE_ERROR_DESCONOCIDO = "MESSAGE_ERROR_DESCONOCIDO";
        public const string CONFIGURACION_CUERPOHTML_PROCESOCOLAS = "CUERPOHTML_PROCESOCOLAS";
        public const string CONFIGURACION_CUERPOHTML_ERRORES = "CUERPOHTML_ERRORES";        
        public const string CONFIGURACION_MESSAGE_ERROR_SERVICIO = "MESSAGE_ERROR_SERVICIO";        
        public const string CONFIGURACION_ASUNTO_ERROR = "ASUNTO_ERROR";
        public const string LOG_NOMBRESERVICIO = "MEFPERUCOMPRASWS";
        public const string CONFIGURACION_ASUNTO_OK = "ASUNTO_OK";
        public const string LOG_NOMBREMETODO = "ReservaCCP";
        public const int LOG_CODIGOSERVICIO = 100;

        //Variables Secciones
        public const string SECCION_MENSAJE_CONTENIDO_CORREO_OK = "#MENSAJE_CONTENIDO_CORREO_OK";
        public const string SECCION_CODIGOMENSAJERESPUESTA = "#CODIGOMENSAJERESPUESTA";
        public const string SECCION_SECUENCIAEJECUTORA = "#SECUENCIAEJECUTORA";
        public const string SECCION_CEAMREQUERIMIENTO = "#CEAMREQUERIMIENTO";
        public const string SECCION_REQUERIMIENTOITEM = "#REQUERIMIENTOITEM";
        public const string SECCION_ANIO_EJECUCION = "#ANIOEJECUCION";        
        public const string SECCION_REQUERIMIENTO = "#REQUERIMIENTO";
        public const string SECCION_PROCESAMIENTO = "#PROCESAMIENTO";
        public const string SECCION_MENSAJE_ERROR = "#MENSAJE_ERROR";
        public const string SECCION_NOMBRE_USUARIO = "#USUARIO";
        public const string SECCION_NOMBRE_CLIENTE = "#CLIENTE";
        public const string SECCION_STACKTRACE = "#STACKTRACE";
        public const string SECCION_ENTIDAD = "#ENTIDAD";
        public const string SECCION_MESSAGE = "#MESSAGE";
        public const string SECCION_SOURCE = "#SOURCE";

        //Sub Estados proceso cotización
        public const int ESTADOCOTIZACION_ENVIOCOLA = 1;
        public const int ESTADOCOTIZACION_PROCESANDO = 2;
        public const int ESTADOCOTIZACION_FINALIZADO_OK = 3;
        public const int ESTADOCOTIZACION_FINALIZADO_ERROR = 4;
    }
}