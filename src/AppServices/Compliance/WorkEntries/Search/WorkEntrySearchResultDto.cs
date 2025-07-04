﻿using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public record WorkEntrySearchResultDto : ISearchResult
{
    public int Id { get; init; }
    public WorkEntryType WorkEntryType { get; [UsedImplicitly] init; }
    public required string FacilityId { get; init; }
    public string? FacilityName { get; set; }
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly EventDate { get; init; }
    public required string EventDateName { get; init; }
    public bool IsReportable { get; init; }
    public bool IsClosed { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }
}
