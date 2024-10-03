USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.LogReport @type nvarchar(max),
                                        @parameters nvarchar(max)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Logs reports.

Input Parameters:
    @type - The type of report.
    @parameters - The parameters of the report.

Returns:
   0 on success
  -1 on error

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2022-02-22  DWaldron            Initial version

***************************************************************************************************/

    SET XACT_ABORT, NOCOUNT ON;
BEGIN TRY

    BEGIN TRANSACTION;

    insert into air.ReportsLog
    (Type,
     Parameters)
    values (@type,
            @parameters);

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
END CATCH;
GO
