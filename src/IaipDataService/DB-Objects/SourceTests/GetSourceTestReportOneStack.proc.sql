USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestReportOneStack
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "One Stack" type stack test.

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
           trim(d.STRPERCENTALLOWABLE)          as PercentAllowable,
           'MaxOperatingCapacity'               as Id,
           trim(d.STRMAXOPERATINGCAPACITY)      as Value,
           u1.STRUNITDESCRIPTION                as Units,
           'OperatingCapacity'                  as Id,
           trim(d.STROPERATINGCAPACITY)         as Value,
           u2.STRUNITDESCRIPTION                as Units,
           'AvgPollutantConcentration'          as Id,
           trim(d.STRPOLLUTANTCONCENTRATIONAVG) as Value,
           u3.STRUNITDESCRIPTION                as Units,
           'AvgEmissionRate'                    as Id,
           trim(d.STREMISSIONRATEAVG)           as Value,
           u4.STRUNITDESCRIPTION                as Units
    from dbo.ISMPREPORTINFORMATION r
        inner join dbo.ISMPREPORTONESTACK d
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
        left join dbo.LOOKUPUNITS u1
        on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
            and u1.STRUNITKEY <> @InvalidKey
        left join dbo.LOOKUPUNITS u2
        on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
            and u2.STRUNITKEY <> @InvalidKey
        left join dbo.LOOKUPUNITS u3
        on u3.STRUNITKEY = d.STRPOLLUTANTCONCENTRATIONUNIT
            and u3.STRUNITKEY <> @InvalidKey
        left join dbo.LOOKUPUNITS u4
        on u4.STRUNITKEY = d.STREMISSIONRATEUNIT
            and u4.STRUNITKEY <> @InvalidKey
    where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

    select trim(t.Value)        as Value,
           u.STRUNITDESCRIPTION as Units
    from (select STRREFERENCENUMBER,
                 1                             as Id,
                 STRALLOWABLEEMISSIONRATE1     as Value,
                 STRALLOWABLEEMISSIONRATEUNIT1 as UnitCode
          from dbo.ISMPREPORTONESTACK
          union
          select STRREFERENCENUMBER,
                 2 as Id,
                 STRALLOWABLEEMISSIONRATE2,
                 STRALLOWABLEEMISSIONRATEUNIT2
          from dbo.ISMPREPORTONESTACK
          union
          select STRREFERENCENUMBER,
                 3 as Id,
                 STRALLOWABLEEMISSIONRATE3,
                 STRALLOWABLEEMISSIONRATEUNIT3
          from dbo.ISMPREPORTONESTACK) t
        inner join dbo.LOOKUPUNITS u
        on u.STRUNITKEY = t.UnitCode
            and u.STRUNITKEY <> @InvalidKey
    where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
    order by t.Id;

    select trim(RunNumber)                        as RunNumber,
           trim(GasTemperature)                   as GasTemperature,
           trim(GasMoisture)                      as GasMoisture,
           trim(GasFlowRateAcfm)                  as GasFlowRateAcfm,
           trim(GasFlowRateDscfm)                 as GasFlowRateDscfm,
           trim(PollutantConcentration)           as PollutantConcentration,
           trim(EmissionRate)                     as EmissionRate,
           isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
    from (select 1                                       as Id,
                 s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1A                        as RunNumber,
                 s.STRGASTEMPERATURE1A                   as GasTemperature,
                 s.STRGASMOISTURE1A                      as GasMoisture,
                 s.STRGASFLOWRATEACFM1A                  as GasFlowRateAcfm,
                 s.STRGASFLOWRATEDSCFM1A                 as GasFlowRateDscfm,
                 s.STRPOLLUTANTCONCENTRATION1A           as PollutantConcentration,
                 s.STREMISSIONRATE1A                     as EmissionRate,
                 substring(r.STRCONFIDENTIALDATA, 35, 7) as ConfidentialParametersCode
          from dbo.ISMPREPORTONESTACK s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select 2,
                 s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1B,
                 s.STRGASTEMPERATURE1B,
                 s.STRGASMOISTURE1B,
                 s.STRGASFLOWRATEACFM1B,
                 s.STRGASFLOWRATEDSCFM1B,
                 s.STRPOLLUTANTCONCENTRATION1B,
                 s.STREMISSIONRATE1B,
                 substring(r.STRCONFIDENTIALDATA, 42, 7)
          from dbo.ISMPREPORTONESTACK s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select 3,
                 IIF(r.STRDOCUMENTTYPE in ('003', '004'), s.STRREFERENCENUMBER, null),
                 s.STRRUNNUMBER1C,
                 s.STRGASTEMPERATURE1C,
                 s.STRGASMOISTURE1C,
                 s.STRGASFLOWRATEACFM1C,
                 s.STRGASFLOWRATEDSCFM1C,
                 s.STRPOLLUTANTCONCENTRATION1C,
                 s.STREMISSIONRATE1C,
                 IIF(r.STRDOCUMENTTYPE in ('003', '004'), substring(r.STRCONFIDENTIALDATA, 49, 7), null)
          from dbo.ISMPREPORTONESTACK s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select 4,
                 IIF(r.STRDOCUMENTTYPE in ('004'), s.STRREFERENCENUMBER, null),
                 s.STRRUNNUMBER1D,
                 s.STRGASTEMPERATURE1D,
                 s.STRGASMOISTURE1D,
                 s.STRGASFLOWRATEACFM1D,
                 s.STRGASFLOWRATEDSCFM1D,
                 s.STRPOLLUTANTCONCENTRATION1D,
                 s.STREMISSIONRATE1D,
                 IIF(r.STRDOCUMENTTYPE in ('003', '004'), substring(r.STRCONFIDENTIALDATA, 56, 7), null)
          from dbo.ISMPREPORTONESTACK s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER) t
    where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
    order by Id;

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportOneStack', @params;

END;

GO
