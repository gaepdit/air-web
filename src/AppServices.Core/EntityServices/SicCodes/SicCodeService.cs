using AirWeb.Domain.Core.Entities;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.Core.EntityServices.SicCodes;

public interface ISicCodeService : IDisposable, IAsyncDisposable
{
    Task<IReadOnlyList<ListItem<string>>> GetActiveListItemsAsync(CancellationToken token = default);
}

public sealed class SicCodeService(ISicCodeRepository repository) : ISicCodeService
{
    public async Task<IReadOnlyList<ListItem<string>>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await repository.GetActiveListAsync(token).ConfigureAwait(false))
        .Select(sic => new ListItem<string>(sic.Id, Name: sic.Display)).ToList();

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();

    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
