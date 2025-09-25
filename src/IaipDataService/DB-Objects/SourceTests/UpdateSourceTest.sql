USE airbranch;
GO
SET ANSI_NULLS ON;
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

***************************************************************************************************/

    SET XACT_ABORT, NOCOUNT ON
BEGIN TRY

    declare @userId int;

    BEGIN TRANSACTION;

    select @userId = NUMUSERID
    from AIRBRANCH.dbo.EPDUSERPROFILES
    where STREMAILADDRESS = @ComplianceAssignmentEmail;

    update dbo.ISMPREPORTINFORMATION
    set ComplianceAssignment = @userId,
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
