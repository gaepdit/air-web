namespace IaipDataService.Permits;

public record PermitSummary
{
    public int ApplicationNumber { get; set; }
    public string FacilityId { get; set; }
    public string FacilityName { get; set; }
    public string PermitNumber { get; set; }
    public string IssuanceDate { get; set; }
    public string FileType { get; set; }
    public string VNarrative { get; set; }
    public string VFinal { get; set; }
    public string PSDAppSum { get; set; }
    public string PSDPrelim { get; set; }
    public string PSDNarrative { get; set; }
    public string PSDFinalDet { get; set; }
    public string PSDFinal { get; set; }
    public string OtherNarrative { get; set; }
    public string OtherPermit { get; set; }
}
