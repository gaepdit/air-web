USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.CountIaipPermitsByFacilityId
    @facilityId varchar(9)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Counts permit data for a given facility for use by the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-11-05  DWaldron            Initial version (#717)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON

    select count(*) from dbo.VW_GA_PERMITS where FacilityId = @facilityId;

END
GO
