USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacilityList
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves list of active facilities including AIRS # and Name for use by the 
  Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-11-21  DWaldron            Initial version

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select right(f.STRAIRSNUMBER, 8) as [Key],
           trim(f.STRFACILITYNAME)   as [Value]
    from dbo.APBFACILITYINFORMATION f
        inner join dbo.AFSFACILITYDATA a
        on f.STRAIRSNUMBER = a.STRAIRSNUMBER
    where a.STRUPDATESTATUS in ('A', 'C')
    order by f.STRAIRSNUMBER;

END;

GO
