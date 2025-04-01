namespace AirWeb.AppServices.CommonDtos;

public record MaxDateAndBooleanDto : MaxDateOnlyDto
{
    public bool Option { get; init; }
}

public class MaxDateAndBooleanValidator : BaseMaxCurrentDateValidator<MaxDateAndBooleanDto>;
