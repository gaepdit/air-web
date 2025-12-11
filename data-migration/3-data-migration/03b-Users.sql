-- insert into AirWeb.dbo.AspNetUsers
-- (Id, GivenName, FamilyName, OfficeId, Active, AccountCreatedAt, AccountUpdatedAt, ProfileUpdatedAt, MostRecentLogin,
--  UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
--  PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, IaipUserId)

select newid()                                               as Id,
       iif(u.NUMUSERID in (0, 1), 'Unknown', u.STRFIRSTNAME) as GivenName,
       iif(u.NUMUSERID in (0, 1), 'User', u.STRLASTNAME)     as FamilyName,
       l.Id                                                  as OfficeId,
       u.NUMEMPLOYEESTATUS                                   as Active,
       sysdatetimeoffset()                                   as AccountCreatedAt,
       sysdatetimeoffset()                                   as AccountUpdatedAt,
       null                                                  as ProfileUpdatedAt,
       null                                                  as MostRecentLogin,
       lower(u.STREMAILADDRESS)                              as UserName,
       upper(u.STREMAILADDRESS)                              as NormalizedUserName,
       u.STREMAILADDRESS                                     as Email,
       upper(u.STREMAILADDRESS)                              as NormalizedEmail,
       0                                                     as EmailConfirmed,
       null                                                  as PasswordHash,
       newid()                                               as SecurityStamp,
       newid()                                               as ConcurrencyStamp,
       IIF(u.NUMEMPLOYEESTATUS = 1 and len(u.STRPHONE) = 10 and right(u.STRPHONE, 7) <> '5551212',
           concat_ws('-', left(u.STRPHONE, 3), substring(u.STRPHONE, 4, 3), right(u.STRPHONE, 4)),
           null)                                             as PhoneNumber,
       0                                                     as PhoneNumberConfirmed,
       0                                                     as TwoFactorEnabled,
       null                                                  as LockoutEnd,
       1                                                     as LockoutEnabled,
       0                                                     as AccessFailedCount,
       u.NUMUSERID                                           as IaipUserId
from AIRBRANCH.dbo.EPDUSERPROFILES u
    inner join AIRBRANCH.air.ComplianceUserIds c
        on c.IaipUserId = u.NUMUSERID
    left join AirWeb.dbo.Lookups l
        on l.Discriminator = 'Office'
        and l.Name =
            case
                when u.NUMUNIT = 18 then N'APB Permitting Program: Chemical Permitting'
                when u.NUMUNIT = 19 then N'APB Permitting Program: Combustion Permitting'
                when u.NUMUNIT = 20 then N'APB Permitting Program: Minerals Permitting'
                when u.NUMUNIT = 30 then N'APB Compliance Program: Air Toxics'
                when u.NUMUNIT = 31 then N'APB Compliance Program: Chemicals/Minerals'
                when u.NUMUNIT = 32 then N'APB Compliance Program: VOC/Combustion'
                when u.NUMUNIT = 33 then N'APB Permitting Program: NOx Permitting'
                when u.NUMUNIT = 34 then N'APB Permitting Program: VOC Permitting'
                when u.NUMUNIT = 37 then N'Coastal District (Brunswick)'
                when u.NUMUNIT = 38 then N'East Central District (Augusta)'
                when u.NUMUNIT = 39 then N'Mountain District (Atlanta)'
                when u.NUMUNIT = 40 then N'Mountain District (Cartersville)'
                when u.NUMUNIT = 41 then N'Northeast District (Athens)'
                when u.NUMUNIT = 42 then N'Southwest District (Albany)'
                when u.NUMUNIT = 43 then N'West Central District (Macon)'
                when u.NUMUNIT = 50 then N'APB Compliance Program: Source Monitoring Unit'
                when u.NUMUNIT = 14 then N'EPD-IT'
                when u.NUMPROGRAM = 3 then N'APB Compliance Program: Source Monitoring Unit'
                when u.NUMPROGRAM = 5 then N'APB Permitting Program'
                when u.NUMBRANCH = 1 then N'Air Protection Branch'
                else N'Other'
                end;

select *
from AirWeb.dbo.AspNetUsers;
