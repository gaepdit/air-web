-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;
-- 
-- insert into AirWeb.dbo.ComplianceWork
-- (
--     -- WorkEntry
--     Id, FacilityId, ComplianceWorkType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate,
--     IsComplianceEvent,
-- 
--     -- ComplianceEvent
--     ActionNumber, DataExchangeStatus, DataExchangeStatusDate,
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
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, IsClosed, ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                       as Id,
       AIRBRANCH.iaip_facility.FormatAirsNumber(i.STRAIRSNUMBER) as FacilityId,
       'Report'                                                  as WorkEntryType,
       ur.Id                                                     as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)               as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRGENERALCOMMENTS)            as Notes,
       convert(date, i.DATRECEIVEDDATE)                          as EventDate,
       1                                                         as IsComplianceEvent,

       convert(int, f.STRAFSACTIONNUMBER)                        as ActionNumber,
       i.ICIS_STATUSIND                                          as DataExchangeStatus,
       null                                                      as DataExchangeStatusDate,

       convert(date, i.DATRECEIVEDDATE)                          as ReceivedDate,
       convert(bit, d.STRSHOWDEVIATION)                          as ReportsDeviations,
       convert(bit, d.STRENFORCEMENTNEEDED)                      as EnforcementNeeded,
       case
           when d.STRREPORTPERIOD = N'Monthly' then N'Monthly'
           when d.STRREPORTPERIOD = N'First Quarter' then N'FirstQuarter'
           when d.STRREPORTPERIOD = N'Second Quarter' then N'SecondQuarter'
           when d.STRREPORTPERIOD = N'Third Quarter' then N'ThirdQuarter'
           when d.STRREPORTPERIOD in (N'Forth Quarter', N'Fourth Quarter') then N'FourthQuarter'
           when d.STRREPORTPERIOD = N'First Semiannual' then N'FirstSemiannual'
           when d.STRREPORTPERIOD = N'Second Semiannual' then N'SecondSemiannual'
           when d.STRREPORTPERIOD = N'Annual' then N'Annual'
           when d.STRREPORTPERIOD in (N'Malfunction/Deviation', N'Malfunction', N'6.1.2')
               then N'MalfunctionDeviation'
           else N'Other'
           end                                                   as ReportingPeriodType,
       convert(date, d.DATREPORTINGPERIODSTART)                  as ReportingPeriodStart,
       convert(date, d.DATREPORTINGPERIODEND)                    as ReportingPeriodEnd,
       AIRBRANCH.air.ReduceText(d.STRREPORTINGPERIODCOMMENTS)    as ReportingPeriodComment,
       convert(date, d.DATREPORTDUEDATE)                         as DueDate,
       convert(date, d.DATSENTBYFACILITYDATE)                    as SentDate,
       convert(bit, d.STRCOMPLETESTATUS)                         as ReportComplete,

       i.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as CreatedAt,
       uc.Id                                                     as CreatedById,
       d.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       0                                                         as IsDeleted,
       IIF(i.DATCOMPLETEDATE is null, 0, 1)                      as IsClosed,
       IIF(i.DATCOMPLETEDATE is null, null, um.Id)               as ClosedById,
       convert(date, i.DATCOMPLETEDATE)                          as ClosedDate

from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.SSCPREPORTS d
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
  and i.STREVENTTYPE = '01'

order by i.STRTRACKINGNUMBER;

SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where ComplianceWorkType = 'Report';
