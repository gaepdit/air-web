USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacilityNextActionNumber
    @FacilityId   varchar(12),
    @ActionNumber smallint = null output
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves the EPA Data Exchange action number and then increments the saved value.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-12-22  DWaldron            Initial version (air-web#455)
2026-07-09  DWaldron            - Allow different AIRS number formats as input
                                - Return action number as output parameter (epa-dx#105)

***************************************************************************************************/

    SET XACT_ABORT, NOCOUNT ON
BEGIN TRY

    declare @previous
        table
        (
            [ActionNumber] smallint
        );

    BEGIN TRANSACTION;

    -- Increment the action number for the facility and remember the previous value for use by the caller.
    update dbo.APBSUPPLAMENTALDATA
    set STRAFSACTIONNUMBER = convert(smallint, STRAFSACTIONNUMBER) + 1
    output convert(smallint, deleted.STRAFSACTIONNUMBER) as ActionNumber into @previous
    where STRAIRSNUMBER = iaip_facility.DbFormatAirsNumber(@FacilityId);

    -- Store the action number in the output parameter.
    select @ActionNumber = ActionNumber from @previous;

    -- Select the action number.
    select @ActionNumber;

    COMMIT TRANSACTION;
    RETURN 0;
END TRY
BEGIN CATCH
    IF @@trancount > 0
        ROLLBACK TRANSACTION;
    DECLARE
        @ErrorMessage nvarchar(4000) = ERROR_MESSAGE(),
        @ErrorSeverity int = ERROR_SEVERITY();
    RAISERROR (@ErrorMessage, @ErrorSeverity, 1);
    RETURN -1;
END CATCH
GO
