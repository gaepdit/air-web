USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER VIEW air.IaipFacilityProgramClassificationData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Normalizes Facility Program Classification info.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-04  DWaldron            Initial version (#162)

***************************************************************************************************/

select t.Sequence,
       right(t.STRAIRSNUMBER, 8) as [FacilityId],
       t.AirProgram
from (select 1 as [Sequence],
             STRAIRSNUMBER,
             IIF(substring(STRSTATEPROGRAMCODES, 1, 1) = '1', 'NSR/PSD Major', null)
               as [AirProgram]
      from dbo.APBHEADERDATA
      union
      select 2,
             STRAIRSNUMBER,
             IIF(substring(STRSTATEPROGRAMCODES, 2, 1) = '1', 'HAPs Major', null)
      from dbo.APBHEADERDATA) t
where AirProgram is not null;

GO
