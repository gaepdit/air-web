# Notification Workflow

* A new Notification can be entered from a Facility.
* The Notification can be edited.
* A Notification can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    NOT{{"`**Notification**`"}}
    CMT{{Comment}}

    enter([Enter new Notification])
    edit([Edit])
    comment([Add Comment])
    editComment([Edit Comment])

    FAC -.-> enter
    NOT -.-> edit
    NOT -.-> comment
    CMT -.-> editComment

    enter -->|Creates| NOT
    edit -->|Updates| NOT
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    
```
