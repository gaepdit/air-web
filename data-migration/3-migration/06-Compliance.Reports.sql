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
--     -- AnnualComplianceCertification, Report
--     ReportsDeviations, EnforcementNeeded,
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
--     -- WorkEntry
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--     ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                              as Id,
       AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                  as FacilityId,
       'Report'                                                         as WorkEntryType,
       ur.Id                                                            as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)                      as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRGENERALCOMMENTS)                   as Notes,
       convert(date, i.DATRECEIVEDDATE)                                 as EventDate,
       convert(bit, 1)                                                  as IsComplianceEvent,
       i.ICIS_STATUSIND                                                 as DataExchangeStatus,

       convert(date, i.DATRECEIVEDDATE)                                 as ReceivedDate,
       convert(bit, STRSHOWDEVIATION)                                   as ReportsDeviations,
       convert(bit, STRENFORCEMENTNEEDED)                               as EnforcementNeeded,
       case
           when STRREPORTPERIOD = N'Monthly' then N'Monthly'
           when STRREPORTPERIOD = N'First Quarter' then N'FirstQuarter'
           when STRREPORTPERIOD = N'Second Quarter' then N'SecondQuarter'
           when STRREPORTPERIOD = N'Third Quarter' then N'ThirdQuarter'
           when STRREPORTPERIOD in (N'Forth Quarter', N'Fourth Quarter') then N'FourthQuarter'
           when STRREPORTPERIOD = N'First Semiannual' then N'FirstSemiannual'
           when STRREPORTPERIOD = N'Second Semiannual' then N'SecondSemiannual'
           when STRREPORTPERIOD = N'Annual' then N'Annual'
           when STRREPORTPERIOD in (N'Malfunction/Deviation', N'Malfunction', N'6.1.2')
               then N'MalfunctionDeviation'
           else N'Other'
           end                                                          as ReportingPeriodType,
       convert(date, DATREPORTINGPERIODSTART)                           as ReportingPeriodStart,
       convert(date, DATREPORTINGPERIODEND)                             as ReportingPeriodEnd,
       AIRBRANCH.air.ReduceText(d.STRREPORTINGPERIODCOMMENTS)           as ReportingPeriodComment,
       convert(date, d.DATREPORTDUEDATE)                                as DueDate,
       convert(date, d.DATSENTBYFACILITYDATE)                           as SentDate,
       convert(bit, STRCOMPLETESTATUS)                                  as ReportComplete,

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
    inner join AIRBRANCH.dbo.SSCPREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirBranchUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.AirBranchUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirBranchUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '01'

order by i.STRTRACKINGNUMBER;

SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'Report';
