# Permit Revocation Workflow

* A new Permit Revocation can be started from a Facility.
* The Permit Revocation can be edited while open.
* Finalizing a Permit Revocation disables updating.
* A Permit Revocation can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    REV{{"`**Permit Revocation**`"}}
    CMT{{Comment}}

    enter([Enter new Permit Revocation])
    comment([Add Comment])
    edit([Edit])
    finalize([Finalize])
    editComment([Edit Comment])

    FAC -.-> enter
    REV -.-> edit
    REV -.-> finalize
    REV -..-> comment
    CMT -.-> editComment

    finalize -->|Disables| edit

    enter -->|Creates| REV
    edit -->|Updates| REV
    finalize -->|Closes| REV
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
```
