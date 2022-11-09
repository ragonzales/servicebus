using PeruCompras.IntegracionMef.Domain.Models;

namespace PeruCompras.IntegracionMef.Domain.Services.Contracts
{
    public interface IProformaService
    {
        Task GenerarProformas(Proforma proforma);
    }
}