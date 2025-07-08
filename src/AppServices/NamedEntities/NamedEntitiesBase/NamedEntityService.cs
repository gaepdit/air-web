using AirWeb.AppServices.IdentityServices;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.NamedEntities.NamedEntitiesBase;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters

public class NamedEntityService<TEntity, TViewDto, TUpdateDto> : INamedEntityService<TViewDto, TUpdateDto>
    where TEntity : StandardNamedEntity
    where TUpdateDto : NamedEntityUpdateDto
#pragma warning restore S2436
{
    private readonly IMapper _mapper;
    private readonly INamedEntityRepository<TEntity> _repository;
    private readonly INamedEntityManager<TEntity> _manager;
    private readonly IUserService _userService;

    protected NamedEntityService(IMapper mapper,
        INamedEntityRepository<TEntity> repository,
        INamedEntityManager<TEntity> manager,
        IUserService userService)
    {
        _mapper = mapper;
        _repository = repository;
        _manager = manager;
        _userService = userService;
    }

    public async Task<TViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<TViewDto>(await _repository.FindAsync(id, token: token).ConfigureAwait(false));

    public async Task<TUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<TUpdateDto>(await _repository.FindAsync(id, token: token).ConfigureAwait(false));

    public async Task<IReadOnlyList<TViewDto>> GetListAsync(CancellationToken token = default) =>
        _mapper.Map<IReadOnlyList<TViewDto>>(await _repository.GetOrderedListAsync(token).ConfigureAwait(false));

    public async Task<IReadOnlyList<ListItem>> GetAsListItemsAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        (await _repository.GetOrderedListAsync(entity => includeInactive || entity.Active, token).ConfigureAwait(false))
        .Select(entity => new ListItem(entity.Id, entity.Name))
        .ToList();

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var entity = await _manager
            .CreateAsync(name, (await _userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id, token)
            .ConfigureAwait(false);
        await _repository.InsertAsync(entity, token: token).ConfigureAwait(false);
        return entity.Id;
    }

    public async Task UpdateAsync(Guid id, TUpdateDto resource, CancellationToken token = default)
    {
        var entity = await _repository.GetAsync(id, token: token).ConfigureAwait(false);
        if (entity.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(entity, resource.Name, token).ConfigureAwait(false);
        entity.Active = resource.Active;
        entity.SetUpdater((await _userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await _repository.UpdateAsync(entity, token: token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) _repository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual async ValueTask DisposeAsyncCore()
    {
        await _repository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
