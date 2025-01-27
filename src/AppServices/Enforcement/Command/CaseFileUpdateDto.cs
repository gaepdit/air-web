using AirWeb.AppServices.Enforcement.CaseFiles;

namespace AirWeb.AppServices.Enforcement.Command;

public record CaseFileUpdateDto
{
    public CaseFileUpdateDto() { }

    public CaseFileUpdateDto(CaseFileSummaryDto caseFile)
    {
        ResponsibleStaffId = caseFile.ResponsibleStaff?.Id;
        DiscoveryDate = caseFile.DiscoveryDate ?? DateOnly.FromDateTime(DateTime.Today);
        Notes = caseFile.Notes;
    }

    // Data properties
    [Required]
    [Display(Name = "Staff responsible")]
    public string? ResponsibleStaffId { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Discovery date")]
    public DateOnly DiscoveryDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
}
