using AirWeb.Domain.EnforcementEntities.CaseFiles;
using GaEpd.AppLibrary.Domain.Predicates;
using IaipDataService.Facilities;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Enforcement.Search
{
    internal static class EnforcementFilters
    {
        public static Expression<Func<CaseFile, bool>> SearchPredicate(EnforcementSearchDto spec) =>
            PredicateBuilder.True<CaseFile>()
                .ByFacilityId(spec.PartialFacilityId);

        private static Expression<Func<CaseFile, bool>> ByFacilityId(
            this Expression<Func<CaseFile, bool>> predicate,
            string? input)
        {
            var cleanInput = FacilityId.CleanFacilityId(input);
            return string.IsNullOrWhiteSpace(cleanInput)
                ? predicate
                : predicate.And(entry => entry.FacilityId.Contains(cleanInput));
        }
    }
}
