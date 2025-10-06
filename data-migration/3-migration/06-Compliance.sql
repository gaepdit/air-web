use AirWeb
go

select

-- WorkEntry
Id,
FacilityId,
WorkEntryType,
ResponsibleStaffId,
AcknowledgmentLetterDate,
Notes,
EventDate,
IsComplianceEvent,

-- ComplianceEvent
DataExchangeStatus,

-- AnnualComplianceCertification, Notification, Report
ReceivedDate,

-- AnnualComplianceCertification
AccReportingYear,
PostmarkDate,
PostmarkedOnTime,
SignedByRo,
OnCorrectForms,
IncludesAllTvConditions,
CorrectlyCompleted,
ReportsDeviations,
IncludesPreviouslyUnreportedDeviations,
ReportsAllKnownDeviations,
ResubmittalRequired,

-- AnnualComplianceCertification, Report
EnforcementNeeded,

-- Inspection
InspectionReason,
InspectionStarted,
InspectionEnded,
WeatherConditions,
InspectionGuide,
FacilityOperating,
DeviationsNoted,

-- Inspection, Notification, PermitRevocation, SourceTestReview
FollowupTaken,

-- Report
ReportingPeriodType,
ReportingPeriodStart,
ReportingPeriodEnd,
ReportingPeriodComment,

-- Notification, Report, SourceTestReview
DueDate,

-- Notification, Report
SentDate,

-- Report
ReportComplete,

-- SourceTestReview
ReferenceNumber,
ReceivedByComplianceDate,

-- Notification
NotificationTypeId,

-- PermitRevocation
PermitRevocationDate,
PhysicalShutdownDate,

-- WorkEntry
CreatedAt,
CreatedById,
UpdatedAt,
UpdatedById,
IsDeleted,
DeletedAt,
DeletedById,
DeleteComments,
IsClosed,
ClosedById,
ClosedDate

from AirWeb.dbo.ComplianceWork;

-- insert into AirWeb.dbo.ComplianceWork
-- (
--     -- WorkEntry
--     Id, FacilityId, WorkEntryType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate, IsComplianceEvent,
-- 
--     -- ComplianceEvent
--     DataExchangeStatus,
-- 
--     -- AnnualComplianceCertification, Notification, Report
--     ReceivedDate,
-- 
--     -- AnnualComplianceCertification
--     AccReportingYear, PostmarkDate, PostmarkedOnTime, SignedByRo, OnCorrectForms, IncludesAllTvConditions,
--     CorrectlyCompleted, ReportsDeviations, IncludesPreviouslyUnreportedDeviations, ReportsAllKnownDeviations,
--     ResubmittalRequired,
-- 
--     -- AnnualComplianceCertification, Report
--     EnforcementNeeded,
-- 
--     -- Inspection
--     InspectionReason, InspectionStarted, InspectionEnded, WeatherConditions, InspectionGuide, FacilityOperating,
--     DeviationsNoted,
-- 
--     -- Inspection, Notification, PermitRevocation, SourceTestReview
--     FollowupTaken,
-- 
--     -- Report
--     ReportingPeriodType, ReportingPeriodStart, ReportingPeriodEnd, ReportingPeriodComment,
-- 
--     -- Notification, Report, SourceTestReview
--     DueDate,
-- 
--     -- Notification, Report
--     SentDate,
-- 
--     -- Report
--     ReportComplete,
-- 
--     -- SourceTestReview
--     ReferenceNumber, ReceivedByComplianceDate,
-- 
--     -- Notification
--     NotificationTypeId,
-- 
--     -- PermitRevocation
--     PermitRevocationDate, PhysicalShutdownDate,
-- 
--     -- WorkEntry
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--     ClosedById, ClosedDate)
