using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Enforcement.Search
{
    internal static class CaseFileFilters
    {
        public static Expression<Func<CaseFile, bool>> SearchPredicate(CaseFileSearchDto spec) =>
            PredicateBuilder.True<CaseFile>()
                .ByDeletedStatus(spec.DeleteStatus)
                .ByClosedStatus(spec.Closed)
                .ByFacilityId(spec.PartialFacilityId)

                // .ByReviewer(spec.ReviewedBy)
                // .ByOffice(spec.Office)
                // .FromDate(spec.DateFrom)
                // .ToDate(spec.DateTo)
                .ByNotesText(spec.Notes);
    }
}
