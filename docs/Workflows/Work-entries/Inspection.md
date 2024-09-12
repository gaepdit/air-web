# Inspection Workflow

## Workflow additions

* An Inspection is a Compliance Event.
* An Inspection is automatically closed when created.

## Flowchart

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**Inspection**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    FCE{{FCE}}
    ENF{{Enforcement}}

    enter([Enter new Inspection])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    fce([Generate FCE])

    FAC -.-> enter
    WRK -.-> edit
    WRK -.-> close
    WRK -.-> comment
    WRK -.-> enforce
    WRK -.-> fce
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
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
        bool DeviationsNoted
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
| SSCPINSPECTIONS.STRINSPECTIONCOMPLIANCESTATUS | varchar(35)   |    ✔    | DeviationsNoted      |
| SSCPINSPECTIONS.STRINSPECTIONCOMMENTS         | varchar(4000) |    ✔    | base.Notes           |
| SSCPINSPECTIONS.STRINSPECTIONFOLLOWUP         | varchar(5)    |    ✔    | FollowupTaken        |
| SSCPINSPECTIONS.STRMODIFINGPERSON             | varchar(3)    |    ?    | base.UpdatedById     |
| SSCPINSPECTIONS.DATMODIFINGDATE               | datetime2(0)  |    ?    | base.UpdatedAt       |
