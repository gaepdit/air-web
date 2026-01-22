with cte as
         (select ID,
                 STRENFORCEMENTNUMBER,
                 DATMODIFINGDATE,
                 STRMODIFINGPERSON,
                 rn = row_number() over
                     (partition by STRENFORCEMENTNUMBER order by DATMODIFINGDATE, ID)
          from AIRBRANCH.dbo.SSCP_ENFORCEMENT)

-- insert
-- into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, CaseFileId)

select newid()                                  as Id,
       'Added'                                  as What,
       ui.Id                                    as WhoId,
       cte.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'CaseFileAuditPoint'                     as Discriminator,
       e.STRENFORCEMENTNUMBER                   as CaseFileId

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
    inner join cte
        on cte.STRENFORCEMENTNUMBER = e.STRENFORCEMENTNUMBER
        and cte.rn = 1
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = cte.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0
order by cte.DATMODIFINGDATE, e.STRENFORCEMENTNUMBER;

with cte as
         (select ID,
                 STRENFORCEMENTNUMBER,
                 DATMODIFINGDATE,
                 STRMODIFINGPERSON,
                 rn = row_number() over
                     (partition by STRENFORCEMENTNUMBER order by DATMODIFINGDATE, ID)
          from AIRBRANCH.dbo.SSCP_ENFORCEMENT)

-- insert
-- into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, CaseFileId)

select newid()                                  as Id,
       'Edited'                                 as What,
       ui.Id                                    as WhoId,
       cte.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'CaseFileAuditPoint'                     as Discriminator,
       e.STRENFORCEMENTNUMBER                   as CaseFileId

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e
    inner join cte
        on cte.STRENFORCEMENTNUMBER = e.STRENFORCEMENTNUMBER
        and cte.rn > 1
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = cte.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0
order by cte.DATMODIFINGDATE, e.STRENFORCEMENTNUMBER;

with cte as
         (select STRENFORCEMENTNUMBER,
                 DATENFORCEMENTFINALIZED,
                 row_number() over
                     (partition by STRENFORCEMENTNUMBER
                     order by max(DATMODIFINGDATE) desc) as rn,
                 max(DATMODIFINGDATE)                    as DATMODIFINGDATE
          from SSCP_ENFORCEMENT
          group by STRENFORCEMENTNUMBER, DATENFORCEMENTFINALIZED)

-- insert
-- into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], Discriminator, CaseFileId)

select newid()                                  as Id,
       'Closed'                                 as What,
       ui.Id                                    as WhoId,
       x.DATMODIFINGDATE
           at time zone 'Eastern Standard Time' as [When],
       'CaseFileAuditPoint'                     as Discriminator,
       x.STRENFORCEMENTNUMBER                   as CaseFileId

from cte
    cross apply (select top (1) *
                 from SSCP_ENFORCEMENT t
                 where t.STRENFORCEMENTNUMBER = cte.STRENFORCEMENTNUMBER
                   and t.DATMODIFINGDATE > cte.DATMODIFINGDATE) x
    inner join AirWeb.dbo.AspNetUsers ui
        on ui.IaipUserId = x.STRMODIFINGPERSON
    inner join SSCP_AUDITEDENFORCEMENT e
        on e.STRENFORCEMENTNUMBER = cte.STRENFORCEMENTNUMBER

where isnull(e.IsDeleted, 0) = 0
  and cte.rn = 2

order by x.DATMODIFINGDATE, x.STRENFORCEMENTNUMBER;

-- insert
-- into AirWeb.dbo.AuditPoints (Id, What, WhoId, [When], MoreInfo, Discriminator, CaseFileId)

select newid()                                  as Id,
       'Compliance Event Linked'                as What,
       uv.Id                                    as WhoId,
       v.CreatedDate
           at time zone 'Eastern Standard Time' as [When],
       concat('Event ID ', v.TrackingNumber)    as MoreInfo,
       'CaseFileAuditPoint'                     as Discriminator,
       e.STRENFORCEMENTNUMBER                   as CaseFileId

from SSCP_AUDITEDENFORCEMENT e
    inner join SSCP_EnforcementEvents v
        on v.EnforcementNumber = e.STRENFORCEMENTNUMBER
    inner join AirWeb.dbo.AspNetUsers uv
        on uv.IaipUserId = v.CreatedBy

where isnull(e.IsDeleted, 0) = 0
order by v.CreatedDate, v.EnforcementNumber, v.TrackingNumber;

select *
from AirWeb.dbo.AuditPoints
where Discriminator = 'CaseFileAuditPoint';
