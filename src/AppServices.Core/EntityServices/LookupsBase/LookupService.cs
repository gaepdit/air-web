using AirWeb.AppServices.Core.Caching;
using AirWeb.AppServices.Core.EntityServices.Users;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AirWeb.AppServices.Core.EntityServices.LookupsBase;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters

public abstract class LookupService<TEntity, TViewDto, TUpdateDto>(
    IMapper mapper,
    INamedEntityRepository<TEntity> repository,
    INamedEntityManager<TEntity> manager,
    IUserService userService,
    IMemoryCache cache,
    ILogger logger)
    : ILookupService<TViewDto, TUpdateDto>
    where TEntity : StandardNamedEntity
    where TUpdateDto : LookupUpdateDto
#pragma warning restore S2436
{
    private static string LookupName => typeof(TEntity).Name;

    public async Task<TViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var cacheKey = $"{LookupName}.{id}";
        if (cache.TryGetValue(cacheKey, logger, out TViewDto? cachedValue)) return cachedValue;

        return cache.Set(
            mapper.Map<TViewDto>(await repository.FindAsync(id, token: token).ConfigureAwait(false)),
            cacheKey, CacheConstants.LookupsCacheTime, logger);
    }

    public async Task<TUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var cacheKey = $"{LookupName}.Update.{id}";
        if (cache.TryGetValue(cacheKey, logger, out TUpdateDto? cachedValue)) return cachedValue;

        return cache.Set(
            mapper.Map<TUpdateDto>(await repository.FindAsync(id, token: token).ConfigureAwait(false)),
            cacheKey, CacheConstants.LookupsCacheTime, logger);
    }

    public async Task<IReadOnlyList<TViewDto>> GetListAsync(CancellationToken token = default)
    {
        var cacheKey = $"{LookupName}.List";
        if (cache.TryGetValue(cacheKey, logger, out IReadOnlyList<TViewDto>? cachedValue)) return cachedValue;

        return cache.Set(
            mapper.Map<IReadOnlyList<TViewDto>>(await repository.GetOrderedListAsync(token).ConfigureAwait(false)),
            cacheKey, CacheConstants.LookupsCacheTime, logger);
    }

    public async Task<IReadOnlyList<ListItem>> GetAsListItemsAsync(bool includeInactive = false,
        CancellationToken token = default)
    {
        var cacheKey = $"{LookupName}.ListItems{(includeInactive ? ".includeInactive" : "")}";
        if (cache.TryGetValue(cacheKey, logger, out IReadOnlyList<ListItem>? cachedValue)) return cachedValue;

        return cache.Set(
            (await repository.GetOrderedListAsync(entity => includeInactive || entity.Active, token)
                .ConfigureAwait(false)).Select(entity => new ListItem(entity.Id, entity.NameWithActivity)).ToList(),
            cacheKey, CacheConstants.LookupsCacheTime, logger);
    }

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var entity = await manager
            .CreateAsync(name, (await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id, token)
            .ConfigureAwait(false);
        await repository.InsertAsync(entity, token: token).ConfigureAwait(false);

        RemoveCaches();
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

        RemoveCaches(id);
    }

    private void RemoveCaches(Guid? id = null)
    {
        cache.Remove($"{LookupName}.List",
            $"{LookupName}.ListItems.includeInactive",
            $"{LookupName}.ListItems");
        if (id != null) cache.Remove($"{LookupName}.{id}", $"{LookupName}.Update.{id}");
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
