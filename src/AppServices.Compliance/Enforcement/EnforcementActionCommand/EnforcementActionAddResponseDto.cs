using AirWeb.AppServices.Core.Utilities;
using GaEpd.AppLibrary.DataAttributes;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record EnforcementActionAddResponseDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [MaxDate]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }
}

public class EnforcementActionAddResponseValidator : AbstractValidator<EnforcementActionAddResponseDto>
{
    public EnforcementActionAddResponseValidator()
    {
        RuleFor(dto => dto.Date)
            .Must(responseDate => responseDate <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The response date cannot be in the future.");

        RuleFor(dto => dto.Date)
            .Must((_, responseDate, context) =>
            {
                var issueDate = context.RootContextData.TryGetValue("enforcementAction.IssueDate", out var value)
                    ? (DateOnly?)value
                    : null;

                return issueDate is null || responseDate >= issueDate;
            })
            .WithMessage("The response date cannot be earlier than the issued date.");
    }
}
