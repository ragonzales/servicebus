using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingCreditoPresupuestario : AutoMapper.Profile
    {
        public MappingCreditoPresupuestario()
        {
            CreateMap<CreditoPresupuestarioDTO, CreditoPresupuestario>().ReverseMap();
        }
    }   
}