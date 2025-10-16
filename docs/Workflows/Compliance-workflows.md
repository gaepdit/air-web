# Compliance Monitoring Workflows

## Compliance Monitoring Types

| Compliance Monitoring Type            | Automatically Closed * | Compliance Event † | Data Exchange ‡ |
|---------------------------------------|:----------------------:|:------------------:|:---------------:|
| Annual Compliance Certification (ACC) |                        |         ✓          |        ✓        |
| Inspection                            |           ✓            |         ✓          |        ✓        |
| Notification                          |           ✓            |                    |                 |
| Permit revocation                     |                        |                    |                 |
| Report                                |           ✓            |         ✓          |        ✓        |
| Risk Management Plan (RMP) Inspection |           ✓            |         ✓          |                 |
| Source Test Compliance Review         |           ✓            |         ✓          |        ✓        |

<small>
* Indicates the Compliance Monitoring Entry is automatically closed when created (but can be reopened).<br>
† Indicates the Compliance Monitoring Entry is a Compliance Event (i.e., available as an enforcement discovery event).<br>
‡ Indicates a Compliance Event that is shared with the ICIS-Air Data Exchange.
</small>

### General Compliance Monitoring Workflow

* A new Compliance Monitoring Entry can be entered from a Facility.
* The Entry can be edited if open.
    * Closing an Entry disables editing.
    * Reopening an Entry enables editing.
    * Some Entries are automatically closed when they are first created.
* An Entry can be deleted/restored *(not shown in diagrams)*.
* Comments can be added.
* A Comment can be deleted *(not shown)*.

### Compliance Event Workflow

* Some Compliance Monitoring types are categorized as *Compliance Events.*
* Enforcement can be started from a Compliance Event.
* Compliance Events are shared with the Data Exchange (excluding RMP Inspections).

## Annual Compliance Certification (ACC) Workflow

* An ACC is a Compliance Event.
* An ACC report can be printed if closed.
    * Closing an ACC enables printing.
    * Reopening an ACC disables printing.

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**ACC**`"}}
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
    WRK -.-> close
    WRK -.-> print
    WRK -..-> comment
    WRK -..-> enforce
    CMT -.-> editComment
    close -->|"`Enables/*disables*`"| print
    close -->|"`Disables/*enables*`"| edit
    WRK -.-> edit
    enter -->|Creates| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    print -->|Opens| RPT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Closing updates| DX
```

## Inspection Workflow

* An Inspection is a Compliance Event.
* An Inspection is automatically closed when created.

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**Inspection**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    ENF{{Enforcement}}
    enter([Enter new Inspection])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    FAC -.-> enter
    WRK -.-> close
    WRK -..-> comment
    WRK -..-> enforce
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    WRK -.-> edit
    enter -->|Creates and closes| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Closing updates| DX
```

## Notification Workflow

* A Notification is automatically closed when created.

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**Notification**`"}}
    CMT{{Comment}}
    enter([Enter new Notification])
    edit([Edit])
    comment([Add Comment])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    FAC -.-> enter
    WRK -.-> edit
    WRK -.-> close
    WRK -.-> comment
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates and closes| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
```

## Permit Revocation Workflow

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**Permit Revocation**`"}}
    CMT{{Comment}}
    enter([Enter new Permit Revocation])
    comment([Add Comment])
    edit([Edit])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    FAC -.-> enter
    WRK -.-> edit
    WRK -.-> close
    WRK -.-> comment
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
```

## Report Workflow

* A Report is a Compliance Event.
* A Report is automatically closed when created.

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**Report**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    ENF{{Enforcement}}
    enter([Enter new Report])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    FAC -.-> enter
    WRK -.-> close
    WRK -..-> comment
    WRK -..-> enforce
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    WRK -.-> edit
    enter -->|Creates and closes| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Closing updates| DX
```

## RMP Inspection Workflow

* An RMP Inspection is a Compliance Event.
* An RMP Inspection is automatically closed when created.

```mermaid
flowchart
    FAC{{Facility}}
    WRK{{"`**RMP Inspection**`"}}
    CMT{{Comment}}
    ENF{{Enforcement}}
    enter([Enter new Inspection])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    FAC -.-> enter
    WRK -.-> edit
    WRK -.-> close
    WRK -..-> comment
    WRK -..-> enforce
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates and closes| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
```

## Source Test Compliance Review Workflow

* A Source Test Compliance Review can only be entered from a Source Test Report (STR).
* A Source Test Compliance Review is a Compliance Event.
* A Source Test Compliance Review is automatically closed when created.

```mermaid
flowchart
    STR{{Source Test Report}}
    WRK{{"`**Source Test Compliance Review**`"}}
    DX{{Data Exchange}}
    CMT{{Comment}}
    ENF{{Enforcement}}
    enter([Enter new Compliance Review])
    comment([Add Comment])
    edit([Edit])
    enforce([Start Enforcement])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    STR -.-> enter
    WRK -.-> close
    WRK -..-> comment
    WRK -..-> enforce
    CMT -.-> editComment
    close -->|"`Disables/*enables*`"| edit
    WRK -.-> edit
    enter -->|Creates and closes| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Closing updates| DX
```
