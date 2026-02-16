using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.DtoInterfaces;
using AirWeb.AppServices.Compliance.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Core.BaseEntities;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Enforcement.Permissions;

public static class CaseFilePermissions
{
    extension(ClaimsPrincipal user)
    {
        public bool CanViewCaseFile<T>(T item)
            where T : IIsClosed, IIsDeleted =>
            user.CanManageCaseFileDeletions() || !item.IsDeleted && user.IsComplianceStaff() ||
            item.IsClosed && user.IsStaff();

        public bool CanCloseCaseFile<T>(T item)
            where T : IIsClosed, IIsDeleted =>
            item is { IsClosed: false, IsDeleted: false } && user.IsEnforcementManager();

        public bool CanManageCaseFileDeletions() =>
            user.IsEnforcementManager();

        public bool CanDeleteCaseFile(CaseFileViewDto item) =>
            !item.IsDeleted && user.CanManageCaseFileDeletions() && !item.HasIssuedEnforcement;

        public bool CanEditCaseFile<T>(T item)
            where T : IIsClosed, IIsDeleted, IHasOwner =>
            item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

        public bool CanReopenCaseFile<T>(T item)
            where T : IIsClosed, IIsDeleted =>
            item is { IsClosed: true, IsDeleted: false } && user.IsEnforcementManager();

        public bool CanRestoreCaseFile(IIsDeleted item) =>
            item.IsDeleted && user.CanManageCaseFileDeletions();
    }
}
