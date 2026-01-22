-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, FacilityId, Notes, Status, IssueDate, IsReportableAction,
-- 
--     -- ReportableEnforcement
--     -- (AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder)
--     ActionNumber, DataExchangeStatus, DataExchangeStatusDate,
-- 
--     -- AdministrativeOrder, ConsentOrder
--     ExecutedDate,
-- 
--     -- AdministrativeOrder
--     AppealedDate,
-- 
--     -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
--     ResolvedDate,
-- 
--     -- EnforcementAction (All)
--     CreatedAt, UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                   as Id,
       e.STRENFORCEMENTNUMBER                                    as CaseFileId,
       'AdministrativeOrder'                                     as ActionType,
       AIRBRANCH.iaip_facility.FormatAirsNumber(e.STRAIRSNUMBER) as FacilityId,

       AIRBRANCH.air.ReduceText(e.STRAOCOMMENT)                  as Notes,
       'Issued'                                                  as Status,
       convert(date, e.DATAOEXECUTED)                            as IssueDate,
       1                                                         as IsReportableAction,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(smallint, e.STRAFSAOTOAGNUMBER)                   as ActionNumber,
       e.ICIS_STATUSIND                                          as DataExchangeStatus,
       null                                                      as DataExchangeStatusDate,

       -- AdministrativeOrder, ConsentOrder
       convert(date, e.DATAOEXECUTED)                            as ExecutedDate,

       -- AdministrativeOrder
       convert(date, e.DATAOAPPEALED)                            as AppealedDate,

       -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
       convert(date, e.DATAORESOLVED)                            as ResolvedDate,

       -- EnforcementAction (All)
       null                                                      as CreatedAt,
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       isnull(e.IsDeleted, 0)                                    as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'
  and e.STRAOEXECUTED = 'True'

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'AdministrativeOrder';
