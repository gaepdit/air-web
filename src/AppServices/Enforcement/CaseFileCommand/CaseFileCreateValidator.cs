using AirWeb.Domain.ComplianceEntities.WorkEntries;
using FluentValidation;

namespace AirWeb.AppServices.Enforcement.CaseFileCommand;

public class CaseFileCreateValidator : AbstractValidator<CaseFileCreateDto>
{
    private readonly IWorkEntryRepository _entryRepository;

    public CaseFileCreateValidator(IWorkEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;

        RuleFor(dto => dto.ResponsibleStaffId).NotEmpty();
        RuleFor(dto => dto.DiscoveryDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Discovery Date cannot be in the future.")
            .MustAsync(async (dto, _, token) =>
                await MustNotPrecedeDiscoveryEvent(dto, token).ConfigureAwait(false))
            .WithMessage("The Discovery Date cannot precede the Compliance Event date.");
    }

    private async Task<bool> MustNotPrecedeDiscoveryEvent(CaseFileCreateDto dto, CancellationToken token) =>
        dto.EventId is null ||
        // FUTURE: Replace with FindAsync using a query projection (anonymous type with only executed date as a single property).
        (await _entryRepository.GetAsync(dto.EventId.Value, token: token).ConfigureAwait(false))
        .EventDate <= dto.DiscoveryDate;
}
