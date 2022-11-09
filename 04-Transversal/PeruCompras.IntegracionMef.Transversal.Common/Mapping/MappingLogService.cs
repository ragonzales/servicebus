using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingLogService : AutoMapper.Profile
    {
        public MappingLogService()
        {
            CreateMap<LogServiceDTO, LogService>().ReverseMap();
        }
    }   
}