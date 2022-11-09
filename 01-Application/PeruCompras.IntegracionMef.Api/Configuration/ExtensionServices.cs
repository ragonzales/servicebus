using PeruCompras.IntegracionMef.Api.Events.Handlers;
using PeruCompras.IntegracionMef.DataBinding.Models;
using PeruCompras.IntegracionMef.Domain.Services;
using PeruCompras.IntegracionMef.Domain.Services.Contracts;
using PeruCompras.IntegracionMef.Transversal.Common;
using PeruCompras.IntegracionMef.Transversal.Common.Resources;
using PeruCompras.IntegracionMef.UnitOfWork.Contracts;
using PeruCompras.IntegracionMef.UnitOfWork.SqlServer;

namespace PeruCompras.IntegracionMef.Api.Configuration
{
    public static class ExtensionServices
    {
        private static string cadenaConexion = string.Empty;
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //CADENA DE CONEXION
            if (string.IsNullOrEmpty(cadenaConexion))
            {
                cadenaConexion = KeyVault.ObtenerKeyVault(configuration.GetValue<string>(Constantes.BD_KEYVAULTURI), configuration.GetValue<string>(Constantes.BD_KEYVAULTSECRET));
            }

            int timeOut = configuration.GetValue<int>(Constantes.BD_TIMEOUT);
            services.AddTransient<IUnitOfWork, UnitOfWorkSqlServer>(x => new UnitOfWorkSqlServer(cadenaConexion, timeOut));

            //Add services
            services.AddSingleton<IServiceBus, ServiceBus>();
            services.AddSingleton<IHandler<ProformaBindingModel>, ReservaEventHandler>();
            //services.AddTransient<IProformaService, ProformaService>();

            services.AddTransient<IProformaService, ProformaService>();
            return services;
        }
    }
}