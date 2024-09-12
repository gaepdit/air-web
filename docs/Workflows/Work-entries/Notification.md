# Notification Workflow

## Workflow additions

* A Notification is automatically closed when created.

## Flowchart

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**Notification**`"}}
    CMT{{Comment}}

    enter([Enter new Notification])
    edit([Edit])
    comment([Add Comment])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])

    FAC -.-> enter
    WRK -.-> edit
    WRK -.-> close
    WRK -.-> comment
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
```

## ERD

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

## Original IAIP table columns

| Column                                     | Type          | Migrate | Destination      |
|--------------------------------------------|---------------|:-------:|------------------|
| SSCPITEMMASTER.DATRECEIVEDDATE             | datetime2(0)  |    ✔    | ReceivedDate     |
| SSCPNOTIFICATIONS.DATNOTIFICATIONDUE       | datetime2(0)  |    ✔    | DueDate          |
| SSCPNOTIFICATIONS.STRNOTIFICATIONDUE       | varchar(5)    |    ✔    | DueDate          |
| SSCPNOTIFICATIONS.DATNOTIFICATIONSENT      | datetime2(0)  |    ✔    | SentDate         |
| SSCPNOTIFICATIONS.STRNOTIFICATIONSENT      | varchar(10)   |    ✔    | SentDate         |
| SSCPNOTIFICATIONS.STRNOTIFICATIONTYPE      | varchar(2)    |    ✔    | NotificationType |
| SSCPNOTIFICATIONS.STRNOTIFICATIONTYPEOTHER | varchar(100)  |    ✔    | base.Notes       |
| SSCPNOTIFICATIONS.STRNOTIFICATIONCOMMENT   | varchar(4000) |    ✔    | base.Notes       |
| SSCPNOTIFICATIONS.STRNOTIFICATIONFOLLOWUP  | varchar(5)    |    ✔    | FollowupTaken    |
| SSCPNOTIFICATIONS.STRMODIFINGPERSON        | varchar(3)    |    ?    | base.UpdatedById |
| SSCPNOTIFICATIONS.DATMODIFINGDATE          | datetime2(0)  |    ?    | base.UpdatedAt   |
