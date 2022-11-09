using PeruCompras.IntegracionMef.DataBinding.Models;
using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingProforma : AutoMapper.Profile
    {
        public MappingProforma()
        {
            CreateMap<ProformaDTO, Proforma>().ReverseMap();
            CreateMap<Proforma, ProformaBindingModel>().ReverseMap();
        }
    }   
}