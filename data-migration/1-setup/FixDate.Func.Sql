USE AIRBRANCH
GO

CREATE OR ALTER FUNCTION air.FixDate
(
    @input datetime2
)
    RETURNS date
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Converts datetime to date and replaces 4-Jul-1776 with null.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-11-14  DWaldron            Initial version (#32)

***************************************************************************************************/

BEGIN

    RETURN convert(date, nullif(@input, '1776-07-04'));

END
GO
