using PeruCompras.IntegracionMef.UnitOfWork.Contracts;

namespace PeruCompras.IntegracionMef.UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServer : IUnitOfWork
    {
        private readonly string _connectionString;
        private readonly int _timeOut;

        public UnitOfWorkSqlServer(string connectionString, int timeOut)
        {
            _connectionString = connectionString;
            _timeOut = timeOut;
        }

        public IUnitOfWorkAdapter Create()
        {
            return new UnitOfWorkSqlServerAdapter(_connectionString, _timeOut);
        }

        public IUnitOfWorkAdapter Update()
        {
            return new UnitOfWorkSqlServerAdapter(_connectionString, _timeOut);
        }
    }
}