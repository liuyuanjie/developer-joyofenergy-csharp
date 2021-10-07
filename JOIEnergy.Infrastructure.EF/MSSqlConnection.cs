using System.Data;
using JOIEnergy.Application;
using Microsoft.Data.SqlClient;

namespace JOIEnergy.Infrastructure.EF
{
    public class MSSqlConnection : IConnection
    {
        private readonly ConnectionSettings _connectionSettings;

        public MSSqlConnection(ConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
        }
        public IDbConnection OpenConnection()
        {
            var sqlConnection = new SqlConnection(_connectionSettings.ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}