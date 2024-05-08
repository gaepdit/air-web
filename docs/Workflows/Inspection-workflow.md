# Inspection Workflow

* A new Inspection can be entered from a Facility.
* The Inspection can be edited.
* Saving an Inspection updates the data exchange.
* Enforcement can be started from an Inspection.
* A new FCE can be entered from an Inspection.
* An Inspection can be deleted/restored *(not shown)*.
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
