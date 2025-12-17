-- insert into AirWeb.dbo.StipulatedPenalties (Id, ConsentOrderId, Amount, ReceivedDate, Notes, CreatedAt, CreatedById,
--                                             IsDeleted)

select newid()                                                  as Id,
       a.Id                                                     as ConsentOrderId,
       convert(decimal(18, 2), trim('$' from replace(s.STRSTIPULATEDPENALTY, ',', '')))
                                                                as Amount,
       convert(date, s.DATMODIFINGDATE)                         as ReceivedDate,
       AIRBRANCH.air.ReduceText(s.STRSTIPULATEDPENALTYCOMMENTS) as Notes,
       s.DATMODIFINGDATE at time zone 'Eastern Standard Time'   as CreatedAt,
       um.Id                                                    as CreatedById,
       convert(bit, 0)                                          as IsDeleted
from AIRBRANCH.dbo.SSCPENFORCEMENTSTIPULATED s
    inner join AirWeb.dbo.EnforcementActions a
        on a.CaseFileId = s.STRENFORCEMENTNUMBER
    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = s.STRMODIFINGPERSON
where s.STRSTIPULATEDPENALTY <> '0'
  and a.ActionType = 'ConsentOrder'

select *
from AirWeb.dbo.StipulatedPenalties;
