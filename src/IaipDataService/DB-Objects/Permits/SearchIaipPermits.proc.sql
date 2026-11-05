USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.SearchIaipPermits
    @name varchar(100),
    @permit varchar(20),
    @dateFrom date,
    @dateTo date,
    @skip int,
    @take int,
    @sort varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Searches for permit data for use by the Air Web IAIP data service.

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
    where (@name is null or FacilityName like concat('%', @name, '%'))
      and (@permit is null or PermitNumber like concat('%', @permit, '%'))
      and (@dateFrom is null or IssuanceDate >= @dateFrom)
      and (@dateTo is null or IssuanceDate <= @dateTo)
    order by case @sort when 'Id' then FacilityId end,
             case @sort when 'Date' then IssuanceDate end,
             case @sort when 'DateDesc' then IssuanceDate end desc,
             case @sort when 'IdDesc' then FacilityId when 'NameDesc' then FacilityName end desc,
             case @sort when 'IdDesc' then FacilityName when 'NameDesc' then FacilityId end desc,
             FacilityName, FacilityId, IssuanceDate desc, ApplicationNumber desc
    offset @skip rows fetch next @take rows only;

END
GO
