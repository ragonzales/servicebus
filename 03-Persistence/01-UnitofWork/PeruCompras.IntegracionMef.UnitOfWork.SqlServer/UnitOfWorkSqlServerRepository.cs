using PeruCompras.IntegracionMef.Repository.Contracts;
using PeruCompras.IntegracionMef.Repository.SqlServer;
using PeruCompras.IntegracionMef.UnitOfWork.Contracts;
using System.Data.SqlClient;

namespace PeruCompras.IntegracionMef.UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerRepository : IUnitOfWorkRepository
    {
        public IProformaRepository ProformaRepository { get; }

        public UnitOfWorkSqlServerRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction, int timeOut)
        {
            ProformaRepository = new ProformaRepository(sqlConnection, sqlTransaction, timeOut);
        }
    }
}