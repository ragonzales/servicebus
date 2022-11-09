using Microsoft.Extensions.Configuration;
using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingConfiguracion : AutoMapper.Profile
    {
        public MappingConfiguracion()
        {
            CreateMap<ConfiguracionDTO, Configuracion>().ReverseMap();
        }
    }   
}