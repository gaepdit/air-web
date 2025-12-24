USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetIaipFacilityNextActionNumber
    @FacilityId varchar(8)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Retrieves the EPA Data Exchange action number and then increments the saved value.

Input Parameters:
    @FacilityId - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-12-22  DWaldron            Initial version

***************************************************************************************************/

    SET XACT_ABORT, NOCOUNT ON
BEGIN TRY

    BEGIN TRANSACTION;

    declare @Previous
        table
        (
            [ActionNumber] smallint
        )

    update dbo.APBSUPPLAMENTALDATA
    set STRAFSACTIONNUMBER = convert(smallint, STRAFSACTIONNUMBER) + 1
    output convert(smallint, deleted.STRAFSACTIONNUMBER) as ActionNumber into @Previous
    where STRAIRSNUMBER = concat('0413', @FacilityId);

    select ActionNumber from @Previous;

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
