-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, ComplianceWorkId)
select newid()                                  as Id,
       'Added'                                  as What,
       isnull(ua.Id, ui.Id)                     as WhoId,
       isnull(a.DATMODIFINGDATE, i.DATMODIFINGDATE)
           at time zone 'Eastern Standard Time' as [When],
       'ComplianceWorkAuditPoint'               as Discriminator,
       i.STRTRACKINGNUMBER                      as ComplianceWorkId

from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    left join AIRBRANCH.dbo.AFSISMPRECORDS a
        on a.STRREFERENCENUMBER = d.STRREFERENCENUMBER
    left join AirWeb.dbo.AspNetUsers ua
        on ua.IaipUserId = a.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = i.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '03'

order by nullif(a.DATMODIFINGDATE, i.DATMODIFINGDATE), i.STRTRACKINGNUMBER;

-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, ComplianceWorkId)
select newid()                                  as Id,
       'Edited'                                 as What,
       ui.Id                                    as WhoId,
       i.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'ComplianceWorkAuditPoint'               as Discriminator,
       i.STRTRACKINGNUMBER                      as ComplianceWorkId

from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPTESTREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    inner join AIRBRANCH.dbo.AFSISMPRECORDS a
        on a.STRREFERENCENUMBER = d.STRREFERENCENUMBER
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = i.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '03'
  and datediff(second, a.DATMODIFINGDATE, d.DATMODIFINGDATE) > 10

order by i.DATMODIFINGDATE, i.STRTRACKINGNUMBER desc;

select *
from AirWeb.dbo.AuditPoints
where Discriminator = 'ComplianceWorkAuditPoint';
