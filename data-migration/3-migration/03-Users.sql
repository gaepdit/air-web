-- insert into AirWeb.dbo.AspNetUsers
-- (Id, GivenName, FamilyName, OfficeId, Active, AccountCreatedAt, AccountUpdatedAt, ProfileUpdatedAt, MostRecentLogin,
--  UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
--  PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, AirbranchUserId)

select newid()                                               as Id,
       iif(u.NUMUSERID in (0, 1), 'Unknown', u.STRFIRSTNAME) as GivenName,
       iif(u.NUMUSERID in (0, 1), 'User', u.STRLASTNAME)     as FamilyName,
       u.NUMUNIT,
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
       null                                                  as PhoneNumber,
       0                                                     as PhoneNumberConfirmed,
       0                                                     as TwoFactorEnabled,
       null                                                  as LockoutEnd,
       1                                                     as LockoutEnabled,
       0                                                     as AccessFailedCount,
       u.NUMUSERID                                           as AirbranchUserId
from AIRBRANCH.dbo.EPDUSERPROFILES u
    inner join AIRBRANCH.air.ComplianceUserIds c
        on c.UserId = u.NUMUSERID
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
                else N'Other'
                end
where NUMEMPLOYEESTATUS = 1
  and l.id is null;

select *
from AirWeb.dbo.AspNetUsers;
