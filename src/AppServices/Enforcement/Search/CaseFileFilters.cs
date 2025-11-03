using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Enforcement.Search;

internal static class CaseFileFilters
{
    public static Expression<Func<CaseFile, bool>> SearchPredicate(CaseFileSearchDto spec) =>
        PredicateBuilder.True<CaseFile>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByClosedStatus(spec.Closed)
            .ByCaseFileStatus(spec.CaseFileStatus)
            .ByFacilityId(spec.FacilityId)
            .ByStaff(spec.Staff)
            .ByOffice(spec.Office)
            .ByNotesText(spec.Notes)
            .ByViolationType(spec.ViolationType)
            .MinDiscoveryDate(spec.DiscoveryDateFrom)
            .MaxDiscoveryDate(spec.DiscoveryDateTo)
            .MinEnforcementDate(spec.EnforcementDateFrom)
            .MaxEnforcementDate(spec.EnforcementDateTo);

    public static Expression<Func<CaseFile, bool>> ByCaseFileStatus(
        this Expression<Func<CaseFile, bool>> predicate,
        CaseFileStatus? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile => caseFile.CaseFileStatus == input);

    public static Expression<Func<CaseFile, bool>> ByStaff(
        this Expression<Func<CaseFile, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(caseFile => caseFile.ResponsibleStaff != null && caseFile.ResponsibleStaff.Id == input);

    public static Expression<Func<CaseFile, bool>> ByOffice(
        this Expression<Func<CaseFile, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile =>
                caseFile.ResponsibleStaff != null &&
                caseFile.ResponsibleStaff.Office != null &&
                caseFile.ResponsibleStaff.Office.Id == input);

    public static Expression<Func<CaseFile, bool>> ByViolationType(
        this Expression<Func<CaseFile, bool>> predicate,
        string? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile =>
                caseFile.ViolationType != null &&
                caseFile.ViolationType.Code == input);

    public static Expression<Func<CaseFile, bool>> MinDiscoveryDate(
        this Expression<Func<CaseFile, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile => caseFile.DiscoveryDate >= input);

    public static Expression<Func<CaseFile, bool>> MaxDiscoveryDate(
        this Expression<Func<CaseFile, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile => caseFile.DiscoveryDate <= input);

    public static Expression<Func<CaseFile, bool>> MinEnforcementDate(
        this Expression<Func<CaseFile, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile => caseFile.EnforcementDate >= input);

    public static Expression<Func<CaseFile, bool>> MaxEnforcementDate(
        this Expression<Func<CaseFile, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(caseFile => caseFile.EnforcementDate <= input);
}
