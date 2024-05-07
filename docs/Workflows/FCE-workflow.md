# Full Compliance Evaluation (FCE) Workflow

* A new FCE can be started from a Facility or an Inspection.
* The FCE can be updated.
* Saving an FCE updates the data exchange.
* The FCE report can be printed.
* An FCE can be deleted/undeleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    INS{{Inspection}}
    FCE{{"`**FCE**`"}}
    DX{{Data Exchange}}
    RPT{{Report}}

    start([Start new FCE])
    update([Edit])
    print([Print])

    FAC -.-> start
    INS -.-> start
    FCE -.-> update
    FCE -.-> print

    start -->|Creates| FCE
    update -->|Updates| FCE
    print -->|Opens| RPT
    start -->|Updates| DX
    update -->|Updates| DX
```
