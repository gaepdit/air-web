using IaipDataService.Facilities;
using IaipDataService.Permits;
using IaipDataService.Utilities;

namespace IaipDataService.TestData;

public class TestPermitService : IPermitService
{
    public Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(FacilityId facilityId, int skip, int take) =>
        Task.FromResult<IReadOnlyCollection<PermitSummary>>(PermitData.GetData
            .Where(ps => ps.FacilityId.Equals(FacilityId.TryFormat(facilityId)))
            .Skip(skip).Take(take)
            .OrderBy(ps => ps.FacilityName).ThenBy(ps => ps.FacilityId).ThenBy(ps => ps.IssuanceDate).ToList());

    public Task<IReadOnlyCollection<PermitSummary>>
        SearchPermitsAsync(string? name, string? permit, int skip, int take) =>
        Task.FromResult<IReadOnlyCollection<PermitSummary>>(FilterPermits(name, permit)
            .Skip(skip).Take(take)
            .OrderBy(ps => ps.FacilityName).ThenBy(ps => ps.FacilityId).ThenBy(ps => ps.IssuanceDate).ToList());

    public Task<int> CountPermitsAsync(FacilityId facilityId) =>
        Task.FromResult(PermitData.GetData.Count(ps => ps.FacilityId.Equals(FacilityId.TryFormat(facilityId))));

    public Task<int> CountPermitsAsync(string? name, string? permit) =>
        Task.FromResult(FilterPermits(name, permit).Count());

    private static IEnumerable<PermitSummary> FilterPermits(string? name, string? permit)
    {
        return PermitData.GetData
            .Where(ps => string.IsNullOrWhiteSpace(name) ||
                         ps.FacilityName.Contains(name, StringComparison.InvariantCultureIgnoreCase))
            .Where(ps => string.IsNullOrWhiteSpace(permit) ||
                         ps.PermitNumber.Contains(permit, StringComparison.InvariantCultureIgnoreCase));
    }

    public Task<byte[]?> GetPermitFileAsync(string fileName) =>
        Task.FromResult(PermitData.GetData.Any(ps => AllFiles(ps).Contains(fileName))
            ? Convert.FromBase64String(EncodedPdfFile)
            : null);

    private static string AllFiles(PermitSummary ps) => new[]
        {
            ps.VNarrative, ps.VFinal, ps.PsdAppSum, ps.PsdPrelim, ps.PsdNarrative, ps.PsdFinalDet, ps.PsdFinal,
            ps.OtherNarrative, ps.OtherPermit,
        }
        .ConcatWithSeparator("|");

    #region Encoded binary data

    private const string EncodedPdfFile =
        "JVBERi0xLjIKMSAwIG9iago8PD4+CnN0cmVhbQpCVC9GMSAyNCBUZiAxMCA4IFREIChIZWxsbyB3b3JsZCEpJyBFVAplbmRzdHJlYW0KZW5kb2JqCjQgMCBvYmoKPDwvVHlwZSAvUGFnZS9QYXJlbnQgMiAwIFIvQ29udGVudHMgMSAwIFI+PgplbmRvYmoKMiAwIG9iago8PC9LaWRzIFs0IDAgUl0vQ291bnQgMS9UeXBlIC9QYWdlcy9NZWRpYUJveCBbMCAwIDI1MCA1MF0+PgplbmRvYmoKMyAwIG9iago8PC9QYWdlcyAyIDAgUi9UeXBlIC9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAzIDAgUj4+CiUlRU9G";

    #endregion
}
