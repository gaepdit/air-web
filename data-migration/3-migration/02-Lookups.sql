use [air-web]
go

-- Notification Types
-- Original values from airbranch.dbo.LOOKUPSSCPNOTIFICATIONS
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Other', convert(bit, 1), 'NotificationType', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Startup', convert(bit, 1), 'NotificationType', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Response Letter', convert(bit, 1), 'NotificationType', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Malfunction', convert(bit, 1), 'NotificationType', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Deviation', convert(bit, 1), 'NotificationType', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Permit Revocation', convert(bit, 0), 'NotificationType', sysdatetimeoffset();

-- Offices
-- Original values modified from airbranch.dbo.LOOKUPEPDPROGRAMS and airbranch.dbo.LOOKUPEPDUNITS
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Air Protection Branch', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'APB Compliance Program', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'APB Compliance Program: Air Toxics', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'APB Compliance Program: Chemicals/Minerals', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'APB Compliance Program: VOC/Combustion', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Director''s Office', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Coastal District (Brunswick)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'East Central District (Augusta)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Mountain District (Atlanta)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Mountain District (Cartersville)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Northeast District (Athens)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Southwest District (Albany)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'West Central District (Macon)', convert(bit, 1), 'Office', sysdatetimeoffset();
INSERT INTO dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
SELECT newid(), N'Other', convert(bit, 1), 'Office', sysdatetimeoffset();
