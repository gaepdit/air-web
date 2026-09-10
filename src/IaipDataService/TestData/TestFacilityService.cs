using IaipDataService.Facilities;
using IaipDataService.Permits;

namespace IaipDataService.TestData;

public sealed class TestFacilityService : IFacilityService
{
    internal IReadOnlyCollection<Facility> Items { get; } = [.. FacilityData.GetData];

    public Task<Facility?> FindFacilityAsync(FacilityId id, bool forceRefresh = false,
        CancellationToken token = default) =>
        FindFacility(id);

    public Task<Facility?> FindFacilityDetailsAsync(FacilityId id, bool forceRefresh = false) =>
        FindFacility(id);

    private Task<Facility?> FindFacility(FacilityId id) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public async Task<string> GetNameAsync(string id) =>
        (await GetAllAsync().ConfigureAwait(false)).SingleOrDefault(f => f.FacilityId == id)?.Name ??
        throw new InvalidOperationException("Facility not found.");

    public Task<bool> ExistsAsync(FacilityId id) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public async Task<ushort> GetNextActionNumberAsync(FacilityId id)
    {
        var facility = await FindFacility(id).ConfigureAwait(false);
        return facility is null
            ? throw new ArgumentException($"Facility not found: {id}")
            : facility.NextActionNumber++;
    }

    public Task<DateTime?> GetFacilityEpaDxDateAsync(FacilityId id,
        CancellationToken token = default)
    {
        var start = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Unspecified);
        var totalDays = Convert.ToInt32((DateTime.Today - start).TotalDays);
        var dxDate = start.AddDays(Random.Shared.Next(totalDays)).AddMinutes(Random.Shared.Next(1440));
        return Task.FromResult<DateTime?>(dxDate);
    }

    public Task<bool> RefreshEpaDataExchange(FacilityId id) => Task.FromResult(true);

    public Task<IReadOnlyCollection<FacilitySummary>> GetAllAsync(bool forceRefresh = false,
        bool includePortableSources = true, CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<FacilitySummary>>([
            .. Items
                .Where(f => includePortableSources || f.Id.CountyCode != "777")
                .Select(f => new FacilitySummary(f))
                .OrderBy(f => f.Id),
        ]);

    public Task<IReadOnlyCollection<FacilityList>> GetListAsync(CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<FacilityList>>(Items
            .Select(f => new FacilityList(f.FacilityId, f.Name, f.Id.Id)).OrderBy(f => f.Id).ToList());

    public Task<IReadOnlyCollection<PermitSummary>> GetPermitListAsync(string? facilityId, string? name, int skip,
        int take, CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<PermitSummary>>(FilteredPermitSummaries(facilityId, name)
            .Skip(skip).Take(take)
            .OrderBy(ps => ps.FacilityName).ThenBy(ps => ps.FacilityId).ThenBy(ps => ps.IssuanceDate).ToList());

    public Task<int> CountPermitsAsync(string? facilityId, string? name, CancellationToken token = default) =>
        Task.FromResult(FilteredPermitSummaries(facilityId, name).Count());

    private static IEnumerable<PermitSummary> FilteredPermitSummaries(string? facilityId, string? name) =>
        PermitData.GetData
            .Where(ps =>
                string.IsNullOrWhiteSpace(facilityId) || ps.FacilityId.Equals(FacilityId.TryFormat(facilityId)))
            .Where(ps => string.IsNullOrWhiteSpace(name) ||
                         ps.FacilityName.Contains(name, StringComparison.InvariantCultureIgnoreCase));

    public Task<byte[]?> GetPermitFileAsync(string fileName) =>
        Task.FromResult<byte[]?>(Convert.FromBase64String(EncodedPdfFile));

    #region Encoded binary data

    private const string EncodedPdfFile =
        "JVBERi0xLjIKMSAwIG9iago8PD4+CnN0cmVhbQpCVC9GMSAyNCBUZiAxMCA4IFREIChIZWxsbyB3b3JsZCEpJyBFVAplbmRzdHJlYW0KZW5kb2JqCjQgMCBvYmoKPDwvVHlwZSAvUGFnZS9QYXJlbnQgMiAwIFIvQ29udGVudHMgMSAwIFI+PgplbmRvYmoKMiAwIG9iago8PC9LaWRzIFs0IDAgUl0vQ291bnQgMS9UeXBlIC9QYWdlcy9NZWRpYUJveCBbMCAwIDI1MCA1MF0+PgplbmRvYmoKMyAwIG9iago8PC9QYWdlcyAyIDAgUi9UeXBlIC9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAzIDAgUj4+CiUlRU9G";

    #endregion
}
