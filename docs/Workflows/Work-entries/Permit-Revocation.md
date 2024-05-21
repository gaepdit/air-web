# Permit Revocation Workflow

* A new Permit Revocation can be started from a Facility.
* The Permit Revocation can be edited while open.
* Finalizing a Permit Revocation disables updating.
* A Permit Revocation can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    REV{{"`**Permit Revocation**`"}}
    CMT{{Comment}}

    enter([Enter new Permit Revocation])
    comment([Add Comment])
    edit([Edit])
    finalize([Finalize])
    editComment([Edit Comment])

    FAC -.-> enter
    REV -.-> edit
    REV -.-> finalize
    REV -..-> comment
    CMT -.-> editComment

    finalize -->|Disables| edit
    enter -->|Creates| REV
    edit -->|Updates| REV
    finalize -->|Closes| REV
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
```

## ERD

```mermaid
erDiagram
    PermitRevocation {
        enum NotificationType
        string NotificationDescription
        date PermitRevocationDate
        date PhysicalShutdownDate
        date SentDate
        bool FollowupTaken
    }
```

## Original IAIP table columns

| Column                                     | Type          | Migrate | Destination          |
|--------------------------------------------|---------------|:-------:|----------------------|
| SSCPITEMMASTER.DATRECEIVEDDATE             | datetime2(0)  |    ✔    | ReceivedDate         |
| SSCPNOTIFICATIONS.DATNOTIFICATIONDUE       | datetime2(0)  |    ✔    | PermitRevocationDate |
| SSCPNOTIFICATIONS.STRNOTIFICATIONDUE       | varchar(5)    |    ✖    | *none*               |
| SSCPNOTIFICATIONS.DATNOTIFICATIONSENT      | datetime2(0)  |    ✔    | PhysicalShutdownDate |
| SSCPNOTIFICATIONS.STRNOTIFICATIONSENT      | varchar(10)   |    ✔    | PhysicalShutdownDate |
| SSCPNOTIFICATIONS.STRNOTIFICATIONTYPE      | varchar(2)    |    ✖    | *none*               |
| SSCPNOTIFICATIONS.STRNOTIFICATIONTYPEOTHER | varchar(100)  |    ✔    | base.Notes           |
| SSCPNOTIFICATIONS.STRNOTIFICATIONCOMMENT   | varchar(4000) |    ✔    | base.Notes           |
| SSCPNOTIFICATIONS.STRNOTIFICATIONFOLLOWUP  | varchar(5)    |    ✔    | FollowupTaken        |
| SSCPNOTIFICATIONS.STRMODIFINGPERSON        | varchar(3)    |    ?    | base.UpdatedById     |
| SSCPNOTIFICATIONS.DATMODIFINGDATE          | datetime2(0)  |    ?    | base.UpdatedAt       |
