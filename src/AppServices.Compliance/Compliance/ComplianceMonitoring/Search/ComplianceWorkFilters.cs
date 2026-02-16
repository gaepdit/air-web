using AirWeb.AppServices.Compliance.FacilitySearch;
using AirWeb.AppServices.Core.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;

internal static class ComplianceWorkFilters
{
    public static Expression<Func<ComplianceWork, bool>> SearchPredicate(ComplianceWorkSearchDto spec) =>
        PredicateBuilder.True<ComplianceWork>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByClosedStatus(spec.Closed)
            .ByWorkType(spec.Include)
            .ByFacilityId(spec.FacilityId)
            .ByOffice(spec.Office)
            .ByStaff(spec.Staff)
            .MinEventDate(spec.EventDateFrom)
            .MaxEventDate(spec.EventDateTo)
            .MinClosedDate(spec.ClosedDateFrom)
            .MaxClosedDate(spec.ClosedDateTo)
            .ByNotesText(spec.Notes);

    extension(Expression<Func<ComplianceWork, bool>> predicate)
    {
        public Expression<Func<ComplianceWork, bool>> ByWorkType(List<WorkTypeSearch> input)
        {
            if (input.Count == 0) return predicate;

            var includePredicate = PredicateBuilder.False<ComplianceWork>();

            if (input.Contains(WorkTypeSearch.Acc))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.AnnualComplianceCertification);

            if (input.Contains(WorkTypeSearch.Inspection))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.Inspection);

            if (input.Contains(WorkTypeSearch.Notification))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.Notification);

            if (input.Contains(WorkTypeSearch.PermitRevocation))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.PermitRevocation);

            if (input.Contains(WorkTypeSearch.Report))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.Report);

            if (input.Contains(WorkTypeSearch.Rmp))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.RmpInspection);

            if (input.Contains(WorkTypeSearch.Str))
                includePredicate = includePredicate
                    .Or(work => work.ComplianceWorkType == ComplianceWorkType.SourceTestReview);

            return predicate.And(includePredicate);
        }

        public Expression<Func<ComplianceWork, bool>> ByStaff(string? input) =>
            string.IsNullOrWhiteSpace(input)
                ? predicate
                : predicate.And(work => work.ResponsibleStaff != null && work.ResponsibleStaff.Id == input);

        public Expression<Func<ComplianceWork, bool>> ByOffice(Guid? input) =>
            input is null
                ? predicate
                : predicate.And(work =>
                    work.ResponsibleStaff != null &&
                    work.ResponsibleStaff.Office != null &&
                    work.ResponsibleStaff.Office.Id == input);

        public Expression<Func<ComplianceWork, bool>> MinEventDate(DateOnly? input) =>
            input is null
                ? predicate
                : predicate.And(work => work.EventDate >= input);

        public Expression<Func<ComplianceWork, bool>> MaxEventDate(DateOnly? input) =>
            input is null
                ? predicate
                : predicate.And(work => work.EventDate <= input);

        public Expression<Func<ComplianceWork, bool>> MinClosedDate(DateOnly? input) =>
            input is null
                ? predicate
                : predicate.And(work => work.ClosedDate >= input);

        public Expression<Func<ComplianceWork, bool>> MaxClosedDate(DateOnly? input) =>
            input is null
                ? predicate
                : predicate.And(work => work.ClosedDate <= input);
    }
}
