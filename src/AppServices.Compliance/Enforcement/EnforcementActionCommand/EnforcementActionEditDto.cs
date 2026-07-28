using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using FluentValidation;
using GaEpd.AppLibrary.DataAttributes;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record EnforcementActionEditDto : NotesDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Issued (Sent to Facility)")]
    [MaxDate]
    public DateOnly? IssueDate { get; init; }

    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; set; }

    [Display(Name = "Response Received")]
    public bool IsResponseReceived { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [MaxDate]
    [Display(Name = "Date Response Received")]
    public DateOnly? ResponseReceived { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Response")]
    public string? ResponseComment { get; set; }
}

public class EnforcementActionEditValidator : AbstractValidator<EnforcementActionEditDto>
{
    public EnforcementActionEditValidator()
    {
        RuleFor(dto => dto.IssueDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.IssueDate.HasValue)
            .WithMessage("The issued date cannot be in the future.");

        RuleFor(dto => dto)
            .Must(dto => dto.IssueDate.HasValue)
            .WithMessage("A response can only be entered for enforcement actions that have been issued.")
            .When(dto => dto.IsResponseReceived);

        RuleFor(dto => dto)
            .Must(dto => dto.ResponseReceived != null)
            .WithMessage("If Response Received is checked, a response date must be entered.")
            .When(dto => dto.IsResponseReceived && dto.IssueDate.HasValue);

        RuleFor(dto => dto)
            .Must(dto => dto.ResponseReceived <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The response date cannot be in the future.")
            .When(dto => dto is { IsResponseReceived: true, ResponseReceived: not null, IssueDate: not null })
            .Must(dto => dto.ResponseReceived >= dto.IssueDate)
            .WithMessage("The response date cannot be earlier than the issued date.")
            .When(dto => dto is { IsResponseReceived: true, ResponseReceived: not null, IssueDate: not null });
    }
}
