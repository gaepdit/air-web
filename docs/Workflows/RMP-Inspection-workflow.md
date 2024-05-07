# RMP Inspection Workflow

* A new RMP Inspection can be entered from a Facility.
* The RMP Inspection can be edited.
* Enforcement can be started from an RMP Inspection.
* An RMP Inspection can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    INS{{"`**RMP Inspection**`"}}
    CMT{{Comment}}
    ENF{{Enforcement}}

    enter([Enter new RMP Inspection])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    updateComment([Edit Comment])
    
    FAC -.-> enter
    INS -.-> edit
    INS -..-> comment
    INS -..-> enforce
    CMT -.-> updateComment

    enter -->|Creates| INS
    edit -->|Updates| INS
    updateComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF

```
