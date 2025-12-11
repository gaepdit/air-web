INSERT INTO AirWeb.dbo.Lookups (Id, Name, Active, Discriminator, CreatedAt)
VALUES

-- Notification Types
-- Original values from AIRBRANCH.dbo.LOOKUPSSCPNOTIFICATIONS
(newid(), N'Other', convert(bit, 1), N'NotificationType', sysdatetimeoffset()),
(newid(), N'Startup', convert(bit, 1), N'NotificationType', sysdatetimeoffset()),
(newid(), N'Response Letter', convert(bit, 1), N'NotificationType', sysdatetimeoffset()),
(newid(), N'Malfunction', convert(bit, 0), N'NotificationType', sysdatetimeoffset()),
(newid(), N'Deviation', convert(bit, 0), N'NotificationType', sysdatetimeoffset()),

-- Offices
-- Original values modified from AIRBRANCH.dbo.LOOKUPEPDPROGRAMS and AIRBRANCH.dbo.LOOKUPEPDUNITS
(newid(), N'Air Protection Branch', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: Air Toxics', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: Chemicals/Minerals', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: VOC/Combustion', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Compliance Program: Source Monitoring Unit', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'Coastal District (Brunswick)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'East Central District (Augusta)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'Mountain District (Atlanta)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'Mountain District (Cartersville)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'Northeast District (Athens)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'Southwest District (Albany)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'West Central District (Macon)', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: Chemical Permitting', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: Combustion Permitting', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: Minerals Permitting', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: NOx Permitting', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'APB Permitting Program: VOC Permitting', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'EPD-IT', convert(bit, 1), N'Office', sysdatetimeoffset()),
(newid(), N'Other', convert(bit, 1), N'Office', sysdatetimeoffset());

select *
from AirWeb.dbo.Lookups;
