use AirWeb
go

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;
-- 
-- insert into AirWeb.dbo.ComplianceWork
-- (
--     -- WorkEntry
--     Id, FacilityId, WorkEntryType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate,
--     IsComplianceEvent,
-- 
--     -- AnnualComplianceCertification, Notification, PermitRevocation, Report
--     ReceivedDate,
-- 
--     -- Inspection, Notification, PermitRevocation, SourceTestReview
--     FollowupTaken,
-- 
--     -- PermitRevocation
--     PermitRevocationDate, PhysicalShutdownDate,
-- 
--     -- WorkEntry
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--     ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                              as Id,
       AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                  as FacilityId,
       'PermitRevocation'                                               as WorkEntryType,
       ur.Id                                                            as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)                      as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRNOTIFICATIONCOMMENT)               as Notes,
       convert(date, i.DATRECEIVEDDATE)                                 as EventDate,
       convert(bit, 0)                                                  as IsComplianceEvent,

       convert(date, i.DATRECEIVEDDATE)                                 as ReceivedDate,
       convert(bit, d.STRNOTIFICATIONFOLLOWUP)                          as FollowupTaken,

       AIRBRANCH.air.FixDate(d.DATNOTIFICATIONDUE)                      as PermitRevocationDate,
       convert(date, d.DATNOTIFICATIONSENT)                             as PhysicalShutdownDate,

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
    inner join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
        and d.STRNOTIFICATIONTYPE = '03'

    left join AIRBRANCH.dbo.LOOKUPSSCPNOTIFICATIONS li
        on li.STRNOTIFICATIONKEY = d.STRNOTIFICATIONTYPE
    left join AirWeb.dbo.Lookups lw
        on lw.Name = li.STRNOTIFICATIONDESC
        and lw.Discriminator = 'NotificationType'

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.AirbranchUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.AirbranchUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '05'

order by i.STRTRACKINGNUMBER;

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'PermitRevocation';
