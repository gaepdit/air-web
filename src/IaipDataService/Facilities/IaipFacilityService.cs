using System.Net.Http.Json;

namespace IaipDataService.Facilities;

public sealed class IaipFacilityService(IHttpClientFactory httpClientFactory) : IFacilityService
{
    public async Task<Facility> GetAsync(FacilityId id, CancellationToken token = default) => 
        await FindAsync(id,token) ?? throw new InvalidOperationException("Facility not found.");

    public async Task<Facility?> FindAsync(FacilityId? id, CancellationToken token = default)
    {
        if (id is null) return null;
        
        using var client = httpClientFactory.CreateClient(IaipDataConstants.IaipApiClient);
        var requestUri = $"{IaipDataConstants.GetFacilityPath}{id.Id}";

        var result = await client.GetFromJsonAsync<Facility?>(requestUri, token);
        return result;
    }

    public async Task<bool> ExistsAsync(FacilityId id, CancellationToken token = default)
    {
        using var client = httpClientFactory.CreateClient(IaipDataConstants.IaipApiClient);
        var requestUri = $"{IaipDataConstants.FacilityExistsPath}{id.Id}";
        return bool.TryParse(await client.GetStringAsync(requestUri, token), out var exists) && exists;
    }

    public Task<IReadOnlyCollection<Facility>> GetListAsync(CancellationToken token = default)
    {
        // This method is only available when using in-memory data, and won't be implemented
        // for accessing from the airbranch DB. A feature enhancement has been suggested for
        // replacing this with a search tool.
        throw new NotImplementedException();
    }
}
