-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, Notes, Status, IssueDate, IsReportableAction,
--     
--     -- EnforcementAction (All)
--     UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                as Id,
       e.STRENFORCEMENTNUMBER                                 as CaseFileId,
       'NoFurtherActionLetter'                                as ActionType,
       nullif
       (concat_ws(CHAR(13) + CHAR(10),
                  iif(e.DATNFATOUC is null, null, 'Date NFA to UC: ' + format(e.DATNFATOUC, 'dd-MMM-yyyy')),
                  iif(e.DATNFATOPM is null, null, 'Date NFA to PM: ' + format(e.DATNFATOPM, 'dd-MMM-yyyy'))),
        '')                                                   as Notes,
       iif(e.STRNFALETTERSENT = 'True', 'Issued', 'Draft')    as Status,
       convert(date, e.DATNFALETTERSENT)                      as IssueDate,
       convert(bit, 0)                                        as IsReportableAction,

       -- EnforcementAction (All)
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time' as UpdatedAt,
       um.Id                                                  as UpdatedById,
       isnull(e.IsDeleted, convert(bit, 0))                   as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers um
        on um.AirBranchUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = convert(bit, 0)
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNFATOUC = 'True' or e.STRNFATOPM = 'True' or e.STRNFALETTERSENT = 'True')

  -- If NOV and NFA issued dates are the same, migrate as `NovNfaLetter`.
  and (e.DATNOVSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'NoFurtherActionLetter';
