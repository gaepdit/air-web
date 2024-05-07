# Annual Compliance Certification (ACC) Workflow

* A new ACC can be started from a Facility.
    - The new ACC should include review data and may include initial comments.
* The ACC can be updated while open.
* Additional comments can be added at any time.
* Closing an ACC disables updating and enables printing.
* Reopening an ACC enables updating and disables printing.
* Closing or Reopening an ACC updates the data exchange.
* The ACC report can be printed.
* Enforcement can be started from a report.
* An ACC can be deleted/undeleted *(not shown)*.
* Comments can be edited.
* A Comment can be deleted *(not shown)*.

```mermaid
flowchart
    FAC{{Facility}}
    ACC{{"`**ACC**`"}}
    CMT{{Comment}}
    DX{{Data Exchange}}
    RPT{{Report}}
    ENF{{Enforcement}}

    start([Start new ACC])
    comment([Add Comment])
    update([Edit])
    enforce([Start new Enforcement])
    print([Print])
    close(["`Close/*Reopen*`"])
    updateComment([Edit Comment])

    FAC -.-> start
    ACC -.-> update
    ACC -.-> close
    ACC -.-> print
    ACC -..-> comment
    ACC -..-> enforce
    CMT -.-> updateComment

    close -->|"`Enables/*disables*`"| print
    close -->|"`Disables/*enables*`"| update

    start -->|Creates| ACC
    update -->|Updates| ACC
    close -->|"`Closes/*reopens*`"| ACC
    updateComment -->|Updates| CMT
    print -->|Opens| RPT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Updates| DX
```
