use AirWeb
go

-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, Notes, Status, CurrentReviewerId,
--     ReviewRequestedDate, ApprovedDate, ApprovedById, IssueDate, CanceledDate,
-- 
--     -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
--     ResolvedDate,
-- 
--     -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
--     ResponseRequested, ResponseReceived, ResponseComment,
-- 
--     -- EnforcementAction (All)
--     CreatedAt, CreatedById, UpdatedAt, UpdatedById, IsDeleted,
--     DeletedAt, DeletedById, DeleteComments)

select newid()                                    as Id,
       e.STRENFORCEMENTNUMBER                     as CaseFileId,
       'LetterOfNoncompliance'                    as ActionType,

       -- TODO: Add "Date to UC", etc. to Notes.
       AIRBRANCH.air.ReduceText(e.STRLONCOMMENTS) as Notes,

       -- TODO: Figure out Enforcement Action Status
       null                                       as Status,

       null                                       as CurrentReviewerId,
       null                                       as ReviewRequestedDate,
       null                                       as ApprovedDate,
       null                                       as ApprovedById,
       convert(date, DATLONSENT)                  as IssueDate,
       null                                       as CanceledDate,

       convert(date, DATLONRESOLVED)              as ResolvedDate,
       convert(bit, 0)                            as ResponseRequested,
       null                                       as ResponseReceived,
       null                                       as ResponseComment,

       
       
       
       null                                       as CreatedAt,
       null                                       as CreatedById,
       null                                       as UpdatedAt,
       null                                       as UpdatedById,
       null                                       as IsDeleted,
       null                                       as DeletedAt,
       null                                       as DeletedById,
       null                                       as DeleteComments

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers ur
        on ur.AirbranchUserId = convert(int, nullif(e.NUMSTAFFRESPONSIBLE, 0))
    inner join AirWeb.dbo.AspNetUsers um
        on um.AirbranchUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = convert(bit, 0)
  and e.STRACTIONTYPE = 'LON';

select *
from AirWeb.dbo.EnforcementActions;
