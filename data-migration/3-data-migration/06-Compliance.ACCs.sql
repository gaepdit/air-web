SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;

insert into AirWeb.dbo.ComplianceWork
(
    -- WorkEntry
    Id, FacilityId, ComplianceWorkType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate,
    IsComplianceEvent,

    -- ComplianceEvent
    ActionNumber, DataExchangeStatus, DataExchangeStatusDate,

    -- AnnualComplianceCertification, Notification, Report
    ReceivedDate,

    -- AnnualComplianceCertification
    AccReportingYear, PostmarkDate, PostmarkedOnTime, SignedByRo, OnCorrectForms, IncludesAllTvConditions,
    CorrectlyCompleted, ReportsDeviations, IncludesPreviouslyUnreportedDeviations, ReportsAllKnownDeviations,
    ResubmittalRequired,

    -- AnnualComplianceCertification, Report
    EnforcementNeeded,

    -- WorkEntry
    CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, IsClosed, ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                       as Id,
       AIRBRANCH.iaip_facility.FormatAirsNumber(i.STRAIRSNUMBER) as FacilityId,
       'AnnualComplianceCertification'                           as WorkEntryType,
       ur.Id                                                     as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)               as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRCOMMENTS)                   as Notes,
       convert(date, i.DATRECEIVEDDATE)                          as EventDate,
       1                                                         as IsComplianceEvent,

       convert(int, f.STRAFSACTIONNUMBER)                        as ActionNumber,
       iif(f.STRAFSACTIONNUMBER is null, 'N', i.ICIS_STATUSIND)  as DataExchangeStatus,
       null                                                      as DataExchangeStatusDate,

       convert(date, i.DATRECEIVEDDATE)                          as ReceivedDate,
       year(d.DATACCREPORTINGYEAR)                               as AccReportingYear,
       convert(date, d.DATPOSTMARKDATE)                          as PostmarkDate,
       convert(bit, d.STRPOSTMARKEDONTIME)                       as PostmarkedOnTime,
       convert(bit, d.STRSIGNEDBYRO)                             as SignedByRo,
       convert(bit, d.STRCORRECTACCFORMS)                        as OnCorrectForms,
       convert(bit, d.STRTITLEVCONDITIONSLISTED)                 as IncludesAllTvConditions,
       convert(bit, d.STRACCCORRECTLYFILLEDOUT)                  as CorrectlyCompleted,
       convert(bit, d.STRREPORTEDDEVIATIONS)                     as ReportsDeviations,
       convert(bit, d.STRDEVIATIONSUNREPORTED)                   as IncludesPreviouslyUnreportedDeviations,
       convert(bit, d.STRKNOWNDEVIATIONSREPORTED)                as ReportsAllKnownDeviations,
       convert(bit, d.STRRESUBMITTALREQUIRED)                    as ResubmittalRequired,
       convert(bit, d.STRENFORCEMENTNEEDED)                      as EnforcementNeeded,

       i.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as CreatedAt,
       uc.Id                                                     as CreatedById,
       d.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       0                                                         as IsDeleted,
       IIF(i.DATCOMPLETEDATE is null, 0, 1)                      as IsClosed,
       IIF(i.DATCOMPLETEDATE is null, null, um.Id)               as ClosedById,
       convert(date, i.DATCOMPLETEDATE)                          as ClosedDate

from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPACCS d
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
  and i.STREVENTTYPE = '04'

order by i.STRTRACKINGNUMBER;

SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where ComplianceWorkType = 'AnnualComplianceCertification';
