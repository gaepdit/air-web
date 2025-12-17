INSERT INTO AirWeb.dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
VALUES

-- Notification Types
-- Original values from AIRBRANCH.dbo.LOOKUPSSCPNOTIFICATIONS
(newid(), N'Other', 1, N'NotificationType', sysdatetimeoffset()),
(newid(), N'Startup', 1, N'NotificationType', sysdatetimeoffset()),
(newid(), N'Response Letter', 1, N'NotificationType', sysdatetimeoffset()),
(newid(), N'Malfunction', 0, N'NotificationType', sysdatetimeoffset()),
(newid(), N'Deviation', 0, N'NotificationType', sysdatetimeoffset()),

-- Offices
-- Original values modified from AIRBRANCH.dbo.LOOKUPEPDPROGRAMS and AIRBRANCH.dbo.LOOKUPEPDUNITS
(newid(), N'Air Protection Branch', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: Air Toxics', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: Chemicals/Minerals', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: VOC/Combustion', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: Source Monitoring Unit', 1, N'Office', sysdatetimeoffset()),
(newid(), N'Coastal District (Brunswick)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'East Central District (Augusta)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'Mountain District (Atlanta)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'Mountain District (Cartersville)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'Northeast District (Athens)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'Southwest District (Albany)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'West Central District (Macon)', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: Chemical Permitting', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: Combustion Permitting', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: Minerals Permitting', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: NOx Permitting', 1, N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: VOC Permitting', 1, N'Office', sysdatetimeoffset()),
(newid(), N'EPD-IT', 1, N'Office', sysdatetimeoffset()),
(newid(), N'Other', 1, N'Office', sysdatetimeoffset());

select *
from AirWeb.dbo.Lookups;
