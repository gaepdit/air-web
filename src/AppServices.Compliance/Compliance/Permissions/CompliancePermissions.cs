using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;
using AirWeb.Domain.Core.BaseEntities;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Compliance.Permissions;

public static class CompliancePermissions
{
    extension(ClaimsPrincipal user)
    {
        public bool CanClose<T>(T item) where T : IIsClosed, IIsDeleted => !item.IsClosed && user.CanFinalize(item);

        public bool CanDelete(IIsDeleted item) => !item.IsDeleted && user.CanManageDeletions();

        public bool CanEdit<T>(T item) where T : IIsClosed, IIsDeleted =>
            item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

        public bool CanEdit(IIsDeleted item) => !item.IsDeleted && user.IsComplianceStaff();

        private bool CanFinalize(IIsDeleted item) =>
            !item.IsDeleted && user.IsComplianceStaff() &&
            // A reviewable item can only be closed if it has been reviewed.
            item is not IReviewedDate { ReviewedDate: null };

        public bool CanManageDeletions() => user.IsComplianceStaff();

        public bool CanReopen<T>(T item) where T : IIsClosed, IIsDeleted => item.IsClosed && user.CanFinalize(item);

        public bool CanRestore(IIsDeleted item) => item.IsDeleted && user.CanManageDeletions();

        public bool CanBeginEnforcement(IComplianceWorkSummaryDto item) =>
            item is { IsComplianceEvent: true, IsDeleted: false } && user.IsComplianceStaff();
    }
}
