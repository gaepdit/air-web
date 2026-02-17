using AirWeb.AppServices.Core.Utilities;

namespace AirWeb.AppServices.Core.CommonDtos;

public record MaxDateOnlyDto : IMaxCurrentDate
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Today);
}

public class MaxDateOnlyValidator : BaseMaxCurrentDateValidator<MaxDateOnlyDto>;
