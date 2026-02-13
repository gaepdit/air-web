using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using FluentValidation;

namespace AirWeb.AppServices.Enforcement.EnforcementActionCommand;

public record AdministrativeOrderCommandDto : CommentDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Issued")]
    public DateOnly? IssueDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Appealed")]
    public DateOnly? AppealedDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }
}

public class AdministrativeOrderCommandValidator : AbstractValidator<AdministrativeOrderCommandDto>
{
    public AdministrativeOrderCommandValidator()
    {
        RuleFor(dto => dto.ExecutedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date executed cannot be in the future.");

        RuleFor(dto => dto.IssueDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date issued cannot be in the future.");

        RuleFor(dto => dto.AppealedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date appealed cannot be in the future.");

        RuleFor(dto => dto.ResolvedDate)
            .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date resolved cannot be in the future.");

        RuleFor(dto => dto)
            .Must(dto => dto.IssueDate == null || dto.IssueDate >= dto.ExecutedDate)
            .WithMessage("The order cannot be issued before it is executed.")
            .Must(dto => dto.AppealedDate == null || dto.AppealedDate >= dto.IssueDate)
            .WithMessage("The order cannot be appealed before it is issued.")
            .Must(dto => dto.ResolvedDate == null || dto.ResolvedDate >= dto.IssueDate)
            .WithMessage("The order cannot be resolved before it is issued.")
            .Must(dto => dto.ResolvedDate == null || dto.AppealedDate == null || dto.ResolvedDate >= dto.AppealedDate)
            .WithMessage("The order cannot be appealed after it is resolved.");
    }
}
