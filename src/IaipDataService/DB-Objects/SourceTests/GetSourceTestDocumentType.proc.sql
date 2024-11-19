USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestDocumentType
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves the document type for a given stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version
2022-02-24  DWaldron            Exclude deleted stack tests

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(int, STRDOCUMENTTYPE) as DocumentType
    from dbo.ISMPREPORTINFORMATION
    where STRREFERENCENUMBER = @ReferenceNumber
      and STRDELETE is null

END;

GO
