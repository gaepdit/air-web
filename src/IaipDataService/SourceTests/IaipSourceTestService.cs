using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;
using IaipDataService.SourceTests.Models.TestRun;
using IaipDataService.Structs;
using IaipDataService.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;

namespace IaipDataService.SourceTests;

public class IaipSourceTestService(
    IDbConnectionFactory dbf,
    IMemoryCache cache,
    ILogger<IaipSourceTestService> logger) : ISourceTestService
{
    public async Task<BaseSourceTestReport?> FindAsync(int referenceNumber)
    {
        var getDocumentTypeTask = GetDocumentTypeAsync(referenceNumber);

        if (!await SourceTestExistsAsync(referenceNumber)) return null;

        return await getDocumentTypeTask switch
        {
            DocumentType.Unassigned => null,
            DocumentType.OneStackTwoRuns or DocumentType.OneStackThreeRuns or DocumentType.OneStackFourRuns =>
                await GetOneStackAsync(referenceNumber),
            DocumentType.TwoStackStandard or DocumentType.TwoStackDre => await GetTwoStackAsync(referenceNumber),
            DocumentType.LoadingRack => await GetLoadingRackAsync(referenceNumber),
            DocumentType.PondTreatment => await GetPondTreatmentAsync(referenceNumber),
            DocumentType.GasConcentration => await GetGasConcentrationAsync(referenceNumber),
            DocumentType.Flare => await GetFlareAsync(referenceNumber),
            DocumentType.Rata => await GetRataAsync(referenceNumber),
            DocumentType.MemorandumStandard or DocumentType.MemorandumToFile or DocumentType.PTE =>
                await GetMemorandumAsync(referenceNumber),
            DocumentType.Method9Multi or DocumentType.Method22 or DocumentType.Method9Single => await GetOpacityAsync(
                referenceNumber),
            _ => null,
        };
    }

    public async Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber, bool forceRefresh = false)
    {
        if (!await SourceTestExistsAsync(referenceNumber)) return null;

        var cacheKey = $"IaipSourceTestService.FindSummaryAsync.{referenceNumber}";
        if (!forceRefresh && cache.TryGetValue(cacheKey, logger, out SourceTestSummary? cachedValue))
            return cachedValue;

        using var db = dbf.Create();

        await using var multi = await db.QueryMultipleAsync("air.GetSourceTestSummary",
            param: new { ReferenceNumber = referenceNumber });

        var testSummary = multi
            .Read<SourceTestSummary, FacilitySummary, DateRange, PersonName, SourceTestSummary>((summary, facility,
                testDates, reviewedByStaff) =>
            {
                summary.Facility = facility;
                summary.TestDates = testDates;
                summary.ReviewedByStaff = reviewedByStaff;
                return summary;
            }).SingleOrDefault();

        return cache.Set(cacheKey, testSummary, CacheConstants.SourceTestExpiration, logger, forceRefresh);
    }

    public async Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        bool forceRefresh = false)
    {
        var cacheKey = $"IaipSourceTestService.GetSourceTestsForFacilityAsync.{facilityId}";
        if (!forceRefresh &&
            cache.TryGetValue(cacheKey, logger, out IReadOnlyCollection<SourceTestSummary>? cachedValue))
            return cachedValue;

        using var db = dbf.Create();

        await using var multi = await db.QueryMultipleAsync("air.GetFacilitySourceTests",
            param: new { FacilityId = facilityId.Id }, commandType: CommandType.StoredProcedure);

        var sourceTests = multi
            .Read<SourceTestSummary, DateRange, PersonName, SourceTestSummary>((summary, testDates, reviewedByStaff) =>
            {
                summary.TestDates = testDates;
                summary.ReviewedByStaff = reviewedByStaff;
                return summary;
            }).ToList();

        return cache.Set(cacheKey, sourceTests, CacheConstants.SourceTestListExpiration, logger, forceRefresh);
    }

    public async Task<IReadOnlyCollection<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(
        bool forceRefresh = false)
    {
        const string cacheKey = "IaipSourceTestService.GetOpenSourceTestsForComplianceAsync";
        if (!forceRefresh &&
            cache.TryGetValue(cacheKey, logger, out IReadOnlyCollection<SourceTestSummary>? cachedValue))
            return cachedValue;

        using var db = dbf.Create();

        await using var multi = await db.QueryMultipleAsync("air.GetOpenSourceTestsForCompliance",
            commandType: CommandType.StoredProcedure);

        var sourceTests = multi
            .Read<SourceTestSummary, DateRange, PersonName, SourceTestSummary>((summary, testDates, reviewedByStaff) =>
            {
                summary.TestDates = testDates;
                summary.ReviewedByStaff = reviewedByStaff;
                return summary;
            }).ToList();

        return cache.Set(cacheKey, sourceTests, CacheConstants.SourceTestListExpiration, logger, forceRefresh);
    }

    public Task UpdateSourceTest(int referenceNumber, string complianceAssignment, bool complianceComplete)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> SourceTestExistsAsync(int referenceNumber)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.SourceTestExists",
            param: new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);
    }

    private async Task<DocumentType> GetDocumentTypeAsync(int referenceNumber)
    {
        using var db = dbf.Create();
        return await db.QuerySingleAsync<DocumentType>("air.GetSourceTestDocumentType",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);
    }

    private async Task<T> GetBaseSourceTestReportAsync<T>(int referenceNumber) where T : BaseSourceTestReport
    {
        using var db = dbf.Create();

        await using var multi = await db.QueryMultipleAsync("air.GetBaseSourceTestReport",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = multi.Read<T, FacilitySummary, PersonName, PersonName, PersonName, DateRange, T>((report, facility,
            reviewedByStaff, complianceManager, testingUnitManager, testDates) =>
        {
            report.Facility = facility;
            report.ReviewedByStaff = reviewedByStaff;
            report.ComplianceManager = complianceManager;
            report.TestingUnitManager = testingUnitManager;
            report.TestDates = testDates;
            return report;
        }).Single();

        report.WitnessedByStaff.AddRange(await multi.ReadAsync<PersonName>());

        return report;
    }

    private async Task<SourceTestReportOneStack> GetOneStackAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportOneStack",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportOneStack>(referenceNumber);

        await using var multi = await getMultiTask;
        _ = multi
            .Read<SourceTestReportOneStack, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                SourceTestReportOneStack>((r, maxOperatingCapacity, operatingCapacity, avgPollutantConcentration,
                avgEmissionRate) =>
            {
                report.MaxOperatingCapacity = maxOperatingCapacity;
                report.OperatingCapacity = operatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgPollutantConcentration = avgPollutantConcentration;
                report.AvgEmissionRate = avgEmissionRate;
                report.PercentAllowable = r.PercentAllowable;
                return r;
            });

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<StackTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportTwoStack> GetTwoStackAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportTwoStack",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportTwoStack>(referenceNumber);
        await using var multi = await getMultiTask;

        _ = multi.Read(
            types:
            [
                typeof(SourceTestReportTwoStack),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
            ],
            map: results =>
            {
                var r = (SourceTestReportTwoStack)results[0];
                report.MaxOperatingCapacity = (ValueWithUnits)results[1];
                report.OperatingCapacity = (ValueWithUnits)results[2];
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.StackOneName = r.StackOneName;
                report.StackTwoName = r.StackTwoName;
                report.StackOneAvgPollutantConcentration = (ValueWithUnits)results[3];
                report.StackTwoAvgPollutantConcentration = (ValueWithUnits)results[4];
                report.StackOneAvgEmissionRate = (ValueWithUnits)results[5];
                report.StackTwoAvgEmissionRate = (ValueWithUnits)results[6];
                report.SumAvgEmissionRate = (ValueWithUnits)results[7];
                report.PercentAllowable = r.PercentAllowable;
                report.DestructionEfficiency = r.DestructionEfficiency;
                return r;
            });

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<TwoStackTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportLoadingRack> GetLoadingRackAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportLoadingRack",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportLoadingRack>(referenceNumber);

        await using var multi = await getMultiTask;
        _ = multi.Read(
            types:
            [
                typeof(SourceTestReportLoadingRack),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
            ],
            map: results =>
            {
                var r = (SourceTestReportLoadingRack)results[0];
                report.MaxOperatingCapacity = (ValueWithUnits)results[1];
                report.OperatingCapacity = (ValueWithUnits)results[2];
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.TestDuration = (ValueWithUnits)results[3];
                report.PollutantConcentrationIn = (ValueWithUnits)results[4];
                report.PollutantConcentrationOut = (ValueWithUnits)results[5];
                report.EmissionRate = (ValueWithUnits)results[6];
                report.DestructionReduction = (ValueWithUnits)results[7];
                return r;
            });

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportPondTreatment> GetPondTreatmentAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportPondTreatment",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportPondTreatment>(referenceNumber);

        await using var multi = await getMultiTask;
        _ = multi
            .Read<SourceTestReportPondTreatment, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                SourceTestReportPondTreatment>((r, maxOperatingCapacity, operatingCapacity, avgPollutantCollectionRate,
                avgTreatmentRate) =>
            {
                report.MaxOperatingCapacity = maxOperatingCapacity;
                report.OperatingCapacity = operatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgPollutantCollectionRate = avgPollutantCollectionRate;
                report.AvgTreatmentRate = avgTreatmentRate;
                report.DestructionEfficiency = r.DestructionEfficiency;
                return r;
            });

        report.TestRuns.AddRange(await multi.ReadAsync<PondTreatmentTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportGasConcentration> GetGasConcentrationAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportGasConcentration",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportGasConcentration>(referenceNumber);

        await using var multi = await getMultiTask;
        _ = multi
            .Read<SourceTestReportGasConcentration, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                SourceTestReportGasConcentration>((r, maxOperatingCapacity, operatingCapacity,
                avgPollutantConcentration, avgEmissionRate) =>
            {
                report.MaxOperatingCapacity = maxOperatingCapacity;
                report.OperatingCapacity = operatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgPollutantConcentration = avgPollutantConcentration;
                report.AvgEmissionRate = avgEmissionRate;
                report.PercentAllowable = r.PercentAllowable;
                return r;
            });

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<GasConcentrationTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportFlare> GetFlareAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportFlare",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportFlare>(referenceNumber);

        await using var multi = await getMultiTask;
        _ = multi
            .Read<SourceTestReportFlare, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                SourceTestReportFlare>((r, maxOperatingCapacity, operatingCapacity, avgHeatingValue,
                avgEmissionRateVelocity) =>
            {
                report.MaxOperatingCapacity = maxOperatingCapacity;
                report.OperatingCapacity = operatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgHeatingValue = avgHeatingValue;
                report.AvgEmissionRateVelocity = avgEmissionRateVelocity;
                report.PercentAllowable = r.PercentAllowable;
                return r;
            });

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<FlareTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportRata> GetRataAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportRata",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportRata>(referenceNumber);

        await using var multi = await getMultiTask;
        var r = await multi.ReadSingleAsync<SourceTestReportRata>();

        report.ApplicableStandard = r.ApplicableStandard;
        report.Diluent = r.Diluent;
        report.Units = r.Units;
        report.RelativeAccuracyCode = r.RelativeAccuracyCode;
        report.RelativeAccuracyPercent = r.RelativeAccuracyPercent;
        report.RelativeAccuracyRequiredPercent = r.RelativeAccuracyRequiredPercent;
        report.RelativeAccuracyRequiredLabel = r.RelativeAccuracyRequiredLabel;

        report.TestRuns.AddRange(await multi.ReadAsync<RataTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestMemorandum> GetMemorandumAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestMemorandum",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestMemorandum>(referenceNumber);

        await using var multi = await getMultiTask;
        _ = multi.Read<SourceTestMemorandum, ValueWithUnits, ValueWithUnits, SourceTestMemorandum>((r,
            maxOperatingCapacity, operatingCapacity) =>
        {
            report.MonitorManufacturer = r.MonitorManufacturer;
            report.MonitorSerialNumber = r.MonitorSerialNumber;
            report.MaxOperatingCapacity = maxOperatingCapacity;
            report.OperatingCapacity = operatingCapacity;
            report.ControlEquipmentInfo = r.ControlEquipmentInfo;
            report.Comments = r.Comments;
            return r;
        });

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<SourceTestReportOpacity> GetOpacityAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetSourceTestReportOpacity",
            param: new { ReferenceNumber = referenceNumber }, commandType: CommandType.StoredProcedure);

        var report = await GetBaseSourceTestReportAsync<SourceTestReportOpacity>(referenceNumber);

        await using var multi = await getMultiTask;
        var r = await multi.ReadSingleAsync<SourceTestReportOpacity>();

        report.ControlEquipmentInfo = r.ControlEquipmentInfo;
        report.OpacityStandard = r.OpacityStandard;
        report.TestDuration = r.TestDuration;
        report.MaxOperatingCapacityUnits = r.MaxOperatingCapacityUnits;
        report.OperatingCapacityUnits = r.OperatingCapacityUnits;
        report.AllowableEmissionRateUnits = r.AllowableEmissionRateUnits;

        report.TestRuns.AddRange(await multi.ReadAsync<OpacityTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }
}
