use AirWeb
go

begin

    declare @otherType uniqueidentifier =
        (select Id
         from AirWeb.dbo.Lookups
         where Discriminator = 'NotificationType'
           and Name = 'Other');

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

    select i.STRTRACKINGNUMBER                                              as Id,
           AIRBRANCH.air.FormatAirsNumber(i.STRAIRSNUMBER)                  as FacilityId,
           'Notification'                                                   as WorkEntryType,
           ur.Id                                                            as ResponsibleStaffId,
           convert(date, i.DATACKNOLEDGMENTLETTERSENT)                      as AcknowledgmentLetterDate,

           -- "Other" type notifications include an additional free text field.
           concat_ws(': ' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                     IIF(d.STRNOTIFICATIONTYPE = '01',
                         nullif(nullif(d.STRNOTIFICATIONTYPEOTHER, 'N/A'), ''),
                         null),
                     + nullif(nullif(d.STRNOTIFICATIONCOMMENT, 'N/A'), '')) as Notes,
           convert(date, i.DATRECEIVEDDATE)                                 as EventDate,
           convert(bit, 0)                                                  as IsComplianceEvent,
           convert(date, i.DATRECEIVEDDATE)                                 as ReceivedDate,

           convert(bit, d.STRNOTIFICATIONFOLLOWUP)                          as FollowupTaken,

           -- Due date and sent date behavior changed multiple times over the years.
           --
           -- * Between 2008-09-11 and 2024-10-22, a flag (STRNOTIFICATIONDUE, STRNOTIFICATIONSENT)
           --   was set to 'False' if the date (DATNOTIFICATIONDUE, DATNOTIFICATIONSENT) is NOT null,
           --   while 'True' indicated the date IS null!
           --
           -- * Starting 2024-10-23 (based on d.DATMODIFINGDATE), the flags were always (mistakenly) set
           --   to 'True', but date nullability can still be assumed to be correct. I plan to fix the
           --   data before final migration.
           --
           -- * Before 2008-09-11, a placeholder date '1776-07-04' was used instead of null. During
           --   this period, the flag values were inconsistently set when non-null dates were used
           --   and so are unreliable. Therefore, we will have to make an assumption one way or the
           --   other whether to use the date values. To me, the data look valid, so I plan to keep
           --   them and ignore the flags.
           --
           -- Given all of the above, it's simplest to just use the dates (replacing the null
           -- placeholder value) and ignore the flags.

           nullif(d.DATNOTIFICATIONDUE, '1776-07-04')                       as DueDate,
           nullif(d.DATNOTIFICATIONSENT, '1776-07-04')                      as SentDate,
           isnull(lw.Id, @otherType)                                        as NotificationTypeId,

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
            -- Notification type 03-Permit Revocation is excluded because it is migrated separately.
            -- (Notification types 04 & 05 don't exist.)
            -- TODO: Consider also excluding 07-Malfunction and 08-Deviation here and migrating them as Reports instead.
            -- See https://github.com/gaepdit/air-web/issues/32#issuecomment-3378116272
            and d.STRNOTIFICATIONTYPE not in ('03', '04', '05')

        left join AIRBRANCH.dbo.LOOKUPSSCPNOTIFICATIONS li
            on li.STRNOTIFICATIONKEY = d.STRNOTIFICATIONTYPE
        left join AirWeb.dbo.Lookups lw
            on lw.Name = li.STRNOTIFICATIONDESC

        inner join AirWeb.dbo.AspNetUsers ur
            on ur.AirbranchUserId = i.STRRESPONSIBLESTAFF
        inner join AirWeb.dbo.AspNetUsers uc
            on uc.AirbranchUserId = i.STRMODIFINGPERSON
        inner join AirWeb.dbo.AspNetUsers um
            on um.AirbranchUserId = d.STRMODIFINGPERSON

    where i.STRDELETE is null
      and i.STREVENTTYPE = '05';

-- SET IDENTITY_INSERT AirWeb.dbo.ComplianceWork OFF;

end

select *
from AirWeb.dbo.ComplianceWork
where WorkEntryType = 'Notification';
