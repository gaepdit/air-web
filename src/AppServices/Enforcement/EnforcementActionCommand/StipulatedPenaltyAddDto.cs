using AirWeb.AppServices.Utilities;
using AirWeb.Domain.DataAttributes;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using FluentValidation;

namespace AirWeb.AppServices.Enforcement.EnforcementActionCommand;

public record StipulatedPenaltyAddDto
{
    [PositiveDecimal(ErrorMessage = "The stipulated penalty amount must be a positive number.")]
    [Display(Name = "Penalty Amount")]
    public decimal Amount { get; init; } = 1;

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly? ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Comment")]
    public string? Notes { get; init; }
}

public class StipulatedPenaltyAddValidator : AbstractValidator<StipulatedPenaltyAddDto>
{
    private readonly IEnforcementActionRepository _repository;

    public StipulatedPenaltyAddValidator(IEnforcementActionRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.ReceivedDate)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date received cannot be in the future.")
            .MustAsync(async (_, receivedDate, context, token) =>
                await NotBeforeExecuted(receivedDate, context, token).ConfigureAwait(false))
            .WithMessage("A stipulated penalty cannot be received before the Consent Order is executed.");

        RuleFor(dto => dto.Amount).GreaterThan(0).WithMessage("The penalty amount must be greater than zero.");
    }

    private async Task<bool> NotBeforeExecuted(DateOnly? receivedDate,
        ValidationContext<StipulatedPenaltyAddDto> context, CancellationToken token)
    {
        if (receivedDate == null) return true;
        if (!context.RootContextData.TryGetValue("ConsentOrder.Id", out var coId) || coId == null)
            return true;
        var executedDate = ((ConsentOrder)await _repository.GetAsync((Guid)coId, token: token)
            .ConfigureAwait(false)).ExecutedDate;
        return receivedDate >= executedDate;
    }
}
