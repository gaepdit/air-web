SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;

insert into AirWeb.dbo.ComplianceWork
(
    -- WorkEntry
    Id, FacilityId, ComplianceWorkType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate,
    IsComplianceEvent,

    -- AnnualComplianceCertification, Notification, PermitRevocation, Report
    ReceivedDate,

    -- Inspection, Notification, PermitRevocation, SourceTestReview
    FollowupTaken,

    -- PermitRevocation
    PermitRevocationDate, PhysicalShutdownDate,

    -- WorkEntry
    CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, IsClosed, ClosedById, ClosedDate)

select i.STRTRACKINGNUMBER                                       as Id,
       AIRBRANCH.iaip_facility.FormatAirsNumber(i.STRAIRSNUMBER) as FacilityId,
       'PermitRevocation'                                        as WorkEntryType,
       ur.Id                                                     as ResponsibleStaffId,
       convert(date, i.DATACKNOLEDGMENTLETTERSENT)               as AcknowledgmentLetterDate,
       AIRBRANCH.air.ReduceText(d.STRNOTIFICATIONCOMMENT)        as Notes,
       convert(date, i.DATRECEIVEDDATE)                          as EventDate,
       0                                                         as IsComplianceEvent,

       convert(date, i.DATRECEIVEDDATE)                          as ReceivedDate,
       convert(bit, d.STRNOTIFICATIONFOLLOWUP)                   as FollowupTaken,

       AIRBRANCH.air.FixDate(d.DATNOTIFICATIONDUE)               as PermitRevocationDate,
       convert(date, d.DATNOTIFICATIONSENT)                      as PhysicalShutdownDate,

       i.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as CreatedAt,
       uc.Id                                                     as CreatedById,
       d.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       0                                                         as IsDeleted,
       IIF(i.DATCOMPLETEDATE is null, 0, 1)                      as IsClosed,
       IIF(i.DATCOMPLETEDATE is null, null, um.Id)               as ClosedById,
       convert(date, i.DATCOMPLETEDATE)                          as ClosedDate

from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER

    left join AIRBRANCH.dbo.LOOKUPSSCPNOTIFICATIONS li
        on li.STRNOTIFICATIONKEY = d.STRNOTIFICATIONTYPE
    left join AirWeb.dbo.Lookups lw
        on lw.Name = li.STRNOTIFICATIONDESC
        and lw.Discriminator = 'NotificationType'

    inner join AirWeb.dbo.AspNetUsers ur
        on ur.IaipUserId = i.STRRESPONSIBLESTAFF
    inner join AirWeb.dbo.AspNetUsers uc
        on uc.IaipUserId = i.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '05'
  and d.STRNOTIFICATIONTYPE = '03'

order by i.STRTRACKINGNUMBER;

SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

select *
from AirWeb.dbo.ComplianceWork
where ComplianceWorkType = 'PermitRevocation';
