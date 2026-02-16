using AirWeb.Domain.Compliance.Facility;
using GaEpd.AppLibrary.Domain.Predicates;
using IaipDataService.Facilities;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.FacilitySearch;

internal static class FacilityFilter
{
    public static Expression<Func<TEntity, bool>> ByFacilityId<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : IFacilityId
    {
        if (string.IsNullOrWhiteSpace(input))
            return predicate;

        if (!FacilityId.IsValidFormat(input))
            return PredicateBuilder.False<TEntity>();

        return predicate.And(entity => entity.FacilityId == new FacilityId(input).FormattedId);
    }
}
