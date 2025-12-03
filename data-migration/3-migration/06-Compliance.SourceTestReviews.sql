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
--     -- Inspection, Notification, PermitRevocation, SourceTestReview
--     FollowupTaken,
-- 
--     -- Notification, Report, SourceTestReview
--     DueDate,
-- 
--     -- SourceTestReview
--     ReferenceNumber, ReceivedByComplianceDate,
-- 
--     -- WorkEntry
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--     ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                              as Id,
       AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                  as FacilityId,
       'SourceTestReview'                                               as WorkEntryType,
       ur.Id                                                            as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)                      as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRTESTREPORTCOMMENTS)                as Notes,
       convert(date, i.DATRECEIVEDDATE)                                 as EventDate,
       convert(bit, 1)                                                  as IsComplianceEvent,
       i.ICIS_STATUSIND                                                 as DataExchangeStatus,

       convert(bit, d.STRTESTREPORTFOLLOWUP)                            as FollowupTaken,
       AIRBRANCH.air.FixDate(d.DATTESTREPORTDUE)                        as DueDate,
       nullif(d.STRREFERENCENUMBER, 'N/A')                              as ReferenceNumber,
       convert(date, i.DATRECEIVEDDATE)                                 as ReceivedByComplianceDate,

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
    inner join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirBranchUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.AirBranchUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirBranchUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '03'

order by i.STRTRACKINGNUMBER;

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'SourceTestReview';
