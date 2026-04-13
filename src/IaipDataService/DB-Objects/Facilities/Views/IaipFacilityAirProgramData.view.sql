USE AIRBRANCH
GO

CREATE OR ALTER VIEW air.IaipFacilityAirProgramData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Normalizes Facility Air Program info for use in the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#162)
2024-10-30  DWaldron            Return integer for enum compatibility (#66)
2026-03-17  DWaldron            Change Air Programs from enum values to codes (#466)

***************************************************************************************************/

select t.Sequence,
       right(t.STRAIRSNUMBER, 8) as [FacilityId],
       t.Code,
       t.Description
from (select 1                                                              as [Sequence],
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 1, 1) = '1', 'CAASIP', null) as [Code],
             'SIP' as [Description]
      from dbo.APBHEADERDATA
      union
      select 2,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 2, 1) = '1', 'CAAFIP', null),
             'Federal SIP'
      from dbo.APBHEADERDATA
      union
      select 3,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 3, 1) = '1', 'CAANFRP', null),
             'Non-Federal SIP'
      from dbo.APBHEADERDATA
      union
      select 4,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 4, 1) = '1', 'CAACFC', null),
             'CFC Tracking'
      from dbo.APBHEADERDATA
      union
      select 5,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 5, 1) = '1', 'CAAPSD', null),
             'PSD'
      from dbo.APBHEADERDATA
      union
      select 6,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 6, 1) = '1', 'CAANSR', null),
             'NSR'
      from dbo.APBHEADERDATA
      union
      select 9,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 7, 1) = '1', 'CAANESH', null),
             'NESHAP'
      from dbo.APBHEADERDATA
      union
      select 10,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 8, 1) = '1', 'CAANSPS', null),
             'NSPS'
      from dbo.APBHEADERDATA
      union
      select 12,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 9, 1) = '1', 'CAAFESOP', null),
             'FESOP'
      from dbo.APBHEADERDATA
      union
      select 11,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 10, 1) = '1', 'CAAAR', null),
             'Acid Precipitation'
      from dbo.APBHEADERDATA
      union
      select 13,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 11, 1) = '1', 'CAANAM', null),
             'Native American'
      from dbo.APBHEADERDATA
      union
      select 8,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 12, 1) = '1', 'CAAMACT', null),
             'MACT'
      from dbo.APBHEADERDATA
      union
      select 7,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 13, 1) = '1', 'CAATVP', null),
             'Title V'
      from dbo.APBHEADERDATA
      union
      select 14,
             STRAIRSNUMBER,
             IIF(substring(STRAIRPROGRAMCODES, 14, 1) = '1', 'CAARMP', null),
             'Risk Management Plan'
      from dbo.APBHEADERDATA) t
where Code is not null;

GO
