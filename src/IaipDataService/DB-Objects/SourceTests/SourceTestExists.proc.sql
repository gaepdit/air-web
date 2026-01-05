USE airbranch;
GO

CREATE OR ALTER PROCEDURE air.SourceTestExists
    @ReferenceNumber int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Reports whether a stack test exists 
            for a given facility and reference number.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2024-10-08  DWaldron            Initial version (#165)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(1))
    from dbo.ISMPREPORTINFORMATION
    where STRDOCUMENTTYPE <> '001'
      and STRDELETE is null
      and convert(int, STRREFERENCENUMBER) = @ReferenceNumber

END
GO
