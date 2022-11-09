using PeruCompras.IntegracionMef.UnitOfWork.Contracts;
using System.Data.SqlClient;

namespace PeruCompras.IntegracionMef.UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private SqlConnection Context { get; set; }
        private SqlTransaction Transaction { get; set; }
        public IUnitOfWorkRepository UnitOfWorkRepository { get; set; }

        public UnitOfWorkSqlServerAdapter(string connectionString, int timeOut)
        {
            Context = new SqlConnection(connectionString);
            Context.Open();
            Transaction = Context.BeginTransaction();
            UnitOfWorkRepository = new UnitOfWorkSqlServerRepository(Context, Transaction, timeOut);
        }

        public void Dispose()
        {
            if (Transaction != null) Transaction.Dispose();

            if (Context != null)
            {
                Context.Close();
                Context.Dispose();
            }
            System.GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }
    }
}
