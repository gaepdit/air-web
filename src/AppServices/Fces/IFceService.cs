using AirWeb.AppServices.CommonDtos;

namespace AirWeb.AppServices.Fces;

public interface IFceService : IDisposable, IAsyncDisposable
{
    Task<int> CreateAsync(FceCreateDto resource, CancellationToken token = default);

    Task<FceViewDto?> FindAsync(int id, CancellationToken token = default);

    Task<FceUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task UpdateAsync(int id, FceUpdateDto resource, CancellationToken token = default);

    Task AddCommentAsync(int id, AddCommentDto<int> addComment, CancellationToken token = default);

    Task DeleteAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);

    Task RestoreAsync(ChangeEntityStatusDto<int> resource, CancellationToken token = default);
}
