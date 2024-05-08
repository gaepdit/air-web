# Full Compliance Evaluation (FCE) Workflow

* A new FCE can be entered from a Facility or an Inspection.
* The FCE can be edited.
* Saving an FCE updates the data exchange.
* The FCE report can be printed.
* An FCE can be deleted/restored *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    INS{{Inspection}}
    FCE{{"`**FCE**`"}}
    DX{{Data Exchange}}
    RPT{{Report}}

    enter([Enter new FCE])
    edit([Edit])
    print([Print])

    FAC -.-> enter
    INS -.-> enter
    FCE -.-> edit
    FCE -.-> print

    enter -->|Creates| FCE
    edit -->|Updates| FCE
    print -->|Opens| RPT
    enter -->|Updates| DX
    edit -->|Updates| DX
```
