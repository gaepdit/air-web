using IaipDataService.Facilities;
using IaipDataService.Permits;

namespace IaipDataService.TestData;

public class TestPermitService : IPermitService
{
    public Task<IReadOnlyCollection<PermitSummary>> GetPermitListAsync(string? facilityId, string? name, int skip,
        int take) =>
        Task.FromResult<IReadOnlyCollection<PermitSummary>>(FilteredPermitSummaries(facilityId, name)
            .Skip(skip).Take(take)
            .OrderBy(ps => ps.FacilityName).ThenBy(ps => ps.FacilityId).ThenBy(ps => ps.IssuanceDate).ToList());

    public Task<int> CountPermitsAsync(string? facilityId, string? name) =>
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
