using AirWeb.AppServices.Core.Utilities;
using GaEpd.AppLibrary.DataAttributes;

namespace AirWeb.AppServices.Core.CommonDtos;

public record MaxDateOnlyDto : IMaxCurrentDate
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [MaxDate]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Today);
}

public class MaxDateOnlyValidator : BaseMaxCurrentDateValidator<MaxDateOnlyDto>;
