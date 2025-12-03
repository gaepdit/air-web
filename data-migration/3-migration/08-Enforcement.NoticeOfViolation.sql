-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, Notes, Status, IssueDate, IsReportableAction,
--     
--     -- ReportableEnforcement
--     -- (AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder)
--     ActionNumber, DataExchangeStatus,
-- 
--     -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
--     ResponseRequested, ResponseReceived, ResponseComment,
-- 
--     -- EnforcementAction (All)
--     UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                as Id,
       e.STRENFORCEMENTNUMBER                                 as CaseFileId,
       'NoticeOfViolation'                                    as ActionType,
       nullif
       (concat_ws(CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                  nullif
                  (concat_ws(CHAR(13) + CHAR(10),
                             iif(e.DATNOVTOUC is null, null, 'Date NOV to UC: ' + format(e.DATNOVTOUC, 'dd-MMM-yyyy')),
                             iif(e.DATNOVTOPM is null, null, 'Date NOV to PM: ' + format(e.DATNOVTOPM, 'dd-MMM-yyyy'))),
                   ''),
                  AIRBRANCH.air.ReduceText(e.STRNOVCOMMENT)),
        '')                                                   as Notes,
       iif(e.STRNOVSENT = 'True', 'Issued', 'Draft')          as Status,
       convert(date, e.DATNOVSENT)                            as IssueDate,
       convert(bit, 1)                                        as IsReportableAction,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(smallint, e.STRAFSNOVSENTNUMBER)               as ActionNumber,
       e.ICIS_STATUSIND                                       as DataExchangeStatus,

       -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(bit, e.STRNOVRESPONSERECEIVED)                 as ResponseRequested,
       convert(date, e.DATNOVRESPONSERECEIVED)                as ResponseReceived,
       null                                                   as ResponseComment,

       -- EnforcementAction (All)
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time' as UpdatedAt,
       um.Id                                                  as UpdatedById,
       isnull(e.IsDeleted, convert(bit, 0))                   as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = convert(bit, 0)
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRNOVTOUC = 'True' or e.STRNOVTOPM = 'True' or e.STRNOVSENT = 'True')

  -- If NOV and NFA issued dates are the same, migrate as `NovNfaLetter`.
  and (e.DATNFALETTERSENT is null or convert(date, e.DATNOVSENT) <> convert(date, e.DATNFALETTERSENT))

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'NoticeOfViolation';
