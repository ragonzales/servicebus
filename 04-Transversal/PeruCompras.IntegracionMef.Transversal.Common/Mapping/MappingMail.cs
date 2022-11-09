using PeruCompras.IntegracionMef.DataModel;
using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Mapping
{
    public class MappingMail : AutoMapper.Profile
    {
        public MappingMail()
        {
            CreateMap<MailDTO, Mail>().ReverseMap();
        }
    }   
}