-- insert into AirWeb.dbo.EnforcementActions
-- (
--     -- EnforcementAction (All)
--     Id, CaseFileId, ActionType, Notes, Status, IssueDate, IsReportableAction,
--
--     -- ReportableEnforcement
--     -- (AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder)
--     ActionNumber, DataExchangeStatus,
--
--     -- AdministrativeOrder, ConsentOrder
--     ExecutedDate,
--
--     -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
--     ResolvedDate,
--
--     -- ConsentOrder
--     ReceivedFromFacility, ReceivedFromDirectorsOffice, OrderId,
--     PenaltyAmount, PenaltyComment, StipulatedPenaltiesDefined,
--
--     -- EnforcementAction (All)
--     UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                            as Id,
       e.STRENFORCEMENTNUMBER                                             as CaseFileId,
       'ConsentOrder'                                                     as ActionType,
       AIRBRANCH.air.ReduceText(e.STRCOCOMMENT)                           as Notes,
       iif(e.STRCOEXECUTED = 'True', 'Issued', 'Draft')                   as Status,
       convert(date, e.DATCOEXECUTED)                                     as IssueDate,
       convert(bit, 1)                                                    as IsReportableAction,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(smallint, e.STRAFSCOEXECUTEDNUMBER)                        as ActionNumber,
       e.ICIS_STATUSIND                                                   as DataExchangeStatus,

       -- AdministrativeOrder, ConsentOrder
       convert(date, e.DATCOEXECUTED)                                     as ExecutedDate,

       -- AdministrativeOrder, ConsentOrder, LetterOfNoncompliance
       convert(date, e.DATCORESOLVED)                                     as ResolvedDate,

       -- ConsentOrder
       convert(date, e.DATCORECEIVEDFROMCOMPANY)                          as ReceivedFromFacility,
       convert(date, e.DATCORECEIVEDFROMDIRECTOR)                         as ReceivedFromDirectorsOffice,
       convert(smallint, nullif(trim('EPD-AQC-' from e.STRCONUMBER), '')) as OrderId,
       convert(decimal(18, 2), e.STRCOPENALTYAMOUNT)                      as PenaltyAmount,
       AIRBRANCH.air.ReduceText(e.STRCOPENALTYAMOUNTCOMMENTS)             as PenaltyComment,
       convert(bit, iif(s.STRENFORCEMENTNUMBER is null, 0, 1))            as StipulatedPenaltiesDefined,

       -- EnforcementAction (All)
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time'             as UpdatedAt,
       um.Id                                                              as UpdatedById,
       isnull(e.IsDeleted, convert(bit, 0))                               as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AIRBRANCH.dbo.SSCPENFORCEMENTSTIPULATED s
        on s.STRENFORCEMENTNUMBER = e.STRENFORCEMENTNUMBER
        and s.STRSTIPULATEDPENALTY <> '0'

    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = convert(bit, 0)
  and e.STRACTIONTYPE = 'CASEFILE'
  and (e.STRCORECEIVEDFROMCOMPANY = 'True' or e.STRCORECEIVEDFROMDIRECTOR = 'True' or e.STRCOEXECUTED = 'True')

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'ConsentOrder';
