using System.Data;

namespace AirWeb.EfRepository.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
