USE AIRBRANCH
GO

CREATE OR ALTER VIEW air.IaipFacilityProgramClassificationData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Normalizes Facility Program Classification info for use in the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#162)
2024-10-30  DWaldron            Return integer for enum compatibility (#66)

***************************************************************************************************/

select t.Sequence,
       right(t.STRAIRSNUMBER, 8) as [FacilityId],
       t.AirProgramClassification
from (select 1 as [Sequence],
             STRAIRSNUMBER,
             IIF(substring(STRSTATEPROGRAMCODES, 1, 1) = '1', 1, null) -- 'NSR/PSD Major'
                 as AirProgramClassification
      from dbo.APBHEADERDATA
      union
      select 2,
             STRAIRSNUMBER,
             IIF(substring(STRSTATEPROGRAMCODES, 2, 1) = '1', 2, null) -- 'HAPs Major'
      from dbo.APBHEADERDATA) t
where AirProgramClassification is not null;

GO
