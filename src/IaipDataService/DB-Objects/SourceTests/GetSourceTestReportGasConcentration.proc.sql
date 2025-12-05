USE airbranch;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestReportGasConcentration
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Gas Concentration" type 
            stack test.

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
           d.STRPERCENTALLOWABLE           as PercentAllowable,
           'MaxOperatingCapacity'          as Id,
           trim(d.STRMAXOPERATINGCAPACITY) as Value,
           u1.STRUNITDESCRIPTION           as Units,
           'OperatingCapacity'             as Id,
           trim(d.STROPERATINGCAPACITY)    as Value,
           u2.STRUNITDESCRIPTION           as Units,
           'AvgPollutantConcentration'     as Id,
           d.STRPOLLUTANTCONCENTRATIONAVG  as Value,
           u3.STRUNITDESCRIPTION           as Units,
           'AvgEmissionRate'               as Id,
           d.STREMISSIONRATEAVG            as Value,
           u4.STRUNITDESCRIPTION           as Units
    from dbo.ISMPREPORTINFORMATION r
        inner join dbo.ISMPREPORTPONDANDGAS d
            on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
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
          from dbo.ISMPREPORTPONDANDGAS
          union
          select STRREFERENCENUMBER,
                 2 as Id,
                 STRALLOWABLEEMISSIONRATE2,
                 STRALLOWABLEEMISSIONRATEUNIT2
          from dbo.ISMPREPORTPONDANDGAS
          union
          select STRREFERENCENUMBER,
                 3 as Id,
                 STRALLOWABLEEMISSIONRATE3,
                 STRALLOWABLEEMISSIONRATEUNIT3
          from dbo.ISMPREPORTPONDANDGAS) t
        inner join dbo.LOOKUPUNITS u
            on u.STRUNITKEY = t.UnitCode
            and u.STRUNITKEY <> @InvalidKey
    where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
    order by t.Id;

    select trim(RunNumber)                        as RunNumber,
           trim(PollutantConcentration)           as PollutantConcentration,
           trim(EmissionRate)                     as EmissionRate,
           isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
    from (select s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1A                        as RunNumber,
                 s.STRPOLLUTANTCONCENTRATION1A           as PollutantConcentration,
                 s.STREMISSIONRATE1A                     as EmissionRate,
                 substring(r.STRCONFIDENTIALDATA, 33, 3) as ConfidentialParametersCode
          from dbo.ISMPREPORTPONDANDGAS s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1B,
                 s.STRPOLLUTANTCONCENTRATION1B,
                 s.STREMISSIONRATE1B,
                 substring(r.STRCONFIDENTIALDATA, 36, 3)
          from dbo.ISMPREPORTPONDANDGAS s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1C,
                 s.STRPOLLUTANTCONCENTRATION1C,
                 s.STREMISSIONRATE1C,
                 substring(r.STRCONFIDENTIALDATA, 39, 3)
          from dbo.ISMPREPORTPONDANDGAS s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER) t
    where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
    order by t.RunNumber;

END;

GO
