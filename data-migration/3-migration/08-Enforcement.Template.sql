use AirWeb
go

-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, Notes, Status, IssueDate,
--     
--     -- None (always null)
--     CurrentReviewerId, ReviewRequestedDate, ApprovedDate, ApprovedById, CanceledDate,
-- 
--     -- ReportableEnforcement
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
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted,
--     DeletedAt, DeletedById, DeleteComments)

select newid()                as Id,
       e.STRENFORCEMENTNUMBER as CaseFileId,
       null                   as ActionType,
       null                   as Notes,

       -- TODO: Figure out Enforcement Action Status
       null                   as Status,

       null                   as IssueDate,

       -- None (always null)
       null                   as CurrentReviewerId,
       null                   as ReviewRequestedDate,
       null                   as ApprovedDate,
       null                   as ApprovedById,
       null                   as CanceledDate,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       null                   as ActionNumber,
       null                   as DataExchangeStatus,

       -- AdministrativeOrder, ConsentOrder
       null                   as ExecutedDate,

       -- AdministrativeOrder
       null                   as AppealedDate,

       -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
       null                   as ResolvedDate,

       -- ConsentOrder
       null                   as ReceivedFromFacility,
       null                   as ReceivedFromDirectorsOffice,
       null                   as OrderId,
       null                   as OrderNumber,
       null                   as PenaltyAmount,
       null                   as PenaltyComment,
       null                   as StipulatedPenaltiesDefined,

       -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       null                   as ResponseRequested,
       null                   as ResponseReceived,
       null                   as ResponseComment,

       -- None (always null)
       null                   as CreatedAt,
       null                   as CreatedById,

       -- EnforcementAction (All)
       null                   as UpdatedAt,
       null                   as UpdatedById,
       null                   as IsDeleted,

       -- None (always null)
       null                   as DeletedAt,
       null                   as DeletedById,
       null                   as DeleteComments

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

where isnull(e.IsDeleted, 0) = convert(bit, 0)
