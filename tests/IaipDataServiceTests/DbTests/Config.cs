using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using Microsoft.Extensions.Configuration;

namespace IaipDataServiceTests.DbTests;

[SetUpFixture]
public class Config
{
    internal static readonly FacilityId NonexistentFacilityId = new("777-99999");
    internal static readonly FacilityId TestFacilityId = new("247-00007");
    internal const string TestFacilityName = "C&D Technologies";


    internal static IDbConnectionFactory? DbConnectionFactory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var config = new ConfigurationBuilder().AddJsonFile("DbTests/testsettings.json").Build();
        DbConnectionFactory = new DbConnectionFactory(config.GetConnectionString("DefaultConnection")!);
    }
}
