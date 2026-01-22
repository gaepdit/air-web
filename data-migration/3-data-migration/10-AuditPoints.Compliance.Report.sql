-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, ComplianceWorkId)
select newid()                                  as Id,
       'Added'                                  as What,
       isnull(ua.Id, ui.Id)                     as WhoId,
       isnull(a.DATMODIFINGDATE, i.DATMODIFINGDATE)
           at time zone 'Eastern Standard Time' as [When],
       'ComplianceWorkAuditPoint' as Discriminator,
       i.STRTRACKINGNUMBER        as ComplianceWorkId

from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.AFSSSCPRECORDS a
        on a.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    left join AirWeb.dbo.AspNetUsers ua
        on ua.IaipUserId = a.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = i.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '01'

order by i.DATMODIFINGDATE, i.STRTRACKINGNUMBER;

-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, ComplianceWorkId)
select newid()                                  as Id,
       'Edited'                                 as What,
       ud.Id                                    as WhoId,
       d.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'ComplianceWorkAuditPoint' as Discriminator,
       i.STRTRACKINGNUMBER        as ComplianceWorkId

from AIRBRANCH.dbo.SSCPITEMMASTER i
    left join AIRBRANCH.dbo.AFSSSCPRECORDS a
        on a.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    inner join AIRBRANCH.dbo.SSCPREPORTS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    inner join AirWeb.dbo.AspNetUsers ud
        on ud.IaipUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '01'
  and datediff(second, isnull(a.DATMODIFINGDATE, i.DATMODIFINGDATE), d.DATMODIFINGDATE) > 10

order by d.DATMODIFINGDATE, i.STRTRACKINGNUMBER;

select *
from AirWeb.dbo.AuditPoints
where Discriminator = 'ComplianceWorkAuditPoint';
