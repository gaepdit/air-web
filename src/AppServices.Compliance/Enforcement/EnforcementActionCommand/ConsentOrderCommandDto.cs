using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Core.Data.DataAttributes;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record ConsentOrderCommandDto : NotesDto
{
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Signed Copy Received From Facility")]
    [MaxDate]
    public DateOnly? ReceivedFromFacility { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Executed")]
    [MaxDate]
    public DateOnly? ExecutedDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Received From Director's Office")]
    [MaxDate]
    public DateOnly? ReceivedFromDirectorsOffice { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Issued")]
    [MaxDate]
    public DateOnly? IssueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Resolved")]
    [MaxDate]
    public DateOnly? ResolvedDate { get; init; }

    [PositiveShort(ErrorMessage = "The Order ID must be a positive number.")]
    [Display(Name = "Order Number")]
    public short OrderId { get; init; } = 1;

    [PositiveDecimal(ErrorMessage = "The penalty amount must be a positive number.")]
    [Display(Name = "Penalty Assessed")]
    public decimal PenaltyAmount { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Penalty Comment")]
    public string? PenaltyComment { get; init; }

    [Display(Name = "Defines Stipulated Penalties")]
    public bool StipulatedPenaltiesDefined { get; init; }
}

public class ConsentOrderCommandValidator : AbstractValidator<ConsentOrderCommandDto>
{
    private readonly IEnforcementActionRepository _repository;

    public ConsentOrderCommandValidator(IEnforcementActionRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.ReceivedFromFacility)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date received from the facility cannot be in the future.");

        RuleFor(dto => dto.ExecutedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The executed date cannot be in the future.");

        RuleFor(dto => dto.ReceivedFromDirectorsOffice)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date received from the Director's Office cannot be in the future.");

        RuleFor(dto => dto.IssueDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The issued date cannot be in the future.");

        RuleFor(dto => dto.ResolvedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The resolved date cannot be in the future.");

        RuleFor(dto => dto)
            .Must(dto => dto.ExecutedDate == null || dto.ReceivedFromFacility == null ||
                         dto.ExecutedDate >= dto.ReceivedFromFacility)
            .WithMessage("The order cannot be executed before it is received from the facility.")
            .Must(dto => dto.IssueDate == null || dto.IssueDate >= dto.ExecutedDate)
            .WithMessage("The issued date cannot be before the executed date.")
            .Must(dto => dto.ReceivedFromDirectorsOffice == null || dto.ReceivedFromFacility == null ||
                         dto.ReceivedFromDirectorsOffice >= dto.ReceivedFromFacility)
            .WithMessage(
                "The order cannot be received from the Director's Office before it is received from the facility.")
            .Must(dto => dto.ResolvedDate == null || dto.ResolvedDate >= dto.ExecutedDate)
            .WithMessage("The resolved date cannot be before the executed date.")
            .Must(dto => dto.ResolvedDate == null || dto.ResolvedDate >= dto.IssueDate)
            .WithMessage("The resolved date cannot be before the issued date.");

        RuleFor(dto => dto.PenaltyAmount).GreaterThanOrEqualTo(0);

        RuleFor(dto => dto.OrderId)
            .GreaterThan((short)0)
            .WithMessage("The order ID must be greater than zero.")
            .MustAsync(async (_, orderId, context, token) =>
                await UniqueOrderId(orderId, context, token).ConfigureAwait(false))
            .WithMessage("The Order ID entered already exists.");
    }

    private async Task<bool> UniqueOrderId(short orderId, ValidationContext<ConsentOrderCommandDto> context,
        CancellationToken token)
    {
        var actionId = context.RootContextData.TryGetValue("Id", out var value)
            ? (Guid?)value
            : null;

        return !await _repository.OrderIdExists(orderId, actionId, token).ConfigureAwait(false);
    }
}
