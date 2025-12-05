USE airbranch;
GO

CREATE OR ALTER VIEW air.IaipFacilityAirProgramData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Normalizes Facility Air Program info for use in the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#162)
2024-10-30  DWaldron            Return integer for enum compatibility (#66)

***************************************************************************************************/

select t.Sequence,
       right(t.STRAIRSNUMBER, 8) as [FacilityId],
       t.AirProgram
from (select 1                                                       as [Sequence],
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 1, 1) = '1', 1, null) as [AirProgram] -- 'SIP'
      from dbo.APBHEADERDATA
      union
      select 2,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 2, 1) = '1', 2, null) -- 'Federal SIP'
      from dbo.APBHEADERDATA
      union
      select 3,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 3, 1) = '1', 3, null) -- 'Non-Federal SIP'
      from dbo.APBHEADERDATA
      union
      select 4,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 4, 1) = '1', 4, null) -- 'CFC Tracking'
      from dbo.APBHEADERDATA
      union
      select 5,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 5, 1) = '1', 5, null) -- 'PSD'
      from dbo.APBHEADERDATA
      union
      select 6,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 6, 1) = '1', 6, null) -- 'NSR'
      from dbo.APBHEADERDATA
      union
      select 9,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 7, 1) = '1', 7, null) -- 'NESHAP'
      from dbo.APBHEADERDATA
      union
      select 10,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 8, 1) = '1', 8, null) -- 'NSPS'
      from dbo.APBHEADERDATA
      union
      select 12,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 9, 1) = '1', 9, null) -- 'FESOP'
      from dbo.APBHEADERDATA
      union
      select 11,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 10, 1) = '1', 10, null) -- 'Acid Precipitation'
      from dbo.APBHEADERDATA
      union
      select 13,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 11, 1) = '1', 11, null) -- 'Native American'
      from dbo.APBHEADERDATA
      union
      select 8,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 12, 1) = '1', 12, null) -- 'MACT'
      from dbo.APBHEADERDATA
      union
      select 7,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 13, 1) = '1', 13, null) -- 'Title V'
      from dbo.APBHEADERDATA
      union
      select 14,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 14, 1) = '1', 14, null) -- 'Risk Management Plan'
      from dbo.APBHEADERDATA) t
where AirProgram is not null;

GO
