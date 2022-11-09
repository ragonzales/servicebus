using System.Data.SqlClient;

namespace PeruCompras.IntegracionMef.Repository.SqlServer
{
    public class Repository
    {
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected int _timeOut;
        
        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _connection, _transaction);
        }
    }
}