# Compliance Work Entry Workflows

## Work Entry Types

- Annual Compliance Certification (ACC) † ‡
- Inspection * † ‡
- Notification *
- Permit revocation
- Report * † ‡
- RMP Inspection * †
- Source Test Compliance Review * † ‡

<small>* Indicates the Work Entry is automatically closed when created.</small><br>
<small>† Indicates the Work Entry is a Compliance Event (i.e., available as an enforcement discovery event).</small><br>
<small>‡ Indicates a Compliance Event that is shared with the ICIS-Air data exchange.</small>

### General Workflow

* A new Work Entry can be entered from a Facility.
* The Work Entry can be edited if open.
    * Closing a Work Entry disables editing.
    * Reopening a Work Entry enables editing.
    * Some Work Entries are automatically closed when they are first created.
* A Work Entry can be deleted/restored *(not shown in diagrams)*.
* Comments can be added and edited.
* A Comment can be deleted *(not shown in diagrams)*.

### Compliance Event Workflow

* Some Work Entry types are categorized as "Compliance Events."
* Enforcement can be started from a Compliance Event.
* Closing a Compliance Event updates the Data Exchange (not including an RMP Inspection).

## Annual Compliance Certification (ACC) Workflow Additions

* An ACC is a Compliance Event.
* An ACC report can be printed if closed.
    * Closing an ACC enables printing.
    * Reopening an ACC disables printing.

### Flowchart

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
    WRK -.-> edit
    WRK -.-> close
    WRK -.-> print
    WRK -..-> comment
    WRK -..-> enforce
    CMT -.-> editComment

    close -->|"`Enables/*disables*`"| print
    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    print -->|Opens| RPT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
    close --->|Closing updates| DX
```

## Inspection Workflow Additions

* An Inspection is a Compliance Event.
* An Inspection is automatically closed when created.

### Flowchart

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
    close --->|Closing updates| DX
```

## Notification Workflow Additions

* A Notification is automatically closed when created.

### Flowchart

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

### Flowchart

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

## Report Workflow Additions

* A Report is a Compliance Event.
* A Report is automatically closed when created.

### Flowchart

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
    close --->|Closing updates| DX
```

## RMP Inspection Workflow Additions

* An RMP Inspection is a Compliance Event.
* An RMP Inspection is automatically closed when created.

### Flowchart

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
    WRK -.-> comment
    WRK -.-> enforce
    CMT -.-> editComment

    close -->|"`Disables/*enables*`"| edit
    enter -->|Creates and closes| WRK
    edit -->|Updates| WRK
    close -->|"`Closes/*reopens*`"| WRK
    editComment -->|Updates| CMT
    comment -->|Adds| CMT
    enforce -->|Creates| ENF
```

## Source Test Compliance Review (STR) Workflow Additions

* An STR can only be entered from a Source Test Report.
* An STR is a Compliance Event.
* An STR is automatically closed when created.
* An STR can be printed if closed.
    * Closing an STR enables printing.
    * Reopening an STR disables printing.

### Flowchart

```mermaid
flowchart
    TST{{Source Test Report}}
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

    TST -.-> enter
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
    close --->|Closing updates| DX
```
