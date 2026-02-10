USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.UpdateSourceTest
    @ReferenceNumber int,
    @ComplianceAssignmentEmail varchar(100),
    @ComplianceReviewDate date
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Updates a source test record with compliance review data.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-09-25  DWaldron            Initial version (#359)
2025-11-03  DWaldron            Save email instead of IAIP User ID (iaip#1334)

***************************************************************************************************/

    SET XACT_ABORT, NOCOUNT ON
BEGIN TRY
    BEGIN TRANSACTION;

    update dbo.ISMPREPORTINFORMATION
    set ComplianceAssignment = @ComplianceAssignmentEmail,
        ComplianceReviewDate = @ComplianceReviewDate
    where STRREFERENCENUMBER = @ReferenceNumber;

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
