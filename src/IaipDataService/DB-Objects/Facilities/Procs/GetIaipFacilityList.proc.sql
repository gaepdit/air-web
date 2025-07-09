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
2025-07-09  DWaldron            Added filter to exclude bad data (AIRS # ending in '00000')

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select right(f.STRAIRSNUMBER, 8) as [Key],
           trim(f.STRFACILITYNAME)   as [Value]
    from dbo.APBFACILITYINFORMATION f
        inner join dbo.AFSFACILITYDATA a
            on f.STRAIRSNUMBER = a.STRAIRSNUMBER
    where a.STRUPDATESTATUS in ('A', 'C')
      -- Once the bad data is cleaned up, we can remove this filter
      and f.STRAIRSNUMBER <> '041312100000'
    order by f.STRAIRSNUMBER;

END;

GO
