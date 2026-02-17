namespace AirWeb.AppServices.Core.CommonDtos;

public record MaxDateAndBooleanDto : MaxDateOnlyDto
{
    public bool Option { get; set; }
}

public class MaxDateAndBooleanValidator : BaseMaxCurrentDateValidator<MaxDateAndBooleanDto>;
