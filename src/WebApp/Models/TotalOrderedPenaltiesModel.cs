using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.WebApp.Models;

public sealed class TotalOrderedPenaties
{
    private readonly AppDbContext dbContext;
    private readonly int caseId;
    private readonly short[] orderedPenaltyIds = [];
    private readonly Guid[] stipulatedPenaltyIds = [];

    public decimal totalOrderedPenaltySum { get; private set; }
    public decimal totalStipulatedPenaltySum { get; private set; }

    public TotalOrderedPenaties(AppDbContext dbContext, int caseId)
    {
        this.dbContext = dbContext;
        this.caseId = caseId;
        if (caseId == 0) return;

        orderedPenaltyIds = GetAllPenaltyIdsFromCaseId().GetAwaiter().GetResult();
        stipulatedPenaltyIds = GetAllStipulatedPenaltyIdsFromCaseId().GetAwaiter().GetResult();

        totalOrderedPenaltySum = AmountSumOfAllOrderedPenaltiesFromThisCase().GetAwaiter().GetResult();
        totalStipulatedPenaltySum = AmountSumOfAllStipulatedPenaltiesFromThisCase().GetAwaiter().GetResult();
    }

    // Methods

    private async Task<short[]> GetAllPenaltyIdsFromCaseId(CancellationToken token = default) =>    // Getting the IDs of the ordered penalties, which are linked to the Case
        await dbContext.Set<ConsentOrder>()
            .Where(consentOrder => consentOrder.CaseFile.Id == caseId)
            .Where(consentOrder => !consentOrder.IsDeleted)
            .Where(consentOrder => consentOrder.OrderId != null)
            .Select(consentOrder => consentOrder.OrderId!.Value)
            .ToArrayAsync(token);

    private async Task<Guid[]> GetAllStipulatedPenaltyIdsFromCaseId(CancellationToken token = default) =>   // Getting the IDs of the stipulated penalties, which are linked to the Case
        await dbContext.Set<StipulatedPenalty>()
            .Where(stipulatedPenalty => stipulatedPenalty.ConsentOrder.CaseFile.Id == caseId)
            .Where(stipulatedPenalty => !stipulatedPenalty.IsDeleted)
            .Where(stipulatedPenalty => !stipulatedPenalty.ConsentOrder.IsDeleted)
            .Select(stipulatedPenalty => stipulatedPenalty.Id)
            .ToArrayAsync(token);

    // Adding Penalty amounts

    private async Task<decimal> AmountSumOfAllOrderedPenaltiesFromThisCase(CancellationToken token = default)
    {
        if (orderedPenaltyIds.Length == 0) return 0;

        var orderedPenaltiesAmountSum = 0m;

        foreach (var orderId in orderedPenaltyIds)
        {
            var amount = await dbContext.Set<ConsentOrder>()
                .Where(order => order.OrderId == orderId)
                .Select(order => order.PenaltyAmount ?? 0m)
                .SingleOrDefaultAsync(token);

            orderedPenaltiesAmountSum += amount;
        }

        return orderedPenaltiesAmountSum;
    }

    private async Task<decimal> AmountSumOfAllStipulatedPenaltiesFromThisCase(CancellationToken token = default)
    {
        if (stipulatedPenaltyIds.Length == 0) return 0;

        var stipulatedPenaltiesAmountSum = 0m;

        foreach (var stipulatedId in stipulatedPenaltyIds)
        {
            var amount = await dbContext.Set<StipulatedPenalty>()
                .Where(stipulation => stipulation.Id == stipulatedId)
                .Select(stipulation => stipulation.Amount)
                .SingleOrDefaultAsync(token);

            stipulatedPenaltiesAmountSum += amount;
        }

        return stipulatedPenaltiesAmountSum;
    }
}
