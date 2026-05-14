using Microsoft.Extensions.Caching.Hybrid;

namespace IaipDataServiceTests;

[SetUpFixture]
public class Setup
{
    internal static HybridCache? FakeCache;

    [OneTimeSetUp]
    public void OneTimeSetUp() => FakeCache = new FakeHybridCache();
}
