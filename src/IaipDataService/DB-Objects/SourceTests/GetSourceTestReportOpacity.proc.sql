USE AIRBRANCH
GO

CREATE OR ALTER PROCEDURE air.GetSourceTestReportOpacity
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for an "Opacity" type stack test.

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
           s.STRCOMPLIANCESTATUS           as ComplianceStatus,
           iif(d.STROPACITYSTANDARD = '30', '30-minute average',
               'Highest 6-minute average') as OpacityStandard,
           iif(trim(d.STROPACITYTESTDURATION) = '', '',
               concat(trim(d.STROPACITYTESTDURATION), ' minutes'))
                                           as TestDuration,
           u1.STRUNITDESCRIPTION           as MaxOperatingCapacityUnits,
           u2.STRUNITDESCRIPTION           as OperatingCapacityUnits,
           iif(r.STRDOCUMENTTYPE = '015', '',
               u3.STRUNITDESCRIPTION)      as AllowableEmissionRateUnits
    from dbo.ISMPREPORTINFORMATION r
        inner join dbo.ISMPREPORTOPACITY d
            on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
        left join dbo.LOOKUPISMPCOMPLIANCESTATUS s
            on s.STRCOMPLIANCEKEY = r.STRCOMPLIANCESTATUS
        left join dbo.LOOKUPUNITS u1
            on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
            and u1.STRUNITKEY <> @InvalidKey
        left join dbo.LOOKUPUNITS u2
            on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
            and u2.STRUNITKEY <> @InvalidKey
        left join dbo.LOOKUPUNITS u3
            on u3.STRUNITKEY = d.STRALLOWABLEEMISSIONRATEUNIT
            and u3.STRUNITKEY <> @InvalidKey
    where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

    select RunNumber,
           trim(MaxOperatingCapacity)      as MaxOperatingCapacity,
           trim(OperatingCapacity)         as OperatingCapacity,
           trim(AllowableEmissionRate)     as AllowableEmissionRate,
           trim(Opacity)                   as Opacity,
           trim(AccumulatedEmissionTime)   as AccumulatedEmissionTime,
           trim(EquipmentItem)             as EquipmentItem,
           iif(ConfidentialParametersCode = '00', '',
               ConfidentialParametersCode) as ConfidentialParametersCode
    from (
        -- Method 22
        select d.STRREFERENCENUMBER,
               1                            as RunNumber,
               d.STRMAXOPERATINGCAPACITY1A  as MaxOperatingCapacity,
               d.STROPERATINGCAPACITY1A     as OperatingCapacity,
               d.STRALLOWABLEEMISSIONRATE22 as AllowableEmissionRate,
               ''                           as Opacity,
               d.STRACCUMULATEDEMISSIONTIME as AccumulatedEmissionTime,
               ''                           as EquipmentItem,
               concat('0',
                      substring(r.STRCONFIDENTIALDATA, 26, 1),
                      substring(r.STRCONFIDENTIALDATA, 27, 1),
                      '0')
                                            as ConfidentialParametersCode
        from dbo.ISMPREPORTOPACITY d
            inner join dbo.ISMPREPORTINFORMATION r
                on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
                and r.STRDOCUMENTTYPE = '015'
        union
        -- Method 9 (Single and Multi)
        select d.STRREFERENCENUMBER,
               1,
               d.STRMAXOPERATINGCAPACITY1A,
               d.STROPERATINGCAPACITY1A,
               d.STRALLOWABLEEMISSIONRATE1A,
               d.STROPACITYPOINTA,
               '',
               d.STREQUIPMENTITEM1,
               iif(r.STRDOCUMENTTYPE = '016',
                   concat('0', -- Method 9 Single
                          substring(r.STRCONFIDENTIALDATA, 26, 1),
                          substring(r.STRCONFIDENTIALDATA, 27, 1),
                          '0'),
                   concat('0', -- Method 9 Multi
                          substring(r.STRCONFIDENTIALDATA, 26, 1),
                          substring(r.STRCONFIDENTIALDATA, 32, 1),
                          substring(r.STRCONFIDENTIALDATA, 52, 1)))
        from dbo.ISMPREPORTOPACITY d
            inner join dbo.ISMPREPORTINFORMATION r
                on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
                and r.STRDOCUMENTTYPE in ('014', '016')
        union
        -- Method 9 Multi follows
        select d.STRREFERENCENUMBER,
               2,
               d.STRMAXOPERATINGCAPACITY2A,
               d.STROPERATINGCAPACITY2A,
               d.STRALLOWABLEEMISSIONRATE2A,
               d.STROPACITYPOINTB,
               '',
               d.STREQUIPMENTITEM2,
               concat('0',
                      substring(r.STRCONFIDENTIALDATA, 27, 1),
                      substring(r.STRCONFIDENTIALDATA, 33, 1),
                      substring(r.STRCONFIDENTIALDATA, 53, 1))
        from dbo.ISMPREPORTOPACITY d
            inner join dbo.ISMPREPORTINFORMATION r
                on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
                and r.STRDOCUMENTTYPE in ('014', '016')
        union
        select d.STRREFERENCENUMBER,
               3,
               d.STRMAXOPERATINGCAPACITY3A,
               d.STROPERATINGCAPACITY3A,
               d.STRALLOWABLEEMISSIONRATE3A,
               d.STROPACITYPOINTC,
               '',
               d.STREQUIPMENTITEM3,
               concat('0',
                      substring(r.STRCONFIDENTIALDATA, 28, 1),
                      substring(r.STRCONFIDENTIALDATA, 34, 1),
                      substring(r.STRCONFIDENTIALDATA, 54, 1))
        from dbo.ISMPREPORTOPACITY d
            inner join dbo.ISMPREPORTINFORMATION r
                on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
                and r.STRDOCUMENTTYPE in ('014', '016')
        union
        select d.STRREFERENCENUMBER,
               4,
               d.STRMAXOPERATINGCAPACITY4A,
               d.STROPERATINGCAPACITY4A,
               d.STRALLOWABLEEMISSIONRATE4A,
               d.STROPACITYPOINTD,
               '',
               d.STREQUIPMENTITEM4,
               concat('0',
                      substring(r.STRCONFIDENTIALDATA, 25, 1),
                      substring(r.STRCONFIDENTIALDATA, 35, 1),
                      substring(r.STRCONFIDENTIALDATA, 55, 1))
        from dbo.ISMPREPORTOPACITY d
            inner join dbo.ISMPREPORTINFORMATION r
                on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
                and r.STRDOCUMENTTYPE in ('014', '016')
        union
        select d.STRREFERENCENUMBER,
               5,
               d.STRMAXOPERATINGCAPACITY5A,
               d.STROPERATINGCAPACITY5A,
               d.STRALLOWABLEEMISSIONRATE5A,
               d.STROPACITYPOINTE,
               '',
               d.STREQUIPMENTITEM5,
               concat('0',
                      substring(r.STRCONFIDENTIALDATA, 30, 1),
                      substring(r.STRCONFIDENTIALDATA, 36, 1),
                      substring(r.STRCONFIDENTIALDATA, 56, 1))
        from dbo.ISMPREPORTOPACITY d
            inner join dbo.ISMPREPORTINFORMATION r
                on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
                and r.STRDOCUMENTTYPE in ('014', '016')) t
    where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
      and not (t.RunNumber > 1 and trim(t.Opacity) in ('', 'N/A'))
    order by t.RunNumber;

END;

GO
