-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, FceId)
select newid()                                  as Id,
       'Added'                                  as What,
       isnull(ua.Id, ui.Id)                     as WhoId,
       isnull(a.DATMODIFINGDATE, i.DATMODIFINGDATE)
           at time zone 'Eastern Standard Time' as [When],
       'FceAuditPoint'                          as Discriminator,
       i.STRFCENUMBER                           as FceId

from AIRBRANCH.dbo.SSCPFCEMASTER i
    left join AIRBRANCH.dbo.AFSSSCPFCERECORDS a
        on a.STRFCENUMBER = i.STRFCENUMBER
    left join AirWeb.dbo.AspNetUsers ua
        on ua.IaipUserId = a.STRMODIFINGPERSON
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = i.STRMODIFINGPERSON

where i.IsDeleted = 'False' or
      i.IsDeleted is null

order by isnull(a.DATMODIFINGDATE, i.DATMODIFINGDATE), i.STRFCENUMBER;

-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, FceId)
select newid()                                  as Id,
       'Edited'                                 as What,
       ui.Id                                    as WhoId,
       i.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'FceAuditPoint'                          as Discriminator,
       i.STRFCENUMBER                           as FceId

from AIRBRANCH.dbo.SSCPFCEMASTER i
    inner join AIRBRANCH.dbo.AFSSSCPFCERECORDS a
        on a.STRFCENUMBER = i.STRFCENUMBER
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = i.STRMODIFINGPERSON

where (i.IsDeleted = 'False' or
       i.IsDeleted is null)
  and datediff(second, a.DATMODIFINGDATE, i.DATMODIFINGDATE) > 10

order by i.DATMODIFINGDATE, i.STRFCENUMBER;

select *
from AirWeb.dbo.AuditPoints
where Discriminator = 'FceAuditPoint';
