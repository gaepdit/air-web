﻿using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Search;

public record WorkEntrySearchResultDto : IStandardSearchResult
{
    public int Id { get; init; }
    public WorkEntryType WorkEntryType { get; [UsedImplicitly] init; }
    public Facility Facility { get; set; } = default!;
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly EventDate { get; init; }
    public required string EventDateName { get; init; }
    public bool IsClosed { get; init; }
    public DateOnly? ClosedDate { get; init; }
    public bool IsDeleted { get; init; }
}
