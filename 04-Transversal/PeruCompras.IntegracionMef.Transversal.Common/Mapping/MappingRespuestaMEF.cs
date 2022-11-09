using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingRespuestaMEF : AutoMapper.Profile
    {
        public MappingRespuestaMEF()
        {
            CreateMap<RespuestaMEFDTO, RespuestaMEF>().ReverseMap();
        }
    }   
}