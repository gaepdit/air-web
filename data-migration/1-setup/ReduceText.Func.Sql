CREATE OR ALTER FUNCTION AIRBRANCH.air.ReduceText
(
    @input varchar(max)
)
    RETURNS varchar(max)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Converts placeholder text to null.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2025-11-14  DWaldron            Initial version (#32)

***************************************************************************************************/

BEGIN

    RETURN nullif(nullif(nullif(nullif(nullif(nullif(
                                                      trim(CHAR(10) + CHAR(13) + ' -' from @input),
                                                      ''), 'N/A'), 'NA'), 'N/A.'), 'None'), 'None.');

END
GO
