USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE etl.GetFacilityEpaDxDate
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves the latest EPA Data Exchange date a given Facility ID for use by the
            Air Web IAIP data service. (Based on AIRBRANCH.dbo.VW_FACILITY_DATADATES.)

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-07-17  DWaldron            Initial version (iaip#1433)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON

    select MAX(SubmitDate)
    from NETWORKNODEFLOW.dbo.SubmissionStatus
    where Status = 'ACCEPTED'
      and ID like concat('%13', @FacilityId, '%');

END
GO
