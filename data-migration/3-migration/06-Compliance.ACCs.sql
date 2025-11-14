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
--     -- WorkEntry
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--     ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                              as Id,
       AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                  as FacilityId,
       'AnnualComplianceCertification'                                  as WorkEntryType,
       ur.Id                                                            as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)                      as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRCOMMENTS)                          as Notes,
       convert(date, i.DATRECEIVEDDATE)                                 as EventDate,
       convert(bit, 1)                                                  as IsComplianceEvent,
       i.ICIS_STATUSIND                                                 as DataExchangeStatus,
       convert(date, i.DATRECEIVEDDATE)                                 as ReceivedDate,

       year(d.DATACCREPORTINGYEAR)                                      as AccReportingYear,
       convert(date, d.DATPOSTMARKDATE)                                 as PostmarkDate,
       convert(bit, d.STRPOSTMARKEDONTIME)                              as PostmarkedOnTime,
       convert(bit, d.STRSIGNEDBYRO)                                    as SignedByRo,
       convert(bit, d.STRCORRECTACCFORMS)                               as OnCorrectForms,
       convert(bit, d.STRTITLEVCONDITIONSLISTED)                        as IncludesAllTvConditions,
       convert(bit, d.STRACCCORRECTLYFILLEDOUT)                         as CorrectlyCompleted,
       convert(bit, d.STRREPORTEDDEVIATIONS)                            as ReportsDeviations,
       convert(bit, d.STRDEVIATIONSUNREPORTED)                          as IncludesPreviouslyUnreportedDeviations,
       convert(bit, d.STRKNOWNDEVIATIONSREPORTED)                       as ReportsAllKnownDeviations,
       convert(bit, d.STRRESUBMITTALREQUIRED)                           as ResubmittalRequired,
       convert(bit, d.STRENFORCEMENTNEEDED)                             as EnforcementNeeded,

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
    left join AIRBRANCH.dbo.SSCPACCS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirbranchUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.AirbranchUserId = i.STRMODIFINGPERSON
    left join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '04';

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'AnnualComplianceCertification';
