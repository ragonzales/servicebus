using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingRespuestaProforma : AutoMapper.Profile
    {
        public MappingRespuestaProforma()
        {
            CreateMap<RespuestaProformaDTO, RespuestaProforma>().ReverseMap();
        }
    }   
}