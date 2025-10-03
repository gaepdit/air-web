USE [AIRBRANCH]
GO
SET ANSI_NULLS ON
GO

CREATE OR ALTER FUNCTION air.FormatAirsNumber
(
    @input varchar(12)
)
    RETURNS varchar(9)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:
  Reormat the old DB AIRS numbers.

Input Parameters:
  @input - AIRS number in the form '041300100001'

Returns:
  AIRS number in the form '001-00001'

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-10-02  DWaldron            Initial version (#32)

***************************************************************************************************/

BEGIN

    RETURN concat_ws('-', substring(@input, 5, 3), right(@input, 5));

END
GO
