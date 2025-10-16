# Full Compliance Evaluation (FCE) Workflow

* A new FCE can be entered from a Facility or an Inspection.
* An FCE can be edited.
* Saving an FCE updates the Data Exchange.
* An FCE report can be printed.
* An FCE can be deleted/restored *(not shown in diagram)*.
* Comments can be added.
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
