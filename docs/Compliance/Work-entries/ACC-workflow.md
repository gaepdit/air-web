# Annual Compliance Certification (ACC) Workflow

* A new ACC can be entered from a Facility.
* The ACC can be edited while open.
* Closing an ACC disables updating and enables printing.
* Reopening an ACC enables updating and disables printing.
* Closing or Reopening an ACC updates the Data Exchange.
* The ACC report can be printed.
* Enforcement can be started from an ACC.
* An ACC can be deleted/restored *(not shown)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    ACC{{"`**ACC**`"}}
    CMT{{Comment}}
    DX{{Data Exchange}}
    RPT{{Report}}
    ENF{{Enforcement}}

    enter([Enter new ACC])
    comment([Add Comment])
    edit([Edit])
    enforce([Start new Enforcement])
    print([Print])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])

    FAC -.-> enter
    ACC -.-> edit
    ACC -.-> close
    ACC -.-> print
    ACC -..-> comment
    ACC -..-> enforce
    CMT -.-> editComment

    close -->|"`Enables/*disables*`"| print
    close -->|"`Disables/*enables*`"| edit

    enter -->|Creates| ACC
    edit -->|Updates| ACC
    close -->|"`Closes/*reopens*`"| ACC
    editComment -->|Updates| CMT
    print -->|Opens| RPT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Updates| DX
```