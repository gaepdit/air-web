using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.Query;
using AirWeb.Domain.EnforcementEntities;
using AutoMapper;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementService(
    IMapper mapper,
    ICaseFileRepository caseFileRepository,
    IFacilityService facilityService) : IEnforcementService
{
    public async Task<CaseFileViewDto?> FindDetailedCaseFileAsync(int id, CancellationToken token = default)
    {
        var caseFile = mapper.Map<CaseFileViewDto?>(await caseFileRepository.FindDetailedCaseFileAsync(id, token)
            .ConfigureAwait(false));

        if (caseFile != null)
        {
            caseFile.FacilityName =
                await facilityService.GetNameAsync((FacilityId)caseFile.FacilityId).ConfigureAwait(false);
        }

        return caseFile;
    }

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
