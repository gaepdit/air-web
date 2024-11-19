USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestReportPondTreatment
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Pond Treatment (Pulping
            Process Condensate)" type stack test.

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
           d.STRPERCENTALLOWABLE           as DestructionEfficiency,
           'MaxOperatingCapacity'          as Id,
           trim(d.STRMAXOPERATINGCAPACITY) as Value,
           u1.STRUNITDESCRIPTION           as Units,
           'OperatingCapacity'             as Id,
           trim(d.STROPERATINGCAPACITY)    as Value,
           u2.STRUNITDESCRIPTION           as Units,
           'AvgPollutantCollectionRate'    as Id,
           d.STRPOLLUTANTCONCENTRATIONAVG  as Value,
           u3.STRUNITDESCRIPTION           as Units,
           'AvgTreatmentRate'              as Id,
           d.STRTREATMENTRATEAVG           as Value,
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
        on u4.STRUNITKEY = d.STRTREATMENTRATEUNIT
            and u4.STRUNITKEY <> @InvalidKey
    where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

    select trim(RunNumber)                        as RunNumber,
           trim(PollutantCollectionRate)          as PollutantCollectionRate,
           trim(TreatmentRate)                    as TreatmentRate,
           isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
    from (select s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1A                        as RunNumber,
                 s.STRPOLLUTANTCONCENTRATION1A           as PollutantCollectionRate,
                 s.STRTREATMENTRATE1A                    as TreatmentRate,
                 substring(r.STRCONFIDENTIALDATA, 33, 3) as ConfidentialParametersCode
          from dbo.ISMPREPORTPONDANDGAS s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1B                        as RunNumber,
                 s.STRPOLLUTANTCONCENTRATION1B           as HeatingValue,
                 s.STRTREATMENTRATE1B                    as EmissionRateVelocity,
                 substring(r.STRCONFIDENTIALDATA, 36, 3) as ConfidentialParametersCode
          from dbo.ISMPREPORTPONDANDGAS s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 s.STRRUNNUMBER1C                        as RunNumber,
                 s.STRPOLLUTANTCONCENTRATION1C           as HeatingValue,
                 s.STRTREATMENTRATE1C                    as EmissionRateVelocity,
                 substring(r.STRCONFIDENTIALDATA, 39, 3) as ConfidentialParametersCode
          from dbo.ISMPREPORTPONDANDGAS s
              inner join dbo.ISMPREPORTINFORMATION r
              on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER) t
    where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
    order by t.RunNumber;

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportPondTreatment', @params;

END;

GO
