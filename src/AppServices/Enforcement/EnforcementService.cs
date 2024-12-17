using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.Query;
using AirWeb.Domain.EnforcementEntities;
using AutoMapper;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementService(
    IMapper mapper,
    ICaseFileRepository repository) : IEnforcementService
{
    public async Task<CaseFileViewDto?> FindAsync(int id, CancellationToken token = default) =>
        mapper.Map<CaseFileViewDto?>(await repository.FindAsync(id, token).ConfigureAwait(false));

    public Task<CreateResult<int>> CreateCaseFileAsync(CaseFileCreateDto resource, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
