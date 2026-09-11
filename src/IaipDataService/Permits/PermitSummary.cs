using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Permits;

[SuppressMessage("Major Code Smell",
    "S3928:Parameter names used into ArgumentException constructors should match an existing one ")]
public record PermitSummary
{
    public required string FacilityId { get; init; }
    public required string FacilityName { get; init; }
    public required string PermitNumber { get; init; }
    public DateTime? IssuanceDate { get; init; }
    public required string FileType { get; init; }
    public string? VNarrative { get; init; }
    public string? VFinal { get; init; }
    public string? PsdAppSum { get; init; }
    public string? PsdPrelim { get; init; }
    public string? PsdNarrative { get; init; }
    public string? PsdFinalDet { get; init; }
    public string? PsdFinal { get; init; }
    public string? OtherNarrative { get; init; }
    public string? OtherPermit { get; init; }

    public string? PermitFile => FileType switch
    {
        FileTypes.Psd => PsdFinal,
        FileTypes.Sip => OtherPermit,
        FileTypes.TitleV => VFinal,
        _ => throw new ArgumentOutOfRangeException()
    };

    public string? NarrativeFile => FileType switch
    {
        FileTypes.Psd => PsdNarrative,
        FileTypes.Sip => OtherNarrative,
        FileTypes.TitleV => VNarrative,
        _ => throw new ArgumentOutOfRangeException()
    };

    public static class FileTypes
    {
        public const string Psd = "PSD/NSR";
        public const string Sip = "SIP";
        public const string TitleV = "Title V";
    }
}
