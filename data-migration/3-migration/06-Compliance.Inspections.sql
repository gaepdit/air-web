use AirWeb
go

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;
-- 
-- insert into AirWeb.dbo.ComplianceWork
-- (
--     -- WorkEntry
--     Id, FacilityId, WorkEntryType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate, IsComplianceEvent,
-- 
--     -- ComplianceEvent
--     DataExchangeStatus,
-- 
--     -- Inspection
--     InspectionReason, InspectionStarted, InspectionEnded, WeatherConditions, InspectionGuide, FacilityOperating,
--     DeviationsNoted,
-- 
--     -- Inspection, Notification, PermitRevocation, SourceTestReview
--     FollowupTaken,
-- 
--     -- WorkEntry
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--     ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                              as Id,
       AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                  as FacilityId,
       iif(i.STREVENTTYPE = '02', 'Inspection', 'RmpInspection')        as WorkEntryType,
       ur.Id                                                            as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)                      as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRINSPECTIONCOMMENTS)                as Notes,
       convert(date, d.DATINSPECTIONDATESTART)                          as EventDate,
       convert(bit, 1)                                                  as IsComplianceEvent,
       i.ICIS_STATUSIND                                                 as DataExchangeStatus,

       case
           when d.STRINSPECTIONREASON = 'Planned Unannounced' then 'PlannedUnannounced'
           when d.STRINSPECTIONREASON = 'Planned Announced' then 'PlannedAnnounced'
           when d.STRINSPECTIONREASON = 'Unplanned' then 'Unplanned'
           when d.STRINSPECTIONREASON = 'Complaint Investigation' then 'Complaint'
           when d.STRINSPECTIONREASON = 'Joint EPD/EPA' then 'JointEpdEpa'
           when d.STRINSPECTIONREASON = 'Multimedia' then 'Multimedia'
           when d.STRINSPECTIONREASON = 'Follow Up' then 'FollowUp'
           end                                                          as InspectionReason,
       d.DATINSPECTIONDATESTART                                         as InspectionStarted,
       d.DATINSPECTIONDATEEND                                           as InspectionEnded,
       AIRBRANCH.air.ReduceText(d.STRWEATHERCONDITIONS)                 as WeatherConditions,
       AIRBRANCH.air.ReduceText(d.STRINSPECTIONGUIDE)                   as InspectionGuide,
       convert(bit, d.STRFACILITYOPERATING)                             as FacilityOperating,
       iif(d.STRINSPECTIONCOMPLIANCESTATUS = 'Deviation(s) Noted',
           convert(bit, 1), convert(bit, 0))                            as DeviationsNoted,
       convert(bit, d.STRINSPECTIONFOLLOWUP)                            as FollowupTaken,

       i.DATMODIFINGDATE at time zone 'Eastern Standard Time'           as CreatedAt,
       uc.Id                                                            as CreatedById,
       d.DATMODIFINGDATE at time zone 'Eastern Standard Time'           as UpdatedAt,
       um.Id                                                            as UpdatedById,
       convert(bit, isnull(i.STRDELETE, 'False'))                       as IsDeleted,
       null                                                             as DeletedAt,
       null                                                             as DeletedById,
       null                                                             as DeleteComments,
       IIF(i.DATCOMPLETEDATE is null, convert(bit, 0), convert(bit, 1)) as IsClosed,
       IIF(i.DATCOMPLETEDATE is null, null, um.Id)                      as ClosedById,
       convert(date, i.DATCOMPLETEDATE)                                 as ClosedDate

from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPINSPECTIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirbranchUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.AirbranchUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE in ('02', '07')

order by i.STRTRACKINGNUMBER;

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType in ('Inspection', 'RmpInspection');
