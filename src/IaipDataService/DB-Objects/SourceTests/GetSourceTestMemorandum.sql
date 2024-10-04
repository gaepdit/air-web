USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestMemorandum
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Memorandum" type stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    declare @InvalidKey varchar(5) = '00000';

    select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                                 as ControlEquipmentInfo,
           trim(char(13) + char(10) + ' ' from d.STRMEMORANDUMFIELD)
                                                 as Comments,
           trim(d.STRMONITORMANUFACTUREANDMODEL) as MonitorManufacturer,
           trim(d.STRMONITORSERIALNUMBER)        as MonitorSerialNumber,
           'MaxOperatingCapacity'                as Id,
           d.STRMAXOPERATINGCAPACITY             as Value,
           u1.STRUNITDESCRIPTION                 as Units,
           'OperatingCapacity'                   as Id,
           d.STROPERATINGCAPACITY                as Value,
           u1.STRUNITDESCRIPTION                 as Units
    from dbo.ISMPREPORTINFORMATION r
        inner join dbo.ISMPREPORTMEMO d
        on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
        left join dbo.LOOKUPUNITS u1
        on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
            and u1.STRUNITKEY <> @InvalidKey
        left join dbo.LOOKUPUNITS u2
        on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
            and u2.STRUNITKEY <> @InvalidKey
    where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

    select trim(t.Value)        as Value,
           u.STRUNITDESCRIPTION as Units
    from (select STRREFERENCENUMBER,
                 1                              as Id,
                 STRALLOWABLEEMISSIONRATE1A     as Value,
                 STRALLOWABLEEMISSIONRATEUNIT1A as UnitCode
          from dbo.ISMPREPORTMEMO
          union
          select STRREFERENCENUMBER,
                 2 as Id,
                 STRALLOWABLEEMISSIONRATE1B,
                 STRALLOWABLEEMISSIONRATEUNIT1B
          from dbo.ISMPREPORTMEMO
          union
          select STRREFERENCENUMBER,
                 3 as Id,
                 STRALLOWABLEEMISSIONRATE1C,
                 STRALLOWABLEEMISSIONRATEUNIT1C
          from dbo.ISMPREPORTMEMO) t
        inner join dbo.LOOKUPUNITS u
        on u.STRUNITKEY = t.UnitCode
            and u.STRUNITKEY <> @InvalidKey
    where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
    order by t.Id;

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportMemorandum', @params;

END;

GO
