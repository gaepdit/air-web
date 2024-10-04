USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.SourceTestExists
    @FacilityId      varchar(8),
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Reports whether a stack test exists 
            for a given facility and reference number.

Input Parameters:
    @FacilityId - The Facility ID
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version
2022-02-24  DWaldron            Exclude deleted stack tests
2024-10-04  DWaldron            Change Facility ID to 8 characters (air-web#162)

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(1))
    from dbo.ISMPMASTER m
        inner join dbo.ISMPREPORTINFORMATION r
        on m.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    where r.STRDOCUMENTTYPE <> '001'
      and r.STRDELETE is null
      and m.STRAIRSNUMBER = concat('0413', @FacilityId)
      and convert(int, m.STRREFERENCENUMBER) = @ReferenceNumber

END;

GO
