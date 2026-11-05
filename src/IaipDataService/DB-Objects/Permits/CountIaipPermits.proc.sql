USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.CountIaipPermits
    @name varchar(100),
    @permit varchar(20),
    @dateFrom date,
    @dateTo date
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Counts permit data for use by the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-11-05  DWaldron            Initial version (#717)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON

    select count(*)
    from dbo.VW_GA_PERMITS
    where (@name is null or FacilityName like concat('%', @name, '%'))
      and (@permit is null or PermitNumber like concat('%', @permit, '%'))
      and (@dateFrom is null or IssuanceDate >= @dateFrom)
      and (@dateTo is null or IssuanceDate <= @dateTo);

END
GO
