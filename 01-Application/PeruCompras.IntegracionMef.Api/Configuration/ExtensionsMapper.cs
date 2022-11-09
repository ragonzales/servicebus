using AutoMapper;
using PeruCompras.IntegracionMef.Transversal.Common.Mapping;

namespace PeruCompras.IntegracionMef.Api.Configuration
{
    public static class ExtensionsMapper
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingRespuestaProforma());
                mc.AddProfile(new MappingCreditoPresupuestario());
                mc.AddProfile(new MappingRespuestaMEF());
                mc.AddProfile(new MappingLogService());
                mc.AddProfile(new MappingProforma());
                mc.AddProfile(new MappingEntidad());
                mc.AddProfile(new MappingMail());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}