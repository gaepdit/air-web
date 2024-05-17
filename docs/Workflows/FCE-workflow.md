# Full Compliance Evaluation (FCE) Workflow

* A new FCE can be entered from a Facility or an Inspection.
* The FCE can be edited.
* Saving an FCE updates the Data Exchange.
* The FCE report can be printed.
* An FCE can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

## Flowchart

```mermaid
flowchart
    FAC{{Facility}}
    INS{{Inspection}}
    FCE{{"`**FCE**`"}}
    DX{{Data Exchange}}
    RPT{{Report}}
    CMT{{Comment}}

    enter([Enter new FCE])
    edit([Edit])
    print([Print])
    comment([Add Comment])
    editComment([Edit Comment])

    FAC -.-> enter
    INS -.-> enter
    FCE -.-> edit
    FCE -.-> print
    FCE -.-> comment
    CMT -.-> editComment

    enter -->|Creates| FCE
    edit -->|Updates| FCE
    print -->|Opens| RPT
    enter -->|Updates| DX
    edit -->|Updates| DX
    comment -->|Adds| CMT
    editComment -->|Updates| CMT
    
```

## Entity Relationship Diagram

```mermaid
erDiagram

FAC["Facility"] {
    string facilityId PK
}

FCE["FCE"] {
    int Id PK
    string facilityId FK
    string reviewedBy FK
    int fceYear
    date completedDate
    bool onsiteInsection
    string comment
}

STF["Staff"] {
    string Id PK
}

CTE["Comment"] {
    int Id PK
    int fceId FK
}

FCE }o--|| FAC : "is completed for"
STF ||--o{ FCE: "completes"
CTE }o--|| FCE: "comments on"

```
