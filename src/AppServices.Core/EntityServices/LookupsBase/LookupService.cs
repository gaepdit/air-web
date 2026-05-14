using AirWeb.AppServices.Core.Caching;
using AirWeb.AppServices.Core.EntityServices.Users;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace AirWeb.AppServices.Core.EntityServices.LookupsBase;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters

public abstract class LookupService<TEntity, TViewDto, TUpdateDto>(
    IMapper mapper,
    INamedEntityRepository<TEntity> repository,
    INamedEntityManager<TEntity> manager,
    IUserService userService,
    HybridCache cache,
    ILogger logger)
    : ILookupService<TViewDto, TUpdateDto>
    where TEntity : StandardNamedEntity
    where TUpdateDto : LookupUpdateDto
#pragma warning restore S2436
{
    private static string LookupName => typeof(TEntity).Name;

    public async Task<TViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        await cache.GetOrCreateAsync($"{LookupName}.{id}",
            factory: async ct => mapper.Map<TViewDto>(await repository.FindAsync(id, ct).ConfigureAwait(false)),
            expiration: CacheConstants.LookupsCacheTime, logger, tag: LookupName, token).ConfigureAwait(false);

    public async Task<TUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        await cache.GetOrCreateAsync($"{LookupName}.Update.{id}",
            factory: async ct => mapper.Map<TUpdateDto>(await repository.FindAsync(id, ct).ConfigureAwait(false)),
            expiration: CacheConstants.LookupsCacheTime, logger, tag: LookupName, token).ConfigureAwait(false);

    public async Task<IReadOnlyList<TViewDto>> GetListAsync(CancellationToken token = default) =>
        await cache.GetOrCreateAsync($"{LookupName}.List",
            factory: async ct =>
                mapper.Map<IReadOnlyList<TViewDto>>(await repository.GetOrderedListAsync(ct).ConfigureAwait(false)),
            expiration: CacheConstants.LookupsCacheTime, logger, tag: LookupName, token).ConfigureAwait(false);

    public async Task<IReadOnlyList<ListItem>> GetAsListItemsAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        await cache.GetOrCreateAsync($"{LookupName}.ListItems{(includeInactive ? ".includeInactive" : "")}",
            factory: async ct => (await repository.GetOrderedListAsync(entity => includeInactive || entity.Active, ct)
                .ConfigureAwait(false)).Select(entity => new ListItem(entity.Id, entity.NameWithActivity)).ToList(),
            expiration: CacheConstants.LookupsCacheTime, logger, tag: LookupName, token).ConfigureAwait(false);

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var entity = await manager
            .CreateAsync(name, (await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id, token)
            .ConfigureAwait(false);
        await repository.InsertAsync(entity, token: token).ConfigureAwait(false);
        await cache.RemoveByTagAsync([LookupName], token).ConfigureAwait(false);
        return entity.Id;
    }

    public async Task UpdateAsync(Guid id, TUpdateDto resource, CancellationToken token = default)
    {
        var entity = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        if (entity.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(entity, resource.Name, token).ConfigureAwait(false);
        entity.Active = resource.Active;
        entity.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await repository.UpdateAsync(entity, token: token).ConfigureAwait(false);
        await cache.RemoveByTagAsync([LookupName], token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        repository.Dispose();
        userService.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual async ValueTask DisposeAsyncCore()
    {
        await repository.DisposeAsync().ConfigureAwait(false);
        userService.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    #endregion
}
