using Microsoft.Data.SqlClient;
using System.Data;

namespace IaipDataService.DbConnection;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection Create() => new SqlConnection(connectionString);
}
