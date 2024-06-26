# Report Workflow

## Flowchart

```mermaid
flowchart
    FAC{{Facility}}
    REP{{"`**Report**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    ENF{{Enforcement}}

    enter([Enter new Report])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    editComment([Edit Comment])

    FAC -.-> enter
    REP -.-> edit
    REP -.-> comment
    REP -.-> enforce
    CMT -.-> editComment

    enter -->|Creates| REP
    edit -->|Updates| REP
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    enter -->|Updates| DX
    edit -->|Updates| DX

```

## ERD

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

## Original IAIP table columns

| Column                                 | Type          | Migrate | Destination            |
|----------------------------------------|---------------|:-------:|------------------------|
| SSCPITEMMASTER.DATRECEIVEDDATE         | datetime2(0)  |    ✔    | ReceivedDate           |
| SSCPREPORTS.STRREPORTPERIOD            | varchar(25)   |    ✔    | ReportingPeriodType             |
| SSCPREPORTS.DATREPORTINGPERIODSTART    | datetime2(0)  |    ✔    | ReportingPeriodStart   |
| SSCPREPORTS.DATREPORTINGPERIODEND      | datetime2(0)  |    ✔    | ReportingPeriodEnd     |
| SSCPREPORTS.STRREPORTINGPERIODCOMMENTS | varchar(4000) |    ✔    | ReportingPeriodComment |
| SSCPREPORTS.DATREPORTDUEDATE           | datetime2(0)  |    ✔    | DueDate                |
| SSCPREPORTS.DATSENTBYFACILITYDATE      | datetime2(0)  |    ✔    | SentDate               |
| SSCPREPORTS.STRCOMPLETESTATUS          | varchar(5)    |    ✔    | ReportComplete       |
| SSCPREPORTS.STRENFORCEMENTNEEDED       | varchar(5)    |    ✔    | EnforcementNeeded    |
| SSCPREPORTS.STRSHOWDEVIATION           | varchar(5)    |    ✔    | ReportsDeviations      |
| SSCPREPORTS.STRGENERALCOMMENTS         | varchar(4000) |    ✔    | base.Notes             |
| SSCPREPORTS.STRMODIFINGPERSON          | varchar(3)    |    ?    | base.UpdatedById       |
| SSCPREPORTS.DATMODIFINGDATE            | datetime2(0)  |    ?    | base.UpdatedAt         |
| SSCPREPORTS.STRSUBMITTALNUMBER         | varchar(3)    |    ✖    | *none*                 |
