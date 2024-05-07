# Report Entry Workflow

* A new Report can be entered from a Facility.
* The Report entry can be edited.
* Saving a Report entry updates the data exchange.
* Enforcement can be started from a Report entry.
* A Report entry can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

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
    updateComment([Edit Comment])
    fce([Generate FCE])

    FAC -.-> enter
    INS -.-> edit
    INS -..-> comment
    INS -..-> enforce
    INS -..-> fce
    CMT -.-> updateComment

    enter -->|Creates| INS
    edit -->|Updates| INS
    updateComment -->|Updates| CMT
    comment -->|Adds| CMT
    fce -->|Creates| FCE
    enforce -->|Creates| ENF
    enter -->|Updates| DX
    edit --->|Updates| DX

```
