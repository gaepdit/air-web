-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, FacilityId, Notes, Status, IssueDate, IsReportableAction,
-- 
--     -- EnforcementAction (All)
--     CreatedAt, UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                   as Id,
       e.STRENFORCEMENTNUMBER                                    as CaseFileId,
       'NoFurtherActionLetter'                                   as ActionType,
       AIRBRANCH.iaip_facility.FormatAirsNumber(e.STRAIRSNUMBER) as FacilityId,

       nullif
       (concat_ws(CHAR(13) + CHAR(10),
                  iif(e.DATNFATOUC is null, null, 'Date NFA to UC: ' + format(e.DATNFATOUC, 'dd-MMM-yyyy')),
                  iif(e.DATNFATOPM is null, null, 'Date NFA to PM: ' + format(e.DATNFATOPM, 'dd-MMM-yyyy'))),
        '')                                                      as Notes,
       iif(e.STRNFALETTERSENT = 'True', 'Issued', 'Draft')       as Status,
       convert(date, e.DATNFALETTERSENT)                         as IssueDate,
       0                                                         as IsReportableAction,

       -- EnforcementAction (All)
       e.DATNFATOUC at time zone 'Eastern Standard Time'         as CreateAt,
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       isnull(e.IsDeleted, 0)                                    as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNFATOUC = 'True' or e.STRNFATOPM = 'True' or e.STRNFALETTERSENT = 'True')

  -- If NOV and NFA issued dates are the same, migrate as `NovNfaLetter`.
  and (e.DATNOVSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'NoFurtherActionLetter';
