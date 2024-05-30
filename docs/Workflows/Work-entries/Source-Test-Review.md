# Source Test Compliance Review Workflow

## Workflow additions

* A new Source Test Compliance Review can be entered from a Source Test Report.

## Flowchart

```mermaid
flowchart
    TST{{Source Test Report}}
    STR{{"`**Source Test Compliance Review**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    ENF{{Enforcement}}

    enter([Enter new Compliance Review])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    editComment([Edit Comment])

    TST -.-> enter
    STR -.-> edit
    STR -.-> comment
    STR -.-> enforce
    CMT -.-> editComment

    enter -->|Creates| STR
    edit -->|Updates| STR
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    enter -->|Updates| DX
    edit -->|Updates| DX

```

## ERD

```mermaid
erDiagram
    SourceTestReview {
        int ReferenceNumber
        date ReceivedByCompliance
        date TestDue
        bool FollowupTaken
    }
```

## Original IAIP table columns

| Column                                | Type          | Migrate | Destination          |
|---------------------------------------|---------------|:-------:|----------------------|
| SSCPITEMMASTER.DATRECEIVEDDATE        | datetime2(0)  |    ✔    | ReceivedByCompliance |
| SSCPTESTREPORTS.STRREFERENCENUMBER    | varchar(9)    |    ✔    | ReferenceNumber      |
| SSCPTESTREPORTS.DATTESTREPORTDUE      | datetime2(0)  |    ✔    | TestDue              |
| SSCPTESTREPORTS.STRTESTREPORTCOMMENTS | varchar(4000) |    ✔    | base.Notes           |
| SSCPTESTREPORTS.STRTESTREPORTFOLLOWUP | varchar(5)    |    ✔    | FollowupTaken        |
| SSCPTESTREPORTS.STRMODIFINGPERSON     | varchar(3)    |    ?    | base.UpdatedById     |
| SSCPTESTREPORTS.DATMODIFINGDATE       | datetime2(0)  |    ?    | base.UpdatedAt       |
