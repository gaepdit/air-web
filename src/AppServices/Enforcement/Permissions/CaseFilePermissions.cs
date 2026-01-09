using AirWeb.AppServices.AuthenticationServices.Roles;
using AirWeb.AppServices.DtoInterfaces;
using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.BaseEntities.Interfaces;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

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

        public bool CanDeleteCaseFile(IIsDeleted item) =>
            !item.IsDeleted && user.CanManageCaseFileDeletions();

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
