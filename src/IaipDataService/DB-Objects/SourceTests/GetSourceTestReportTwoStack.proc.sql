USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestReportTwoStack
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Two Stack" type stack test.

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
           trim(d.STRSTACKONENAME)             as StackOneName,
           trim(d.STRSTACKTWONAME)             as StackTwoName,
           trim(d.STRPERCENTALLOWABLE)         as PercentAllowable,
           trim(d.STRDESTRUCTIONPERCENT)       as DestructionEfficiency,
           'MaxOperatingCapacity'              as Id,
           trim(d.STRMAXOPERATINGCAPACITY)     as Value,
           u1.STRUNITDESCRIPTION               as Units,
           'OperatingCapacity'                 as Id,
           trim(d.STROPERATINGCAPACITY)        as Value,
           u2.STRUNITDESCRIPTION               as Units,
           'StackOneAvgPollutantConcentration' as Id,
           d.STRPOLLUTANTCONCENTRATIONAVG1     as Value,
           u3.STRUNITDESCRIPTION               as Units,
           'StackTwoAvgPollutantConcentration' as Id,
           d.STRPOLLUTANTCONCENTRATIONAVG2     as Value,
           u3.STRUNITDESCRIPTION               as Units,
           'StackOneAvgEmissionRate'           as Id,
           d.STREMISSIONRATEAVG1               as Value,
           u4.STRUNITDESCRIPTION               as Units,
           'StackTwoAvgEmissionRate'           as Id,
           d.STREMISSIONRATEAVG2               as Value,
           u4.STRUNITDESCRIPTION               as Units,
           'SumAvgEmissionRate'                as Id,
           d.STREMISSIONRATETOTALAVG           as Value,
           u4.STRUNITDESCRIPTION               as Units
    from dbo.ISMPREPORTINFORMATION r
        inner join dbo.ISMPREPORTTWOSTACK d
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
          from dbo.ISMPREPORTTWOSTACK
          union
          select STRREFERENCENUMBER,
                 2 as Id,
                 STRALLOWABLEEMISSIONRATE2,
                 STRALLOWABLEEMISSIONRATEUNIT2
          from dbo.ISMPREPORTTWOSTACK
          union
          select STRREFERENCENUMBER,
                 3 as Id,
                 STRALLOWABLEEMISSIONRATE3,
                 STRALLOWABLEEMISSIONRATEUNIT3
          from dbo.ISMPREPORTTWOSTACK) t
        inner join dbo.LOOKUPUNITS u
            on u.STRUNITKEY = t.UnitCode
            and u.STRUNITKEY <> @InvalidKey
    where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
    order by t.Id;

    select trim(RunNumber)                        as RunNumber,
           trim(StackOneGasTemperature)           as StackOneGasTemperature,
           trim(StackOneGasMoisture)              as StackOneGasMoisture,
           trim(StackOneGasFlowRateAcfm)          as StackOneGasFlowRateAcfm,
           trim(StackOneGasFlowRateDscfm)         as StackOneGasFlowRateDscfm,
           trim(StackOnePollutantConcentration)   as StackOnePollutantConcentration,
           trim(StackOneEmissionRate)             as StackOneEmissionRate,
           trim(StackTwoGasTemperature)           as StackTwoGasTemperature,
           trim(StackTwoGasMoisture)              as StackTwoGasMoisture,
           trim(StackTwoGasFlowRateAcfm)          as StackTwoGasFlowRateAcfm,
           trim(StackTwoGasFlowRateDscfm)         as StackTwoGasFlowRateDscfm,
           trim(StackTwoPollutantConcentration)   as StackTwoPollutantConcentration,
           trim(StackTwoEmissionRate)             as StackTwoEmissionRate,
           trim(SumEmissionRate)                  as SumEmissionRate,
           isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
    from (select 1                             as Id, -- run 1
                 s.STRREFERENCENUMBER          as ReferenceNumber,
                 s.STRRUNNUMBER1A              as RunNumber,
                 s.STRGASTEMPERATURE1A         as StackOneGasTemperature,
                 s.STRGASMOISTURE1A            as StackOneGasMoisture,
                 s.STRGASFLOWRATEACFM1A        as StackOneGasFlowRateAcfm,
                 s.STRGASFLOWRATEDSCFM1A       as StackOneGasFlowRateDscfm,
                 s.STRPOLLUTANTCONCENTRATION1A as StackOnePollutantConcentration,
                 s.STREMISSIONRATE1A           as StackOneEmissionRate,
                 s.STRGASTEMPERATURE2A         as StackTwoGasTemperature,
                 s.STRGASMOISTURE2A            as StackTwoGasMoisture,
                 s.STRGASFLOWRATEACFM2A        as StackTwoGasFlowRateAcfm,
                 s.STRGASFLOWRATEDSCFM2A       as StackTwoGasFlowRateDscfm,
                 s.STRPOLLUTANTCONCENTRATION2A as StackTwoPollutantConcentration,
                 s.STREMISSIONRATE2A           as StackTwoEmissionRate,
                 s.STREMISSIONRATETOTAL1       as SumEmissionRate,
                 concat(substring(r.STRCONFIDENTIALDATA, 36, 7),
                        substring(r.STRCONFIDENTIALDATA, 58, 6),
                        substring(r.STRCONFIDENTIALDATA, 84, 1))
                                               as ConfidentialParametersCode
          from dbo.ISMPREPORTTWOSTACK s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select 2, -- run 2
                 s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1B,
                 s.STRGASTEMPERATURE1B,
                 s.STRGASMOISTURE1B,
                 s.STRGASFLOWRATEACFM1B,
                 s.STRGASFLOWRATEDSCFM1B,
                 s.STRPOLLUTANTCONCENTRATION1B,
                 s.STREMISSIONRATE1B,
                 s.STRGASTEMPERATURE2B,
                 s.STRGASMOISTURE2B,
                 s.STRGASFLOWRATEACFM2B,
                 s.STRGASFLOWRATEDSCFM2B,
                 s.STRPOLLUTANTCONCENTRATION2B,
                 s.STREMISSIONRATE2B,
                 s.STREMISSIONRATETOTAL2,
                 concat(substring(r.STRCONFIDENTIALDATA, 43, 7),
                        substring(r.STRCONFIDENTIALDATA, 65, 6),
                        substring(r.STRCONFIDENTIALDATA, 85, 1))
          from dbo.ISMPREPORTTWOSTACK s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select 3, -- run 3
                 s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1C,
                 s.STRGASTEMPERATURE1C,
                 s.STRGASMOISTURE1C,
                 s.STRGASFLOWRATEACFM1C,
                 s.STRGASFLOWRATEDSCFM1C,
                 s.STRPOLLUTANTCONCENTRATION1C,
                 s.STREMISSIONRATE1C,
                 s.STRGASTEMPERATURE2C,
                 s.STRGASMOISTURE2C,
                 s.STRGASFLOWRATEACFM2C,
                 s.STRGASFLOWRATEDSCFM2C,
                 s.STRPOLLUTANTCONCENTRATION2C,
                 s.STREMISSIONRATE2C,
                 s.STREMISSIONRATETOTAL3,
                 concat(substring(r.STRCONFIDENTIALDATA, 50, 7),
                        substring(r.STRCONFIDENTIALDATA, 72, 6),
                        substring(r.STRCONFIDENTIALDATA, 86, 1))
          from dbo.ISMPREPORTTWOSTACK s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER) t
    where convert(int, ReferenceNumber) = @ReferenceNumber
    order by Id;

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportTwoStack', @params;

END;

GO
