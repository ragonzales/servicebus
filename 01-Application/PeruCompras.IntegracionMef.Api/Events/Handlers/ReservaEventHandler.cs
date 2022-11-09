using AutoMapper;
using PeruCompras.IntegracionMef.DataBinding.Models;
using PeruCompras.IntegracionMef.Domain.Models;
using PeruCompras.IntegracionMef.Domain.Services.Contracts;

namespace PeruCompras.IntegracionMef.Api.Events.Handlers
{
    public class ReservaEventHandler : IHandler<ProformaBindingModel>
    {
        private readonly IProformaService _proformaService;
        private readonly IMapper _mapper;

        public ReservaEventHandler(IProformaService proformaService, IMapper mapper)
        {
            _mapper = mapper;
            _proformaService = proformaService;
        }

        public async Task Execute(ProformaBindingModel proformaBM)
        {
            var proforma = _mapper.Map<Proforma>(proformaBM);
            await _proformaService.GenerarProformas(proforma);
        }
    }
}
