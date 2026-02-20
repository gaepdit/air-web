using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.DtoInterfaces;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Core.BaseEntities;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Compliance.Permissions;

public static class CompliancePermissions
{
    extension(ClaimsPrincipal user)
    {
        // FUTURE: Move these common permissions elsewhere. --
        public bool CanAddComment(IIsDeleted item) => !item.IsDeleted && user.IsComplianceStaff();

        public bool CanClose<T>(T item) where T : IIsClosed, IIsDeleted => !item.IsClosed && user.CanFinalize(item);

        public bool CanDelete(IIsDeleted item) => !item.IsDeleted && user.CanManageDeletions();

        public bool CanDeleteComment<T>(T item) where T : IIsDeleted, IHasOwner =>
            !item.IsDeleted && (user.CanManageDeletions() || user.IsOwner(item));
        // -- end common permissions

        public bool CanEdit<T>(T item) where T : IIsClosed, IIsDeleted =>
            item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

        public bool CanEdit(IIsDeleted item) => !item.IsDeleted && user.IsComplianceStaff();

        public bool CanFinalize(IIsDeleted item) => !item.IsDeleted && user.IsComplianceStaff();

        public bool CanManageDeletions() => user.IsComplianceManager();

        public bool CanReopen<T>(T item) where T : IIsClosed, IIsDeleted => item.IsClosed && user.CanFinalize(item);

        public bool CanRestore(IIsDeleted item) => item.IsDeleted && user.CanManageDeletions();

        // FUTURE: Reevaluate the following
        public bool CanView<T>(T item) where T : IIsClosed, IIsDeleted => user.CanManageDeletions() ||
                                                                          !item.IsDeleted && user.IsComplianceStaff() ||
                                                                          item.IsClosed && user.IsStaff();

        public bool CanViewDraftEnforcement<T>(T item) where T : IIsClosed, IIsDeleted => user.IsComplianceStaff();
    }
}
