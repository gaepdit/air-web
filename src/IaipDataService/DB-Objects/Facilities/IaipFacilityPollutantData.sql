USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER VIEW air.IaipFacilityPollutantData
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Normalizes Facility Pollutant info for use in the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-30  DWaldron            Initial version (#66)

***************************************************************************************************/

select distinct right(p.STRAIRSNUMBER, 8)      as [FacilityId],
                i.ICIS_POLLUTANT_CODE          as Code,
                iif(l.STRPOLLUTANTDESCRIPTION is null,
                    i.ICIS_POLLUTANT_DESC,
                    l.STRPOLLUTANTDESCRIPTION) as Description
from dbo.APBAIRPROGRAMPOLLUTANTS p
    inner join dbo.LK_ICIS_POLLUTANT i
    on p.STRPOLLUTANTKEY = i.LGCY_POLLUTANT_CODE
    left join dbo.LOOKUPPOLLUTANTS l
    on p.STRPOLLUTANTKEY = l.STRPOLLUTANTCODE;

GO
