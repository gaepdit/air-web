USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestReportRata
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "RATA" type stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                          as ControlEquipmentInfo,
           d.STRAPPLICABLESTANDARD        as ApplicableStandard,
           p.STRPOLLUTANTDESCRIPTION      as Diluent,
           u.STRUNITDESCRIPTION           as Units,
           d.STRACCURACYCHOICE            as RelativeAccuracyCode,
           d.STRRELATIVEACCURACYPERCENT   as RelativeAccuracyPercent,
           d.STRACCURACYREQUIREDPERCENT   as RelativeAccuracyRequiredPercent,
           d.STRACCURACYREQUIREDSTATEMENT as RelativeAccuracyRequiredLabel,
           case
               when r.STRCOMPLIANCESTATUS in ('02', '03') then 'Pass'
               when r.STRCOMPLIANCESTATUS in ('05') then 'Fail'
               else 'N/A'
               end as ComplianceStatus
    from dbo.ISMPREPORTINFORMATION r
        inner join dbo.ISMPREPORTRATA d
            on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
        left join dbo.LOOKUPUNITS u
            on u.STRUNITKEY = d.STRRATAUNITS
        left join dbo.LOOKUPPOLLUTANTS p
            on p.STRPOLLUTANTCODE = d.STRDILUENT
    where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

    select trim(RunNumber)                 as RunNumber,
           trim(ReferenceMethod)           as ReferenceMethod,
           trim(Cms)                       as Cms,
           Omitted,
           IIF(ConfidentialParametersCode = '0', '',
               ConfidentialParametersCode) as ConfidentialParametersCode
    from (select s.STRREFERENCENUMBER,
                 '1'                   as RunNumber,
                 s.STRREFERENCEMETHOD1 as ReferenceMethod,
                 s.STRCMS1             as Cms,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 1, 1))
                                       as Omitted,
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 29, 1),
                        substring(r.STRCONFIDENTIALDATA, 41, 1))
                                       as ConfidentialParametersCode
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '2',
                 s.STRREFERENCEMETHOD2,
                 s.STRCMS2,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 2, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 30, 1),
                        substring(r.STRCONFIDENTIALDATA, 42, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '3',
                 s.STRREFERENCEMETHOD3,
                 s.STRCMS3,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 3, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 31, 1),
                        substring(r.STRCONFIDENTIALDATA, 43, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '4',
                 s.STRREFERENCEMETHOD4,
                 s.STRCMS4,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 4, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 32, 1),
                        substring(r.STRCONFIDENTIALDATA, 44, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '5',
                 s.STRREFERENCEMETHOD5,
                 s.STRCMS5,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 5, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 33, 1),
                        substring(r.STRCONFIDENTIALDATA, 45, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '6',
                 s.STRREFERENCEMETHOD6,
                 s.STRCMS6,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 6, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 34, 1),
                        substring(r.STRCONFIDENTIALDATA, 46, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '7',
                 s.STRREFERENCEMETHOD7,
                 s.STRCMS7,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 7, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 35, 1),
                        substring(r.STRCONFIDENTIALDATA, 47, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '8',
                 s.STRREFERENCEMETHOD8,
                 s.STRCMS8,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 8, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 36, 1),
                        substring(r.STRCONFIDENTIALDATA, 48, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '9',
                 s.STRREFERENCEMETHOD9,
                 s.STRCMS9,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 9, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 37, 1),
                        substring(r.STRCONFIDENTIALDATA, 49, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '10',
                 s.STRREFERENCEMETHOD10,
                 s.STRCMS10,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 10, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 38, 1),
                        substring(r.STRCONFIDENTIALDATA, 50, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '11',
                 s.STRREFERENCEMETHOD11,
                 s.STRCMS11,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 11, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 39, 1),
                        substring(r.STRCONFIDENTIALDATA, 51, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
          union
          select s.STRREFERENCENUMBER,
                 '12',
                 s.STRREFERENCEMETHOD12,
                 s.STRCMS12,
                 convert(bit, substring(s.STRRUNSINCLUDEDKEY, 12, 1)),
                 concat('0',
                        substring(r.STRCONFIDENTIALDATA, 40, 1),
                        substring(r.STRCONFIDENTIALDATA, 52, 1))
          from dbo.ISMPREPORTRATA s
              inner join dbo.ISMPREPORTINFORMATION r
                  on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER) t
    where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
      and not (trim(t.ReferenceMethod) in ('', 'N/A')
        and trim(t.Cms) in ('', 'N/A'))
    order by convert(int, t.RunNumber);

END;

GO
