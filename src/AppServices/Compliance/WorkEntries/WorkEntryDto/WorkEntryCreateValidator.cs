using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public class WorkEntryCreateValidator : AbstractValidator<IWorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.FacilityId).NotEmpty();
        RuleFor(dto => dto.ResponsibleStaffId).NotEmpty();
        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must(date => date is null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Acknowledgment Letter Date cannot be in the future.");
    }
}
