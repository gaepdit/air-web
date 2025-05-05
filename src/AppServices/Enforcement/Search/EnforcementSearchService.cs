using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Users;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Search
{
    public sealed class EnforcementSearchService(

        IFacilityService facilityService,
        IMapper mapper,
        IUserService userService,
        IAuthorizationService authorization)
    {
        public async Task<IPaginatedResult<EnforcementSearchResultDto>> SearchAsync(EnforcementSearchDto spec,
            PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
        {
            return List<EnforcementSearchResultDto> { new EnforcementSearchResultDto { FacilityId = "00100010", DiscoveryDate = new DateOnly(2025,4,22), CaseFileStatus = Domain.EnforcementEntities.CaseFiles.CaseFileStatus.Open };
        }
        public async Task<int> CountAsync(EnforcementSearchDto spec, CancellationToken token)
        {
            return 1;
        }
    }
}
