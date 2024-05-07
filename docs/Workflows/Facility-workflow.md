# Facility Workflow

* A new FCE can be entered from a Facility.
* A new ACC can be entered from a Facility.
* A new Inspection can be entered from a Facility.
* A new RMP Inspection can be entered from a Facility.
* A new Report can be entered from a Facility.

```mermaid
flowchart LR
    FAC{{"`**Facility**`"}}
    FCE{{FCE}}
    ACC{{ACC}}
    INS{{Inspection}}
    RMP{{RMP Inspection}}
    REP{{Report}}

    fce([Generate FCE])
    acc([Enter new ACC])
    ins([Enter new Inspection])
    rmp([Enter new RMP Inspection])
    rep([Enter new Report])

    FAC -.-> fce -->|Creates| FCE
    FAC -.-> acc -->|Creates| ACC
    FAC -.-> ins -->|Creates| INS
    FAC -.-> rmp -->|Creates| RMP
    FAC -.-> rep -->|Creates| REP

```
