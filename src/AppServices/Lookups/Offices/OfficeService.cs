using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Lookups.LookupsBase;
using AirWeb.Core.Entities;
using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Lookups.Offices;

public class OfficeService(
    IMapper mapper,
    IOfficeRepository repository,
    IOfficeManager manager,
    IUserService userService,
    IAuthorizationService authorization)
    : LookupService<Office, OfficeViewDto, OfficeUpdateDto>
        (mapper, repository, manager, userService),
        IOfficeService
{
    private readonly IUserService _userService = userService;

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id = null,
        bool includeInactive = false,
        CancellationToken token = default)
    {
        if (id is null) return [];

        var user = _userService.GetCurrentPrincipal();

        if (includeInactive &&
            (user is null || !await authorization.Succeeded(user, Policies.ActiveUser).ConfigureAwait(false)))
            includeInactive = false;

        return (await repository.GetStaffMembersListAsync(id.Value, includeInactive, token).ConfigureAwait(false))
            .Select(staff => new ListItem<string>(staff.Id, staff.SortableNameWithInactive)).ToList();
    }
}
