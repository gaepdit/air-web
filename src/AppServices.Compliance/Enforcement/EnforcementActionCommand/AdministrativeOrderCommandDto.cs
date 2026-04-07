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
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The executed date cannot be in the future.");

        RuleFor(dto => dto.IssueDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The issued date cannot be in the future.");

        RuleFor(dto => dto.AppealedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The appealed date cannot be in the future.");

        RuleFor(dto => dto.ResolvedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The resolved date cannot be in the future.");

        RuleFor(dto => dto)
            .Must(dto => dto.IssueDate == null || dto.IssueDate >= dto.ExecutedDate)
            .WithMessage("The issued date cannot be before the executed date.")
            .Must(dto => dto.AppealedDate == null || dto.AppealedDate >= dto.IssueDate)
            .WithMessage("The appealed date cannot be before the issued date.")
            .Must(dto => dto.ResolvedDate == null || dto.ResolvedDate >= dto.IssueDate)
            .WithMessage("The resolved date cannot be before the issued date.")
            .Must(dto => dto.ResolvedDate == null || dto.AppealedDate == null || dto.ResolvedDate >= dto.AppealedDate)
            .WithMessage("The appealed date cannot be after the resolved date.");
    }
}
