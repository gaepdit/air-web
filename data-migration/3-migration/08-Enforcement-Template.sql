use AirWeb
go

-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, Notes, Status, IssueDate, IsReportableAction,
--     
--     -- None (always null)
--     -- CurrentReviewerId, ReviewRequestedDate, ApprovedDate, ApprovedById, CanceledDate,
--     -- CreatedAt, CreatedById, DeletedAt, DeletedById, DeleteComments,
-- 
--     -- ReportableEnforcement
--     -- (AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder)
--     ActionNumber, DataExchangeStatus,
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
--     -- ConsentOrder
--     ReceivedFromFacility, ReceivedFromDirectorsOffice, OrderId, OrderNumber,
--     PenaltyAmount, PenaltyComment, StipulatedPenaltiesDefined,
-- 
--     -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
--     ResponseRequested, ResponseReceived, ResponseComment,
-- 
--     -- EnforcementAction (All)
--     UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                as Id,
       e.STRENFORCEMENTNUMBER                                 as CaseFileId,
       ''                                                     as ActionType,
       nullif(concat_ws(CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                        iif(null is null, null, 'Date LON to UC: ' + format(null, 'd-MMM-yyyy')),
                        AIRBRANCH.air.ReduceText(null)),
              '')                                             as Notes,
       iif(null = 'True', 'Issued', 'Draft')                  as Status,
       convert(date, null)                                    as IssueDate,
       convert(bit, null)                                     as IsReportableAction,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(smallint, null)                                as ActionNumber,
       e.ICIS_STATUSIND                                       as DataExchangeStatus,

       -- AdministrativeOrder, ConsentOrder
       null                                                   as ExecutedDate,

       -- AdministrativeOrder
       null                                                   as AppealedDate,

       -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
       convert(date, null)                                    as ResolvedDate,

       -- ConsentOrder
       null                                                   as ReceivedFromFacility,
       null                                                   as ReceivedFromDirectorsOffice,
       null                                                   as OrderId,
       null                                                   as OrderNumber,
       null                                                   as PenaltyAmount,
       null                                                   as PenaltyComment,
       null                                                   as StipulatedPenaltiesDefined,

       -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(bit, null)                                     as ResponseRequested,
       convert(date, null)                                    as ResponseReceived,
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
