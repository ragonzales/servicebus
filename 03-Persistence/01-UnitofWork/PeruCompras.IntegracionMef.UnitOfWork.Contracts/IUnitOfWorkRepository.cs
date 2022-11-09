using PeruCompras.IntegracionMef.Repository.Contracts;

namespace PeruCompras.IntegracionMef.UnitOfWork.Contracts
{
    public interface IUnitOfWorkRepository
    {
        IProformaRepository ProformaRepository { get; }
    }
}