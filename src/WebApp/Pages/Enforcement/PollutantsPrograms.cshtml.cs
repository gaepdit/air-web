using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.WebApp.Models;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class PollutantsProgramsModel(ICaseFileService caseFileService, IFacilityService facilityService) : PageModel
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    [BindProperty]
    public List<PollutantSetting> PollutantSettings { get; set; } = [];

    [BindProperty]
    public List<AirProgramSetting> AirProgramSettings { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");
        var caseFile = await caseFileService.FindSummaryAsync(Id, token);
        if (caseFile is null) return NotFound();
        if (caseFile.IsClosed) return BadRequest();
        if (!User.CanEditCaseFile(caseFile)) return Forbid();

        var facilityData = (await facilityService.FindFacilityDetailsAsync((FacilityId?)caseFile.FacilityId))
            ?.RegulatoryData;
        if (facilityData is null) return NotFound();

        var caseFilePollutants = await caseFileService.GetPollutantsAsync(Id, token);
        PollutantSettings = facilityData.Pollutants.Select(pollutant => new PollutantSetting
        {
            Code = pollutant.Code,
            Description = pollutant.Description,
            IsSelected = caseFilePollutants.Any(cfPollutant => cfPollutant.Code == pollutant.Code)
        }).ToList();

        var caseFileAirPrograms = await caseFileService.GetAirProgramsAsync(Id, token);
        AirProgramSettings = facilityData.AirPrograms.Select(airProgram => new AirProgramSetting
        {
            AirProgram = airProgram,
            IsSelected = caseFileAirPrograms.Any(cfAirProgram => cfAirProgram == airProgram)
        }).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (Id == 0) return BadRequest();
        var caseFile = await caseFileService.FindSummaryAsync(Id, token);
        if (caseFile is null || caseFile.IsClosed || !User.CanEditCaseFile(caseFile)) return BadRequest();

        await caseFileService.SavePollutantsAndProgramsAsync(Id,
            pollutants: PollutantSettings.Where(setting => setting.IsSelected).Select(setting => setting.Code),
            airPrograms: AirProgramSettings.Where(setting => setting.IsSelected).Select(setting => setting.AirProgram),
            token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Enforcement successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    public class PollutantSetting
    {
        public required string Code { get; init; }
        public required string Description { get; init; }
        public bool IsSelected { get; init; }
    }

    public class AirProgramSetting
    {
        public required AirProgram AirProgram { get; init; }
        public bool IsSelected { get; init; }
    }
}
