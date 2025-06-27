﻿using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.Enforcement.Search;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record CaseFileExportDto : ISearchResult
{
    public CaseFileExportDto(CaseFile caseFile, string deleted)
    {
        CaseFileId = caseFile.Id;
        FacilityId = caseFile.FacilityId;
        ResponsibleStaff = caseFile.ResponsibleStaff?.SortableFullName;
        CaseFileStatus = caseFile.CaseFileStatus.GetDescription();
        ViolationType = caseFile.ViolationType == null
            ? ""
            : $"{caseFile.ViolationType.SeverityCode}: {caseFile.ViolationType.Description}";
        DiscoveryDate = caseFile.DiscoveryDate;
        DayZero = caseFile.DayZero;
        EnforcementDate = caseFile.EnforcementDate;
        Notes = caseFile.Notes;
        Deleted = caseFile.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Case File ID")]
    public int CaseFileId { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string FacilityId { get; init; }

    [XLColumn(Header = "Facility")]
    public string? FacilityName { get; set; }

    [XLColumn(Header = "Responsible Staff")]
    public string? ResponsibleStaff { get; init; }

    [XLColumn(Header = "Case File Status")]
    public string? CaseFileStatus { get; init; }

    [XLColumn(Header = "Violation Type")]
    public string? ViolationType { get; init; }

    [XLColumn(Header = "Discovery Date")]
    public DateOnly? DiscoveryDate { get; init; }

    [XLColumn(Header = "Day Zero")]
    public DateOnly? DayZero { get; init; }

    [XLColumn(Header = "Date of initial Enforcement Action")]
    public DateOnly? EnforcementDate { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
