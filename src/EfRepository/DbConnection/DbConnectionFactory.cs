using Microsoft.Data.SqlClient;
using System.Data;

namespace AirWeb.EfRepository.DbConnection;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection Create() => new SqlConnection(connectionString);
}
