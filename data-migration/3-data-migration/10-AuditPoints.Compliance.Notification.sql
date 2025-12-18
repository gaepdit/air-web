-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, WorkEntryId)
select newid()                                  as Id,
       'Added'                                  as What,
       ui.Id                                    as WhoId,
       i.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'WorkEntryAuditPoint' as Discriminator,
       i.STRTRACKINGNUMBER   as WorkEntryId

from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = i.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '05'

order by i.DATMODIFINGDATE, i.STRTRACKINGNUMBER;

-- insert into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, WorkEntryId)
select newid()                                  as Id,
       'Edited'                                 as What,
       ud.Id                                    as WhoId,
       d.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'WorkEntryAuditPoint' as Discriminator,
       i.STRTRACKINGNUMBER   as WorkEntryId

from AIRBRANCH.dbo.SSCPITEMMASTER i
    inner join AIRBRANCH.dbo.SSCPNOTIFICATIONS d
        on d.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
    inner join AirWeb.dbo.AspNetUsers ud
        on ud.IaipUserId = d.STRMODIFINGPERSON

where i.STRDELETE is null
  and i.STREVENTTYPE = '05'
  and datediff(second, i.DATMODIFINGDATE, d.DATMODIFINGDATE) > 10

order by d.DATMODIFINGDATE, i.STRTRACKINGNUMBER;

select *
from AirWeb.dbo.AuditPoints
where Discriminator = 'WorkEntryAuditPoint';
