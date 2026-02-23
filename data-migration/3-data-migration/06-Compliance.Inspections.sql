SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;

insert into AirWeb.dbo.ComplianceWork
(
    -- WorkEntry
    Id, FacilityId, ComplianceWorkType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate,
    IsComplianceEvent,

    -- ComplianceEvent
    ActionNumber, DataExchangeStatus, DataExchangeStatusDate,

    -- Inspection
    InspectionReason, InspectionStarted, InspectionEnded, WeatherConditions, InspectionGuide,
    FacilityOperating, DeviationsNoted,

    -- Inspection, Notification, PermitRevocation, SourceTestReview
    FollowupTaken,

    -- WorkEntry
    CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, IsClosed, ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                                               as Id,
       AIRBRANCH.iaip_facility.FormatAirsNumber(i.STRAIRSNUMBER)                         as FacilityId,
       iif(i.STREVENTTYPE = '02', 'Inspection', 'RmpInspection')                         as WorkEntryType,
       ur.Id                                                                             as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)                                       as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRINSPECTIONCOMMENTS)                                 as Notes,
       convert(date, isnull(d.DATINSPECTIONDATESTART, i.DATRECEIVEDDATE))                as EventDate,
       1                                                                                 as IsComplianceEvent,

       convert(int, f.STRAFSACTIONNUMBER)                                                as ActionNumber,
       iif(f.STRAFSACTIONNUMBER is null, 'N', i.ICIS_STATUSIND)                          as DataExchangeStatus,
       null                                                                              as DataExchangeStatusDate,

       case
           when d.STRINSPECTIONREASON = 'Planned Unannounced' then 'PlannedUnannounced'
           when d.STRINSPECTIONREASON = 'Planned Announced' then 'PlannedAnnounced'
           when d.STRINSPECTIONREASON = 'Unplanned' then 'Unplanned'
           when d.STRINSPECTIONREASON = 'Complaint Investigation' then 'Complaint'
           when d.STRINSPECTIONREASON = 'Joint EPD/EPA' then 'JointEpdEpa'
           when d.STRINSPECTIONREASON = 'Multimedia' then 'Multimedia'
           when d.STRINSPECTIONREASON = 'Follow Up' then 'FollowUp'
           else 'PlannedAnnounced'
           end                                                                           as InspectionReason,
       isnull(d.DATINSPECTIONDATESTART, i.DATRECEIVEDDATE)                               as InspectionStarted,
       isnull(d.DATINSPECTIONDATEEND, i.DATRECEIVEDDATE)                                 as InspectionEnded,
       AIRBRANCH.air.ReduceText(d.STRWEATHERCONDITIONS)                                  as WeatherConditions,
       AIRBRANCH.air.ReduceText(d.STRINSPECTIONGUIDE)                                    as InspectionGuide,
       convert(bit, isnull(d.STRFACILITYOPERATING, 1))                                   as FacilityOperating,
       iif(d.STRINSPECTIONCOMPLIANCESTATUS = 'Deviation(s) Noted', 1, 0)                 as DeviationsNoted,
       convert(bit, isnull(d.STRINSPECTIONFOLLOWUP, 0))                                  as FollowupTaken,

       i.DATMODIFINGDATE at time zone 'Eastern Standard Time'                            as CreatedAt,
       uc.Id                                                                             as CreatedById,
       isnull(d.DATMODIFINGDATE, i.DATMODIFINGDATE) at time zone 'Eastern Standard Time' as UpdatedAt,
       um.Id                                                                             as UpdatedById,
       0                                                                                 as IsDeleted,
       IIF(i.DATCOMPLETEDATE is null, 0, 1)                                              as IsClosed,
       IIF(i.DATCOMPLETEDATE is null, null, um.Id)                                       as ClosedById,
       convert(date, i.DATCOMPLETEDATE)                                                  as ClosedDate

from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPINSPECTIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    left join AFSSSCPRECORDS f
        on f.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.IaipUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.IaipUserId = i.STRMODIFINGPERSON
    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE in ('02', '07')

order by i.STRTRACKINGNUMBER;

SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where ComplianceWorkType in ('Inspection', 'RmpInspection');
