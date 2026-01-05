select 'SSCPACCS'                        as [table_name],
       count(*)                          as [count],
       count(distinct STRTRACKINGNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPACCS
union
select 'SSCPENFORCEMENTSTIPULATED' as [table_name],
       count(*)                    as [count],
       count(distinct concat(STRENFORCEMENTNUMBER, '_', STRENFORCEMENTKEY))
                                   as [count_distinct]
from AIRBRANCH.dbo.SSCPENFORCEMENTSTIPULATED
union
select 'SSCPFCE'                    as [table_name],
       count(*)                     as [count],
       count(distinct STRFCENUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPFCE
union
select 'SSCPFCEMASTER'              as [table_name],
       count(*)                     as [count],
       count(distinct STRFCENUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPFCEMASTER
union
select 'SSCPINSPECTIONS'                 as [table_name],
       count(*)                          as [count],
       count(distinct STRTRACKINGNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPINSPECTIONS
union
select 'SSCPITEMMASTER'                  as [table_name],
       count(*)                          as [count],
       count(distinct STRTRACKINGNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPITEMMASTER
union
select 'SSCPNOTIFICATIONS'               as [table_name],
       count(*)                          as [count],
       count(distinct STRTRACKINGNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPNOTIFICATIONS
union
select 'SSCPREPORTS'                     as [table_name],
       count(*)                          as [count],
       count(distinct STRTRACKINGNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPREPORTS
union
select 'SSCPTESTREPORTS'                 as [table_name],
       count(*)                          as [count],
       count(distinct STRTRACKINGNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCPTESTREPORTS
union
select 'SSCP_AUDITEDENFORCEMENT'            as [table_name],
       count(*)                             as [count],
       count(distinct STRENFORCEMENTNUMBER) as [count_distinct]
from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT
union
select 'SSCP_EnforcementEvents' as [table_name],
       count(*)                 as [count],
       count(distinct concat(EnforcementNumber, '_', TrackingNumber))
                                as [count_distinct]
from AIRBRANCH.dbo.SSCP_EnforcementEvents

---

select 'AspNetUsers' as [table_name],
       count(*)      as [count]
from AirWeb.dbo.AspNetUsers
union
select 'AuditPoints' as [table_name],
       count(*)      as [count]
from AirWeb.dbo.AuditPoints
union
select 'CaseFileComplianceEvents' as [table_name],
       count(*)                   as [count]
from AirWeb.dbo.CaseFileComplianceEvents
union
select 'CaseFiles' as [table_name],
       count(*)    as [count]
from AirWeb.dbo.CaseFiles
union
select 'Comments' as [table_name],
       count(*)   as [count]
from AirWeb.dbo.Comments
union
select 'ComplianceWork' as [table_name],
       count(*)         as [count]
from AirWeb.dbo.ComplianceWork
union
select 'EmailLogs' as [table_name],
       count(*)    as [count]
from AirWeb.dbo.EmailLogs
union
select 'EnforcementActionReviews' as [table_name],
       count(*)                   as [count]
from AirWeb.dbo.EnforcementActionReviews
union
select 'EnforcementActions' as [table_name],
       count(*)             as [count]
from AirWeb.dbo.EnforcementActions
union
select 'Fces'   as [table_name],
       count(*) as [count]
from AirWeb.dbo.Fces
union
select 'Lookups' as [table_name],
       count(*)  as [count]
from AirWeb.dbo.Lookups
union
select 'StipulatedPenalties' as [table_name],
       count(*)              as [count]
from AirWeb.dbo.StipulatedPenalties
union
select 'ViolationTypes' as [table_name],
       count(*)         as [count]
from AirWeb.dbo.ViolationTypes
;
