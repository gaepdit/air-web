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
    comment --->|Adds| CMT
    enforce --->|Creates| ENF
    enter -->|Updates| DX
    edit --->|Updates| DX

```
