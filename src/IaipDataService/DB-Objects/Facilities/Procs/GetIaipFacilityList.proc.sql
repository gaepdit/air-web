USE AIRBRANCH
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
2026-04-08  DWaldron            Refactored to include more information for each facility (#545)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select right(f.STRAIRSNUMBER, 8) as [Id],
           trim(f.STRFACILITYNAME)   as [Name],
           f.STRFACILITYCITY as [City],
           f.STRFACILITYSTATE as [State]
    from dbo.APBFACILITYINFORMATION f
        inner join dbo.AFSFACILITYDATA a
            on f.STRAIRSNUMBER = a.STRAIRSNUMBER
    where a.STRUPDATESTATUS in ('A', 'C')
    order by f.STRAIRSNUMBER;

END;

GO
