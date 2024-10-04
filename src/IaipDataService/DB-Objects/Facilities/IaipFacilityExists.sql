USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.IaipFacilityExists
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Reports whether facility with a given ID exists.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2022-02-22  DWaldron            Initial version
2022-12-08  DWaldron            Update filter for approved facilities (IAIP-1177)
2024-10-03  DWaldron            Change Facility ID to 8 characters (air-web#162)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(*))
    from dbo.AFSFACILITYDATA
    where STRAIRSNUMBER = concat('0413', @FacilityId)
      and STRUPDATESTATUS in ('A', 'C');

END;

GO
