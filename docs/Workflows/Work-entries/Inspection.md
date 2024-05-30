# Inspection Workflow

## Workflow additions

* A new FCE can be entered from an Inspection.

## Flowchart

```mermaid
flowchart
    FAC{{Facility}}
    INS{{"`**Inspection**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    FCE{{FCE}}
    ENF{{Enforcement}}

    enter([Enter new Inspection])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    editComment([Edit Comment])
    fce([Generate FCE])

    FAC -.-> enter
    INS -.-> edit
    INS -.-> comment
    INS -.-> enforce
    INS -.-> fce
    CMT -.-> editComment

    enter -->|Creates| INS
    edit -->|Updates| INS
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    fce -->|Creates| FCE
    enforce -->|Creates| ENF
    enter -->|Updates| DX
    edit -->|Updates| DX

```

## ERD

```mermaid
erDiagram
    Inspection {
        datetime InspectionStarted
        datetime InspectionEnded
        enum InspectionReason
        string WeatherConditions
        string InspectionGuide
        bool FacilityOperating
        enum ComplianceStatus
        bool FollowupTaken
    }
```

## Original IAIP table columns

| Column                                        | Type          | Migrate | Destination          |
|-----------------------------------------------|---------------|:-------:|----------------------|
| SSCPITEMMASTER.DATRECEIVEDDATE                | datetime2(0)  |    ✖    | *none*               |
| SSCPINSPECTIONS.DATINSPECTIONDATESTART        | datetime2(0)  |    ✔    | InspectionStarted    |
| SSCPINSPECTIONS.DATINSPECTIONDATEEND          | datetime2(0)  |    ✔    | InspectionEnded      |
| SSCPINSPECTIONS.STRINSPECTIONREASON           | varchar(35)   |    ✔    | InspectionReason     |
| SSCPINSPECTIONS.STRWEATHERCONDITIONS          | varchar(100)  |    ✔    | WeatherConditions    |
| SSCPINSPECTIONS.STRINSPECTIONGUIDE            | varchar(100)  |    ✔    | InspectionGuide      |
| SSCPINSPECTIONS.STRFACILITYOPERATING          | varchar(5)    |    ✔    | WasFacilityOperating |
| SSCPINSPECTIONS.STRINSPECTIONCOMPLIANCESTATUS | varchar(35)   |    ✔    | ComplianceStatus     |
| SSCPINSPECTIONS.STRINSPECTIONCOMMENTS         | varchar(4000) |    ✔    | base.Notes           |
| SSCPINSPECTIONS.STRINSPECTIONFOLLOWUP         | varchar(5)    |    ✔    | FollowupTaken        |
| SSCPINSPECTIONS.STRMODIFINGPERSON             | varchar(3)    |    ?    | base.UpdatedById     |
| SSCPINSPECTIONS.DATMODIFINGDATE               | datetime2(0)  |    ?    | base.UpdatedAt       |
