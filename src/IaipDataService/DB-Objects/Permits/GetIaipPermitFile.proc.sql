USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetIaipPermitFile
    @fileName varchar(12)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Gets permit document for use by the Air Web IAIP data service.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2026-11-05  DWaldron            Initial version (#717)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON

    select PDFPERMITDATA
    from dbo.APBPERMITS
    where STRFILENAME = @fileName;

END
GO
