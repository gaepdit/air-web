using Dapper;

namespace IaipDataServiceTests.DbTests.DbObjects;

[TestFixture]
public class DatabaseObjectTests
{
    private static async Task<bool> DbObjectCheck(string spName)
    {
        const string sql = "select convert(bit, isnull(object_id(@SpName), 0))";
        using var db = Config.DbConnectionFactory!.Create();
        return await db.ExecuteScalarAsync<bool>(sql, new { SpName = spName });
    }

    [Test]
    [TestCase("air.GetBaseSourceTestReport")]
    [TestCase("air.GetFacilitySourceTests")]
    [TestCase("air.GetIaipAnnualFeesSummary")]
    [TestCase("air.GetIaipFacility")]
    [TestCase("air.GetIaipFacilityDetails")]
    [TestCase("air.GetIaipFacilityList")]
    [TestCase("air.GetIaipFacilityName")]
    [TestCase("air.GetOpenSourceTestsForCompliance")]
    [TestCase("air.GetSourceTestDocumentType")]
    [TestCase("air.GetSourceTestMemorandum")]
    [TestCase("air.GetSourceTestReportFlare")]
    [TestCase("air.GetSourceTestReportGasConcentration")]
    [TestCase("air.GetSourceTestReportLoadingRack")]
    [TestCase("air.GetSourceTestReportOneStack")]
    [TestCase("air.GetSourceTestReportOpacity")]
    [TestCase("air.GetSourceTestReportPondTreatment")]
    [TestCase("air.GetSourceTestReportRata")]
    [TestCase("air.GetSourceTestReportTwoStack")]
    [TestCase("air.GetSourceTestSummary")]
    [TestCase("air.IaipAnnualFeesSummary")]
    [TestCase("air.IaipFacilityAirProgramData")]
    [TestCase("air.IaipFacilityData")]
    [TestCase("air.IaipFacilityExists")]
    [TestCase("air.IaipFacilityPollutantData")]
    [TestCase("air.IaipFacilityProgramClassificationData")]
    [TestCase("air.IaipSourceTestSummary")]
    [TestCase("air.SourceTestExists")]
    [TestCase("air.UpdateSourceTest")]
    public async Task DatabaseObjectShouldExist(string spName) => (await DbObjectCheck(spName)).Should().BeTrue();
}
