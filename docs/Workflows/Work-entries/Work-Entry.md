# Work Entry

## General Workflow

* A new Work Entry can be entered from a Facility.
* The Work Entry can be edited.
* Enforcement can be started from a Compliance Event-type Work Entry.
* Saving a Compliance Event updates the Data Exchange.
* A Work Entry can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

## Entities

- WRK: Work Entry
    - ACC: Annual Compliance Certification (ACC) †
    - INS: Inspection †
    - RMP: RMP Inspection †
    - REP: Report †
    - STR: Source Test Compliance Review †
    - NOT: Notification
    - REV: Permit revocation

<small>† Indicates a Compliance Event (available as an enforcement discovery event).</small>

## Base ERD

```mermaid
erDiagram
    FAC["Facility"] {
        string facilityId PK
    }

    WRK["Work Entry"] {
        int Id PK
        string FacilityId FK
        enum WorkEntryType
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

## Original IAIP table columns

| Column                                    | Type         | Migrate | Destination              |
|-------------------------------------------|--------------|:-------:|--------------------------|
| SSCPITEMMASTER.STRTRACKINGNUMBER          | numeric(10)  |    ✔    | Id                       |
| SSCPITEMMASTER.STRAIRSNUMBER              | varchar(12)  |    ✔    | FacilityId               |
| SSCPITEMMASTER.DATRECEIVEDDATE            | datetime2(0) |    ✔    | *subtypes*               |
| SSCPITEMMASTER.STREVENTTYPE               | varchar(3)   |    ✔    | WorkEntryType            |
| SSCPITEMMASTER.STRRESPONSIBLESTAFF        | varchar(3)   |    ✔    | ResponsibleStaffId       |
| SSCPITEMMASTER.DATCOMPLETEDATE            | datetime2(0) |    ✔    | Closed, ClosedDate       |
| SSCPITEMMASTER.STRMODIFINGPERSON          | varchar(3)   |    ✔    | base.UpdatedById         |
| SSCPITEMMASTER.DATMODIFINGDATE            | datetime2(0) |    ✔    | base.UpdatedAt           |
| SSCPITEMMASTER.STRDELETE                  | varchar(5)   |    ✔    | base.IsDeleted           |
| SSCPITEMMASTER.DATACKNOLEDGMENTLETTERSENT | datetime2(0) |    ✔    | AcknowledgmentLetterDate |
| SSCPITEMMASTER.DATINFORMATIONREQUESTDATE  | datetime2(0) |    ✖    | *none*                   |
| SSCPITEMMASTER.ICIS_STATUSIND             | varchar      | *defer* | EpaDxStatus              |
