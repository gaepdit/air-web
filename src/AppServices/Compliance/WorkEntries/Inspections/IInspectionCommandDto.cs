﻿using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public interface IInspectionCommandDto
{
    public DateOnly InspectionStartedDate { get; }
    public TimeOnly InspectionStartedTime { get; }
    public DateOnly InspectionEndedDate { get; }
    public TimeOnly InspectionEndedTime { get; }
    public InspectionReason? InspectionReason { get; }
    public string WeatherConditions { get; }
    public string InspectionGuide { get; }
    public bool FacilityOperating { get; }
    public ComplianceStatus ComplianceStatus { get; }
    public bool FollowupTaken { get; }
}
