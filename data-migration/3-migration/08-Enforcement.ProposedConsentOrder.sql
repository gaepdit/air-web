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
       'ProposedConsentOrder'                                 as ActionType,
       nullif
       (concat_ws(CHAR(13) + CHAR(10),
                  iif(e.DATCOTOUC is null, null, 'Date Proposed CO to UC: ' + format(e.DATCOTOUC, 'dd-MMM-yyyy')),
                  iif(e.DATCOTOPM is null, null, 'Date Proposed CO to PM: ' + format(e.DATCOTOPM, 'dd-MMM-yyyy'))),
        '')                                                   as Notes,
       iif(e.STRCOPROPOSED = 'True', 'Issued', 'Draft')       as Status,
       convert(date, e.DATCOPROPOSED)                         as IssueDate,
       convert(bit, 1)                                        as IsReportableAction,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(smallint, e.STRAFSCOPROPOSEDNUMBER)            as ActionNumber,
       e.ICIS_STATUSIND                                       as DataExchangeStatus,

       -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(bit, 0)                                        as ResponseRequested,
       null                                                   as ResponseReceived,
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
  and (e.STRCOTOUC = 'True' or e.STRCOTOPM = 'True' or e.STRCOPROPOSED = 'True')

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'ProposedConsentOrder';
