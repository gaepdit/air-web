USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacilityName
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves the name of a facility with a given ID for use by the Air Web IAIP data service.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-11-21  DWaldron            Initial version

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select STRFACILITYNAME as FacilityName
    from dbo.APBFACILITYINFORMATION
    where STRAIRSNUMBER = concat('0413', @FacilityId);

END;

GO
