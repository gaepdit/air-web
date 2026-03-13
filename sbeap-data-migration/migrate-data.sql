use AirWeb

-- === Lookups: Agency

insert into AirWeb.dbo.Lookups
(Id, Name, Active, Discriminator, CreatedAt, CreatedById, UpdatedAt, UpdatedById)
select Id,
       Name,
       Active,
       'Agency' as Discriminator,
       CreatedAt,
       CreatedById,
       UpdatedAt,
       UpdatedById
from Sbeap.dbo.Agencies;

-- === Lookups: ActionItemType

insert into AirWeb.dbo.Lookups
(Id, Name, Active, Discriminator, CreatedAt, CreatedById, UpdatedAt, UpdatedById)
select Id,
       Name,
       Active,
       'ActionItemType' as Discriminator,
       CreatedAt,
       CreatedById,
       UpdatedAt,
       UpdatedById
from Sbeap.dbo.ActionItemTypes;

-- === Customers

insert into AirWeb.dbo.SbeapCustomers
(Id, Name, Description, SicCode, County, Website, Location_Street, Location_Street2, Location_City,
 Location_State, Location_PostalCode, MailingAddress_Street, MailingAddress_Street2, MailingAddress_City,
 MailingAddress_State, MailingAddress_PostalCode, DeleteComments, CreatedAt, CreatedById, UpdatedAt,
 UpdatedById, IsDeleted, DeletedAt, DeletedById)
select Id,
       Name,
       Description,
       SicCode,
       County,
       Website,
       isnull(Location_Street, '')           as Location_Street,
       Location_Street2,
       isnull(Location_City, '')             as Location_City,
       isnull(Location_State, '')            as Location_State,
       isnull(Location_PostalCode, '')       as Location_PostalCode,
       isnull(MailingAddress_Street, '')     as MailingAddress_Street,
       MailingAddress_Street2,
       isnull(MailingAddress_City, '')       as MailingAddress_City,
       isnull(MailingAddress_State, '')      as MailingAddress_State,
       isnull(MailingAddress_PostalCode, '') as MailingAddress_PostalCode,
       DeleteComments,
       CreatedAt,
       CreatedById,
       UpdatedAt,
       UpdatedById,
       IsDeleted,
       DeletedAt,
       DeletedById
from Sbeap.dbo.Customers;

-- === Contacts

insert into AirWeb.dbo.SbeapContacts
(Id, CustomerId, EnteredById, EnteredOn, Honorific, GivenName, FamilyName, Title, Email, Notes,
 Address_Street, Address_Street2, Address_City, Address_State, Address_PostalCode, CreatedAt, CreatedById,
 UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById)
select Id,
       CustomerId,
       IIF(EnteredById = N'648803ee-8b11-487b-b818-674ef62db7bd',
           N'DA50A9D8-57D3-4EEB-A849-9F2F98D6D554',
           EnteredById)               as EnteredById,
       EnteredOn,
       Honorific,
       GivenName,
       FamilyName,
       Title,
       Email,
       Notes,
       isnull(Address_Street, '')     as Address_Street,
       Address_Street2,
       isnull(Address_City, '')       as Address_City,
       isnull(Address_State, '')      as Address_State,
       isnull(Address_PostalCode, '') as Address_PostalCode,
       CreatedAt,
       CreatedById,
       UpdatedAt,
       UpdatedById,
       IsDeleted,
       DeletedAt,
       DeletedById
from Sbeap.dbo.Contacts;

-- === Cases

insert into AirWeb.dbo.SbeapCases
(Id, CustomerId, Description, CaseOpenedDate, CaseClosedDate, CaseClosureNotes, ReferralAgencyId,
 ReferralDate, ReferralNotes, DeleteComments, CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted,
 DeletedAt, DeletedById)
select Id,
       CustomerId,
       Description,
       CaseOpenedDate,
       CaseClosedDate,
       CaseClosureNotes,
       ReferralAgencyId,
       ReferralDate,
       ReferralNotes,
       DeleteComments,
       CreatedAt,
       CreatedById,
       UpdatedAt,
       UpdatedById,
       IsDeleted,
       DeletedAt,
       DeletedById
from Sbeap.dbo.Cases;

-- === ActionItems

insert into AirWeb.dbo.SbeapActionItems
(Id, CaseworkId, ActionItemTypeId, ActionDate, Notes, EnteredById, EnteredOn, CreatedAt, CreatedById,
 UpdatedAt, UpdatedById, IsDeleted, DeletedAt, DeletedById)
select Id,
       CaseworkId,
       ActionItemTypeId,
       ActionDate,
       Notes,
       IIF(EnteredById = N'648803ee-8b11-487b-b818-674ef62db7bd',
           N'DA50A9D8-57D3-4EEB-A849-9F2F98D6D554',
           EnteredById)               as EnteredById,
       EnteredOn,
       CreatedAt,
       CreatedById,
       UpdatedAt,
       UpdatedById,
       IsDeleted,
       DeletedAt,
       DeletedById
from Sbeap.dbo.ActionItems;
