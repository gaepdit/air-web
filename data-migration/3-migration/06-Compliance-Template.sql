use AirWeb
go

-- insert into AirWeb.dbo.ComplianceWork
-- (
--     -- WorkEntry
--     Id, FacilityId, WorkEntryType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate, IsComplianceEvent,
-- 
--     -- ComplianceEvent
--     DataExchangeStatus,
-- 
--     -- AnnualComplianceCertification, Notification, PermitRevocation, Report
--     ReceivedDate,
-- 
--     -- AnnualComplianceCertification
--     AccReportingYear, PostmarkDate, PostmarkedOnTime, SignedByRo, OnCorrectForms, IncludesAllTvConditions,
--     CorrectlyCompleted, IncludesPreviouslyUnreportedDeviations, ReportsAllKnownDeviations,
--     ResubmittalRequired,
-- 
--     -- AnnualComplianceCertification, Report
--     ReportsDeviations, EnforcementNeeded,
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

-- AnnualComplianceCertification, Notification, PermitRevocation, Report
ReceivedDate,

-- AnnualComplianceCertification
AccReportingYear,
PostmarkDate,
PostmarkedOnTime,
SignedByRo,
OnCorrectForms,
IncludesAllTvConditions,
CorrectlyCompleted,
IncludesPreviouslyUnreportedDeviations,
ReportsAllKnownDeviations,
ResubmittalRequired,

-- AnnualComplianceCertification, Report
ReportsDeviations,
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

