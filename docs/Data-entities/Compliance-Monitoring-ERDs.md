# Compliance Monitoring ERDs

## Compliance Work Entities

```mermaid
erDiagram
    FAC["Facility 🛩️"] {
        string FacilityId PK
    }

    TST["Source Test Report 🛩️"] {
        int ReferenceNumber PK
    }

    FCE["FCE ⚓"] {
        int Id PK
    }

    WRK["Compliance Work ⚓"] {
        int Id PK
        bool IsComplianceEvent
    }

    CME["Compliance Event"]
    STR["Source Test Review"]
    ACC["ACC"]
    INS["Inspection"]
    RMP["RMP Inspection"]
    REP["Report"]
    NOT["Notification"]
    REV["Permit Revocation"]
    STR |o--|| CME: "is a type of"
    ACC |o--|| CME: "is a type of"
    BINS |o--|| CME: "is a type of"
    INS |o--|| CME: "is a type of"
    REP |o--|| CME: "is a type of"
    CME |o--|| WRK: "is a subset of"
    RMP |o--|| CME: "is a type of"
    NOT |o--|| WRK: "is a type of"
    REV |o--|| WRK: "is a type of"
    STR |o--|| TST: "evaluates"
    TST }o--|| FAC: "is conducted at"
    WRK }o--|| FAC: "is entered for"
    FCE }o--|| FAC: "is completed for"

```

## Base ERD

```mermaid
erDiagram
    FAC["Facility"] {
        string facilityId PK
    }

    WRK["Compliance Work"] {
        int Id PK
        string FacilityId FK
        enum ComplianceWorkType
        string ResponsibleStaffId FK
        bool Closed
        date ClosedDate
        string ClosedByStaffId FK
        date AcknowledgmentLetterDate
        string Notes
        bool IsComplianceEvent
        enum EpaDxStatus
    }

    STF["Staff"] {
        string Id PK
    }

    CMT["Comment"] {
        int Id PK
        int FceId FK
    }

    WRK }o--|| FAC: "is entered for"
    STF ||--o{ WRK: "enters"
    CMT }o--|| WRK: "comments on"

```

### Derived event types

- Report
- Inspection
- Performance Tests
- Annual Compliance Certification
- Notification
- RMP Inspection

## ACC columns

```mermaid
erDiagram
    ACC {
        date ReceivedDate
        int AccReportingYear
        date Postmarked
        bool PostmarkedOnTime
        bool SignedByRo
        bool OnCorrectForms
        bool IncludesAllTvConditions
        bool CorrectlyCompleted
        bool ReportsDeviations
        bool IncludesPreviouslyUnreportedDeviations
        bool ReportsAllKnownDeviations
        bool ResubmittalRequired
        bool EnforcementNeeded
    }
```

## Inspection/RMP Inspection columns

```mermaid
erDiagram
    Inspection {
        datetime InspectionStarted
        datetime InspectionEnded
        enum InspectionReason
        string WeatherConditions
        string InspectionGuide
        bool FacilityOperating
        bool DeviationsNoted
        bool FollowupTaken
    }
```

## Notification columns

```mermaid
erDiagram
    Notification {
        Guid NotificationType
        date ReceivedDate
        date DueDate
        date SentDate
        bool FollowupTaken
    }
```

### Notification types

- Other
- Startup
- Permit Revocation
- Response Letter
- Malfunction
- Deviation

## Permit Revocation columns

```mermaid
erDiagram
    PermitRevocation {
        date ReceivedDate
        date PermitRevocationDate
        date PhysicalShutdownDate
        bool FollowupTaken
    }
```

## Report columns

```mermaid
erDiagram
    Report {
        date ReceivedDate
        enum ReportingPeriodType
        date ReportingPeriodStart
        date ReportingPeriodEnd
        string ReportingPeriodComment
        date DueDate
        date SentDate
        bool ReportComplete
        bool ReportsDeviations
        bool EnforcementNeeded
    }
```

## Source Test Review columns

```mermaid
erDiagram
    SourceTestReview {
        int ReferenceNumber
        date ReceivedByCompliance
        date TestDue
        bool FollowupTaken
    }
```
