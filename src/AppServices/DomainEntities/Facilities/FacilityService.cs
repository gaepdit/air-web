using AirWeb.Domain.ExternalEntities.Facilities;
using AutoMapper;

namespace AirWeb.AppServices.DomainEntities.Facilities;

public sealed class FacilityService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IFacilityRepository repository
) : IFacilityService
{
    public async Task<FacilityViewDto?> FindAsync(FacilityId id, CancellationToken token = default) =>
        mapper.Map<FacilityViewDto>(await repository.FindFacilityAsync(id, token).ConfigureAwait(false));

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
