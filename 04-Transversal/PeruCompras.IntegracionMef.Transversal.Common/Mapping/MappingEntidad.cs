using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingEntidad : AutoMapper.Profile
    {
        public MappingEntidad()
        {
            CreateMap<EntidadDTO, Entidad>().ReverseMap();
        }
    }
}