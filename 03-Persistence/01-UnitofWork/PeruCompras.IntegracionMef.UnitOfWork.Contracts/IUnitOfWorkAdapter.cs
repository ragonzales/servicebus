namespace PeruCompras.IntegracionMef.UnitOfWork.Contracts
{
    public interface IUnitOfWorkAdapter : IDisposable
    {
        IUnitOfWorkRepository UnitOfWorkRepository { get; }
        void SaveChanges();
        void Rollback();
    }
}
