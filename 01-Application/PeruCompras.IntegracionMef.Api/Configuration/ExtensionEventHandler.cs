using Microsoft.Azure.ServiceBus;
using PeruCompras.IntegracionMef.Api.Events.Handlers;
using PeruCompras.IntegracionMef.DataBinding.Models;
using PeruCompras.IntegracionMef.Transversal.Common;
using PeruCompras.IntegracionMef.Transversal.Common.Resources;
using System.Text;
using System.Text.Json;

namespace PeruCompras.IntegracionMef.Api.Configuration
{
    public static class ExtensionEventHandler
    {
        private static string cadenaConexionServiceBus = string.Empty;
        public static void UseEventHandler(this IApplicationBuilder app, IConfiguration configuration)
        {
            var receiver = app.ApplicationServices.GetService<IServiceBus>();
            var reservaEventHandler = app.ApplicationServices.GetService<IHandler<ProformaBindingModel>>();
            Register(receiver, configuration, reservaEventHandler);
        }

        private static void Register<T>(IServiceBus service, IConfiguration configuration, IHandler<T> handler) where T : class
        {
            //CADENA DE CONEXION
            if (string.IsNullOrEmpty(cadenaConexionServiceBus))
            {
                cadenaConexionServiceBus = KeyVault.ObtenerKeyVault(configuration.GetValue<string>(Constantes.SERVICEBUS_KEYVAULTURI), configuration.GetValue<string>(Constantes.SERVICEBUS_KEYVAULTSECRET));
            }

            var client = service.GetQueueClient(cadenaConexionServiceBus, configuration.GetValue<string>(Constantes.SERVICEBUS_KEYCOLA));
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            client.RegisterMessageHandler(async (Message message, CancellationToken token) =>
            {
                var payload = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(message.Body));
                await client.CompleteAsync(message.SystemProperties.LockToken);
                await handler.Execute(payload);
            }, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            // your custom message log
            return Task.CompletedTask;
        }
    }
}