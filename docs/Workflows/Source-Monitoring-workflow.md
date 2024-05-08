# Source Monitoring Compliance Review Workflow

* A new Source Monitoring Compliance Review can be entered from a Source Monitoring Report.
* The Review entry can be edited.
* Saving a Review entry updates the data exchange.
* Enforcement can be started from a Review entry.
* A Review entry can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    MON{{Source Monitoring Report}}
    SMR{{"`**Source Monitoring Compliance Review**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    ENF{{Enforcement}}

    enter([Enter new Compliance Review])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    editComment([Edit Comment])

    MON -.-> enter
    SMR -.-> edit
    SMR -.-> comment
    SMR -.-> enforce
    CMT -.-> editComment

    enter -->|Creates| SMR
    edit -->|Updates| SMR
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    enter -->|Updates| DX
    edit -->|Updates| DX

```
