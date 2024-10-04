USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER VIEW air.IaipFacilityAirProgramData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Normalizes Facility Air Program info.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#162)

***************************************************************************************************/

select t.Sequence,
       right(t.STRAIRSNUMBER, 8) as [FacilityId],
       t.AirProgram
from (select 1                                                           as [Sequence],
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 1, 1) = '1', 'SIP', null) as [AirProgram]
      from dbo.APBHEADERDATA
      union
      select 2,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 2, 1) = '1', 'Federal SIP', null)
      from dbo.APBHEADERDATA
      union
      select 3,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 3, 1) = '1', 'Non-Federal SIP', null)
      from dbo.APBHEADERDATA
      union
      select 4,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 4, 1) = '1', 'CFC Tracking', null)
      from dbo.APBHEADERDATA
      union
      select 5,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 5, 1) = '1', 'PSD', null)
      from dbo.APBHEADERDATA
      union
      select 6,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 6, 1) = '1', 'NSR', null)
      from dbo.APBHEADERDATA
      union
      select 9,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 7, 1) = '1', 'NESHAP', null)
      from dbo.APBHEADERDATA
      union
      select 10,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 8, 1) = '1', 'NSPS', null)
      from dbo.APBHEADERDATA
      union
      select 12,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 9, 1) = '1', 'FESOP', null)
      from dbo.APBHEADERDATA
      union
      select 11,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 10, 1) = '1', 'Acid Precipitation', null)
      from dbo.APBHEADERDATA
      union
      select 13,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 11, 1) = '1', 'Native American', null)
      from dbo.APBHEADERDATA
      union
      select 8,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 12, 1) = '1', 'MACT', null)
      from dbo.APBHEADERDATA
      union
      select 7,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 13, 1) = '1', 'Title V', null)
      from dbo.APBHEADERDATA
      union
      select 14,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 14, 1) = '1', 'Risk Management Plan', null)
      from dbo.APBHEADERDATA) t
where AirProgram is not null;

GO
