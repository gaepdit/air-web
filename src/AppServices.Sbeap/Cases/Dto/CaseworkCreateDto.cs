using AirWeb.AppServices.Core.Utilities;

namespace AirWeb.AppServices.Sbeap.Cases.Dto;

public record CaseworkCreateDto(Guid CustomerId)
{
    [Display(Name = "Date Opened")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly CaseOpenedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    public string? Description { get; init; } = string.Empty;
}
