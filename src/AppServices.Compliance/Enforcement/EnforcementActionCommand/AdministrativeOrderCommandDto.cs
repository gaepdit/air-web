using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Core.Data.DataAttributes;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record AdministrativeOrderCommandDto : NotesDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Executed")]
    [MaxDate]
    public DateOnly? ExecutedDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Issued")]
    [MaxDate]
    public DateOnly? IssueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Appealed")]
    [MaxDate]
    public DateOnly? AppealedDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Resolved")]
    [MaxDate]
    public DateOnly? ResolvedDate { get; init; }
}

public class AdministrativeOrderCommandValidator : AbstractValidator<AdministrativeOrderCommandDto>
{
    public AdministrativeOrderCommandValidator()
    {
        RuleFor(dto => dto.ExecutedDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.ExecutedDate.HasValue)
            .WithMessage("The executed date cannot be in the future.");

        RuleFor(dto => dto.IssueDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.IssueDate.HasValue)
            .WithMessage("The issued date cannot be in the future.");

        RuleFor(dto => dto.AppealedDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.AppealedDate.HasValue)
            .WithMessage("The appealed date cannot be in the future.");

        RuleFor(dto => dto.ResolvedDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.ResolvedDate.HasValue)
            .WithMessage("The resolved date cannot be in the future.");

        RuleFor(dto => dto)
            .Must(dto => dto.IssueDate >= dto.ExecutedDate)
            .When(dto => dto.IssueDate.HasValue && dto.ExecutedDate.HasValue)
            .WithMessage("The issued date cannot be before the executed date.");

        RuleFor(dto => dto.ExecutedDate)
            .NotNull()
            .When(dto => dto.IssueDate.HasValue)
            .WithMessage("The issued date cannot be entered without an executed date.");

        RuleFor(dto => dto)
            .Must(dto => dto.AppealedDate >= dto.IssueDate)
            .When(dto => dto.AppealedDate.HasValue && dto.IssueDate.HasValue)
            .WithMessage("The appealed date cannot be before the issued date.");

        RuleFor(dto => dto.IssueDate)
            .NotNull()
            .When(dto => dto.AppealedDate.HasValue)
            .WithMessage("The appealed date cannot be entered without an issued date.");

        RuleFor(dto => dto)
            .Must(dto => dto.ResolvedDate >= dto.IssueDate)
            .When(dto => dto.ResolvedDate.HasValue && dto.IssueDate.HasValue)
            .WithMessage("The resolved date cannot be before the issued date.");

        RuleFor(dto => dto.IssueDate)
            .NotNull()
            .When(dto => dto.ResolvedDate.HasValue)
            .WithMessage("The resolved date cannot be entered without an issued date.");

        RuleFor(dto => dto)
            .Must(dto => dto.ResolvedDate >= dto.AppealedDate)
            .When(dto => dto.ResolvedDate.HasValue && dto.AppealedDate.HasValue)
            .WithMessage("The appealed date cannot be after the resolved date.");
    }
}
