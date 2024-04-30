using System.Data;

namespace MyApp.EfRepository.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
