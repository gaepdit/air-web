begin

    declare @otherType uniqueidentifier =
        (select Id
         from AirWeb.dbo.Lookups
         where Discriminator = 'NotificationType'
           and Name = 'Other');

    --
--     SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork ON;
--
--     insert into AirWeb.dbo.ComplianceWork
--     (
--         -- WorkEntry
--         Id, FacilityId, WorkEntryType, ResponsibleStaffId, AcknowledgmentLetterDate, Notes, EventDate,
--         IsComplianceEvent,
--
--         -- AnnualComplianceCertification, Notification, Report
--         ReceivedDate,
--
--         -- Inspection, Notification, PermitRevocation, SourceTestReview
--         FollowupTaken,
--
--         -- Notification, Report, SourceTestReview
--         DueDate,
--
--         -- Notification, Report
--         SentDate,
--
--         -- Notification
--         NotificationTypeId,
--
--         -- WorkEntry
--         CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById, DeleteComments, IsClosed,
--         ClosedById, ClosedDate)

    select i.STRTRACKINGNUMBER                                                         as Id,
           AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                             as FacilityId,
           'Notification'                                                              as WorkEntryType,
           ur.Id                                                                       as ResponsibleStaffId,
           convert(date, i.DATACKNOLEDGMENTLETTERSENT)                                 as AcknowledgmentLetterDate,

           -- "Other"-type notifications include an additional free text field.
           nullif(concat_ws(': ' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                            IIF(d.STRNOTIFICATIONTYPE = '01',
                                AIRBRANCH.air.ReduceText(d.STRNOTIFICATIONTYPEOTHER), null),
                            + AIRBRANCH.air.ReduceText(d.STRNOTIFICATIONCOMMENT)), '') as Notes,
           convert(date, i.DATRECEIVEDDATE)                                            as EventDate,
           0                                                                           as IsComplianceEvent,
           convert(date, i.DATRECEIVEDDATE)                                            as ReceivedDate,

           convert(bit, d.STRNOTIFICATIONFOLLOWUP)                                     as FollowupTaken,
           AIRBRANCH.air.FixDate(d.DATNOTIFICATIONDUE)                                 as DueDate,
           AIRBRANCH.air.FixDate(d.DATNOTIFICATIONSENT)                                as SentDate,
           isnull(lw.Id, @otherType)                                                   as NotificationTypeId,

           i.DATMODIFINGDATE at time zone 'Eastern Standard Time'                      as CreatedAt,
           uc.Id                                                                       as CreatedById,
           d.DATMODIFINGDATE at time zone 'Eastern Standard Time'                      as UpdatedAt,
           um.Id                                                                       as UpdatedById,
           convert(bit, isnull(i.STRDELETE, 'False'))                                  as IsDeleted,
           null                                                                        as DeletedAt,
           null                                                                        as DeletedById,
           null                                                                        as DeleteComments,
           IIF(i.DATCOMPLETEDATE is null, 0, 1)                                        as IsClosed,
           IIF(i.DATCOMPLETEDATE is null, null, um.Id)                                 as ClosedById,
           convert(date, i.DATCOMPLETEDATE)                                            as ClosedDate

    from AIRBRANCH.dbo.SSCPITEMMASTER i
        left join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
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
        left join AirWeb.dbo.AspNetUsers um
            on um.IaipUserId = d.STRMODIFINGPERSON

    where i.STRDELETE is null
      and i.STREVENTTYPE = '05'
      -- Notification type 03 - "Permit Revocation" - is excluded here and migrated separately as its own event type.
      and (d.STRNOTIFICATIONTYPE <> '03' or d.STRNOTIFICATIONTYPE is null)

    order by i.STRTRACKINGNUMBER;

    SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

end

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'Notification';
