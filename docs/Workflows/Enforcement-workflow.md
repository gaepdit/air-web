# Enforcement Workflow

## Enforcement Case

* A new Enforcement can be entered from a Facility or Compliance Event.
* An Enforcement can be linked to multiple Compliance Events.
* The Enforcement can be edited while open.
* Closing an Enforcement disables all editing.
* Reopening an Enforcement enables all editing.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.
* An Enforcement can be deleted *(not shown)*.

## Enforcement Action

* An Enforcement Action can be added to an Enforcement.
* An Enforcement Action can be edited while the Enforcement is open.
* Issuing an Enforcement Action closes it and disables all editing.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.
* An Enforcement Action can be deleted *(not shown)*.

## Enforcement Action Review

* An Enforcement Action can be submitted for review, creating an Enforcement Action Review.
* An Enforcement Action Review can be completed, updating the Enforcement Action.

## Data Exchange/Internal Auditing

* Submitting to EPA will enable the Data Exchange.
* Any of the following will update the Data Exchange and generate an audit point *(not shown)*:
    * Adding or editing an Enforcement or Enforcement Action.
    * Closing or reopening the Enforcement.
    * Linking a Compliance Event.
    * Issuing an Enforcement Action.

## Flow Chart

```mermaid
flowchart
    FAC{{Facility}}
    EVT{{Compliance Event}}
    ENF{{"`**Enforcement**`"}}
    CMT{{Enforcement Comment}}
    ACT{{"`**Enforcement Action**`"}}
    REV{{"Enforcement Action Review"}}
    CMA{{Action Comment}}
    STP{{Stipulated Penalty}}

    link([Link Event])
    add([Enter new LON/Case File])
    comment([Add Comment])
    editEnf([Edit])
    addAction([Add Enforcement Action])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    editAction([Edit Action])
    commentAction([Add Comment])
    editCommentAction([Edit Action Comment])
    review([Submit for Review])
    respond([Approve/Return])
    issue([Issue/Abandon])
    epa([Submit to EPA])
    penalty([Add Penalty])

    FAC -.-> add
    ENF -.-> link
    EVT -.-> add
    ENF -.-> editEnf
    ENF -.-> close
    ENF -..-> comment
    ENF -.-> addAction
    ENF -.-> epa
    CMT -.-> editComment
    ACT -.-> commentAction
    ACT -.-> editAction
    ACT -.-> review
    ACT -.-> issue
    CMA -.-> editCommentAction
    REV -.-> respond
    ACT -..-> penalty

    close -->|"`Disables/*enables*`"| editEnf
    close -->|"`Disables/*enables*`"| addAction
    close -->|"`Disables/*enables*`"| link
    issue -->|Disables| editAction
    issue -->|Disables| commentAction
    issue -->|Disables| review

    link -->|Links to| EVT
    add -->|Creates| ENF
    editEnf -->|Updates| ENF
    editAction -->|Updates| ACT
    issue -->|Closes| ACT
    issue -->|Updates status| ENF
    close -->|"`Closes/*reopens*`"| ENF
    close -->|Closes all| ACT
    close -->|Disables| penalty
    editComment -->|Updates| CMT
    editCommentAction -->|Updates| CMA
    comment -->|Adds| CMT
    commentAction -->|Adds| CMA
    addAction -->|Adds| ACT
    review -->|Starts| REV
    respond -->|Updates| ACT
    penalty -->|Adds| STP

```

## Detailed Entity Relationship Diagram


```mermaid
erDiagram

FAC["Facility"] {
    string airsNumber PK
}

CWE["Compliance Event (Work Entry)"] {
    int Id PK
    string airsNumber FK
}

ENF["Enforcement"] {
    int Id PK
    string airsNumber FK
}

CEL["Compliance Event/Enforcement Linkage"] {
    int enforcementId FK
    int workEntryId FK
}

ECM["Enforcement Comment"] {
    Guid Id PK
    int enforcementId FK
}

ACT["Enforcement Action"] {
    Guid Id PK
    int enforcementId FK
}

REV["Enforcement Action Review"] {
    Guid Id PK
    int enforcementId FK
}

CMA["Action Comment"] {
    Guid Id PK
    Guid actionId FK
}

DOC["LON, NOV, AO, NFA"]
CO["Consent Order"]
POL["Pollutant"]
PGM["Air Program"]
STP["Stipulated Penalty"]

CWE }o--|| FAC : "is entered for"
ENF }o--|| FAC : "is issued to"

CEL }o--|| CWE : "is triggered by"
CEL }o--|| ENF : "is addressed by"

ECM }o--|| ENF : "comments on"
ACT }|--|| ENF : "advances"

ENF ||--o{ POL : "is associated with"
ENF ||--o{ PGM : "is associated with"

DOC |o--|| ACT : "is a type of"
CO |o--|| ACT : "is a type of"

REV }|--|| ACT : "reviews"
CMA }o--|| ACT : "comments on"

STP }o--|| CO : "may be required by"

```
