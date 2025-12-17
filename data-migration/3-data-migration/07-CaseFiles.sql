-- SET IDENTITY_INSERT AirWeb.dbo.CaseFiles ON;
--
-- insert into AirWeb.dbo.CaseFiles
-- (Id, FacilityId, ResponsibleStaffId, Notes, ViolationTypeCode, CaseFileStatus,
--  DiscoveryDate, DayZero, EnforcementDate, PollutantIds, AirPrograms, ActionNumber,
--  DataExchangeStatus, UpdatedAt, UpdatedById, IsDeleted, IsClosed, ClosedDate)

select e.STRENFORCEMENTNUMBER                                                                           as Id,
       AIRBRANCH.air.FormatAirsNumber(e.STRAIRSNUMBER)                                                  as FacilityId,
       ur.Id                                                                                            as ResponsibleStaffId,
       AIRBRANCH.air.ReduceText(e.STRGENERALCOMMENTS)                                                   as Notes,
       nullif(e.STRHPV, '')                                                                             as ViolationTypeCode,
       case
           when e.STRENFORCEMENTFINALIZED = 'True'
               then 'Closed'
           when e.STRCOEXECUTED = 'True' or e.STRAOEXECUTED = 'True'
               then 'SubjectToComplianceSchedule'
           when e.STRLONSENT = 'True' or e.STRNOVSENT = 'True' or e.STRCOPROPOSED = 'True'
               then 'Open'
           else 'Draft'
           end                                                                                          as CaseFileStatus,
       convert(date, e.DATDISCOVERYDATE)                                                                as DiscoveryDate,
       convert(date, e.DATDAYZERO)                                                                      as DayZero,
       convert(date, least(e.DATLONSENT, e.DATNOVSENT, e.DATCOPROPOSED, e.DATCOEXECUTED, e.DATAOEXECUTED))
                                                                                                        as EnforcementDate,

       -- Parse Pollutants as JSON from `STRPOLLUTANTS`
       isnull((select '[' + string_agg(quotename(lk.ICIS_POLLUTANT_CODE, '"'), ',') + ']'
               from (select distinct lk.ICIS_POLLUTANT_CODE
                     from string_split(e.STRPOLLUTANTS, ',') s
                         inner join AIRBRANCH.dbo.LK_ICIS_POLLUTANT lk
                             on lk.LGCY_POLLUTANT_CODE = substring(trim(s.value), 2, 10)) as lk), '[]') as PollutantIds,

       -- Parse Air Programs as JSON from `STRPOLLUTANTS`
       isnull((select '[' + string_agg(quotename(ap.Program, '"'), ',') + ']'
               from (select distinct case left(trim(s.value), 1)
                                         when '0' then 'SIP'
                                         when '1' then 'FederalSIP'
                                         when '3' then 'NonFederalSIP'
                                         when '4' then 'CfcTracking'
                                         when '6' then 'PSD'
                                         when '7' then 'NSR'
                                         when '8' then 'NESHAP'
                                         when '9' then 'NSPS'
                                         when 'F' then 'FESOP'
                                         when 'A' then 'AcidPrecipitation'
                                         when 'I' then 'NativeAmerican'
                                         when 'M' then 'MACT'
                                         when 'V' then 'TitleV'
                                         when 'R' then 'RMP'
                                         end as Program
                     from string_split(e.STRPOLLUTANTS, ',') s) ap
               where ap.Program is not null), '[]')                                                     as AirPrograms,

       convert(smallint, e.STRAFSKEYACTIONNUMBER)                                                       as ActionNumber,
       e.ICIS_STATUSIND                                                                                 as DataExchangeStatus,

       e.DATMODIFINGDATE at time zone 'Eastern Standard Time'                                           as UpdatedAt,
       um.Id                                                                                            as UpdatedById,
       isnull(e.IsDeleted, 0)                                                                           as IsDeleted,
       convert(bit, e.STRENFORCEMENTFINALIZED)                                                          as IsClosed,
       convert(date, e.DATENFORCEMENTFINALIZED)                                                         as ClosedDate

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers ur
        on ur.IaipUserId = convert(int, nullif(e.NUMSTAFFRESPONSIBLE, 0))
    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0

order by e.STRENFORCEMENTNUMBER;

SET IDENTITY_INSERT AirWeb.dbo.CaseFiles OFF;

select *
from AirWeb.dbo.CaseFiles;
