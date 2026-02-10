insert into AirWeb.dbo.EnforcementActions
(
    -- EnforcementAction (All)
    Id, CaseFileId, ActionType, FacilityId, Notes, Status, IssueDate, IsReportableAction,

    -- ReportableEnforcement
    -- (AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder)
    ActionNumber, DataExchangeStatus, DataExchangeStatusDate,

    -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
    ResponseRequested, ResponseReceived, ResponseComment,

    -- EnforcementAction (All)
    CreatedAt, UpdatedAt, UpdatedById, IsDeleted)

select newid()                                                   as Id,
       e.STRENFORCEMENTNUMBER                                    as CaseFileId,
       'NovNfaLetter'                                            as ActionType,
       AIRBRANCH.iaip_facility.FormatAirsNumber(e.STRAIRSNUMBER) as FacilityId,

       nullif
       (concat_ws(CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                  nullif
                  (concat_ws(CHAR(13) + CHAR(10),
                             iif(e.DATNOVTOUC is null, null, 'Date NOV to UC: ' + format(e.DATNOVTOUC, 'dd-MMM-yyyy')),
                             iif(e.DATNOVTOPM is null, null, 'Date NOV to PM: ' + format(e.DATNOVTOPM, 'dd-MMM-yyyy'))),
                   ''),
                  AIRBRANCH.air.ReduceText(e.STRNOVCOMMENT)),
        '')                                                      as Notes,

       -- Only issued NOVs & NFAs with identical issue dates are migrated as combined NOV/NFA letters. 
       -- Draft letters are migrated as separate NOVs and NFAs. This might miss some draft NOV/NFA letters,
       -- but the number of those would be very small, and all would be open enforcement and so easily caught
       -- and corrected by staff.
       'Issued'                                                  as Status,

       convert(date, e.DATNOVSENT)                               as IssueDate,
       1                                                         as IsReportableAction,

       -- AdministrativeOrder, ConsentOrder, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(smallint, e.STRAFSNOVSENTNUMBER)                  as ActionNumber,
       e.ICIS_STATUSIND                                          as DataExchangeStatus,
       null                                                      as DataExchangeStatusDate,

       -- InformationalLetter, LetterOfNoncompliance, NoticeOfViolation, NovNfaLetter, ProposedConsentOrder
       convert(bit, e.STRNOVRESPONSERECEIVED)                    as ResponseRequested,
       convert(date, e.DATNOVRESPONSERECEIVED)                   as ResponseReceived,
       null                                                      as ResponseComment,

       -- EnforcementAction (All)
       e.DATNOVTOUC at time zone 'Eastern Standard Time'         as CreateAt,
       e.DATMODIFINGDATE at time zone 'Eastern Standard Time'    as UpdatedAt,
       um.Id                                                     as UpdatedById,
       isnull(e.IsDeleted, 0)                                    as IsDeleted

from AIRBRANCH.dbo.SSCP_AUDITEDENFORCEMENT e

    left join AirWeb.dbo.AspNetUsers um
        on um.IaipUserId = e.STRMODIFINGPERSON

where isnull(e.IsDeleted, 0) = 0
  and e.STRACTIONTYPE = 'CASEFILE'

  -- If NOV and NFA issued dates are the same, migrate as `NovNfaLetter`.
  -- (This syntax excludes all records where the NOV or NFA issue dates are 
  -- null, so no additional filtering is needed.)
  and convert(date, e.DATNOVSENT) = convert(date, e.DATNFALETTERSENT)

order by e.STRENFORCEMENTNUMBER;

select *
from AirWeb.dbo.EnforcementActions
where ActionType = 'NovNfaLetter';
