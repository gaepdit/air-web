using IaipDataService.Facilities;
using IaipDataService.PermitFees;

namespace IaipDataService.TestData;

public class TestPermitFeesService : IPermitFeesService
{
    public Task<List<AnnualFeeSummary>> GetAnnualFeesAsync(FacilityId facilityId, DateOnly cutoffDate,
        int lookBackYears, bool forceRefresh = false)
    {
        var annualFees = new List<AnnualFeeSummary>();
        var finalYear = (short)(cutoffDate.Month < 10 ? cutoffDate.Year - 1 : cutoffDate.Year);

        for (short i = 0; i < lookBackYears; i++)
        {
            var balanced = NextBoolean();
            var invoicedAmount = Random.Shared.Next(1, 50_000_000) / 100m;
            var paidAmount = balanced
                ? invoicedAmount
                : Random.Shared.Next(decimal.ToInt32(invoicedAmount - 1) * 100) / 100m;
            var balance = invoicedAmount - paidAmount;

            annualFees.Add(new AnnualFeeSummary
            {
                FeeYear = finalYear - i,
                InvoicedAmount = invoicedAmount,
                PaidAmount = paidAmount,
                Balance = balance,
                Status = balanced ? AnnualFeeStatus.PaidInFull : AnnualFeeStatus.PartialPayment,
            });
        }

        return Task.FromResult(annualFees);
    }

    private static bool NextBoolean() => Convert.ToBoolean(Random.Shared.Next(maxValue: 2));
}
