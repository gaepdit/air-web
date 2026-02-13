using AirWeb.AppServices.Core.Utilities;
using AirWeb.AppServices.Enforcement.CaseFileQuery;

namespace AirWeb.AppServices.Enforcement.CaseFileCommand;

public record CaseFileUpdateDto
{
    public CaseFileUpdateDto() { }

    public CaseFileUpdateDto(CaseFileViewDto caseFile)
    {
        ResponsibleStaffId = caseFile.ResponsibleStaff?.Id;
        DiscoveryDate = caseFile.DiscoveryDate ?? DateOnly.FromDateTime(DateTime.Today);
        Notes = caseFile.Notes;
        ViolationTypeCode = caseFile.ViolationType?.Code;
    }

    // Data properties
    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Discovery Date")]
    public DateOnly DiscoveryDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    [Display(Name = "Violation Type")]
    public string? ViolationTypeCode { get; init; }
}
