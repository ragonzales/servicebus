namespace PeruCompras.IntegracionMef.UnitOfWork.Contracts
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create();
        IUnitOfWorkAdapter Update();
    }
}