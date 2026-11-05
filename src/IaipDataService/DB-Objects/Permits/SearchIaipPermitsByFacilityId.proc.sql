USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.SearchIaipPermitsByFacilityId
    @facilityId varchar(9),
    @skip int,
    @take int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Searches for permit data for a given facility for use by the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-11-05  DWaldron            Initial version (#717)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON

    select FacilityId,
           FacilityName,
           PermitNumber,
           IssuanceDate,
           FileType,
           VNarrative,
           VFinal,
           OtherNarrative,
           OtherPermit,
           PSDAppSum,
           PSDPrelim,
           PSDNarrative,
           PSDFinalDet,
           PSDFinal
    from dbo.VW_GA_PERMITS
    where FacilityId = @facilityId
    order by FacilityName, FacilityId, IssuanceDate desc, ApplicationNumber desc
    offset @skip rows fetch next @take rows only

END
GO
