using System.Data;

namespace IaipDataService.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
