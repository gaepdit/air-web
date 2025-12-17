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
