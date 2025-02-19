namespace AirWeb.AppServices.CommonDtos;

public record MaxCurrentDateOnlyDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Today);
}
