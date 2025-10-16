# Enforcement Case Workflow

Case Files address non-compliance. They are linked to a single facility and may be linked to one or more Compliance
Events. One or more Enforcement Actions will be issued for a Case File, some of which can be considered to resolve the
Case File. Once all work on a Case File is complete, it can be closed.

## Case File

* A new Case File can be entered from a Facility or Compliance Event.
* A Case File can be linked to multiple Compliance Events.
* The Case File can be edited while open.
* Closing/finalizing a Case File disables all editing.
* Reopening a Case File re-enables all editing.
* Comments can be added.
* A Comment can be deleted *(not shown in diagram)*.
* A Case File can be deleted *(not shown in diagram)*.

### Case File Status

The Case File status depends (in part) on the status of its Enforcement Actions.

- *Draft* - Case File has no issued Enforcement Actions.
- *Open* - Case File has at least one issued Enforcement Action.
- *Subject to compliance schedule* - Case File has at least one Formal Enforcement Action (see Data Exchange table
  below) that has been executed.
- *Closed* - Case File is closed.

```mermaid
flowchart LR
    d[Draft] --> c[Closed]
    d --> o[Open]
    o --> c
    o --> s[Compliance Schedule]
    s --> c
```

## Enforcement Action

* An Enforcement Action can be added to an open Case File.
* An Enforcement Action can be edited while the Case File is open.
* An Enforcement Action can be submitted for review, creating an Enforcement Action Review.
* An Enforcement Action Review can be completed, updating the Enforcement Action status.
* An Enforcement Action can be issued (sent to facility) or canceled (closed as unsent), both of which disable the
  review
  process.
* An Enforcement Action can be deleted *(not shown in diagram)*.

### Enforcement Action Types

* Informational Letter
* Letter of Noncompliance (LON)
* No Further Action Letter (NFA)
* Notice of Violation (NOV)
* Combined NOV/NFA Letter
* Proposed Consent Order (PCO)
* Consent Order (CO)
* Administrative Order (AO)

### Enforcement Action Type-Specific Logic

* An LON or a Combined NOV/NFA letter cannot be added if a reportable Enforcement Action (see Data Exchange table below)
  has already been issued.
* An NFA cannot be added directly to a Case File. It can only be generated from an existing NOV from the Actions menu.
* A CO cannot be added directly to a Case File. It can only be generated from an existing PCO from the Actions menu.
* Stipulated Penalties can be added to a Consent Order.
* A Stipulated Penalty can be deleted *(not shown in diagram)*.

### Enforcement Action Status

```mermaid
flowchart LR
    d[Draft] --> i[Issued]
    d --> c[Canceled]
    d --> r[Under Review]
    r --> c
    r --> a[Approved]
    a --> i
```

## Case File Process Flow Chart

```mermaid
flowchart
    FAC{{Facility}}
    EVT{{Compliance Event}}
    ENF{{"`**Case File**`"}}
    CTE{{Case File Comment}}
    ACT{{"`**Enforcement Action**`"}}
    link([Link Event])
    add([Enter new Case File])
    comment([Add Comment])
    editEnf([Edit])
    addAction([Add Action])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    editAction([Edit Action])
    issue([Issue or Cancel])
    FAC -.-> add
    ENF -.-> link
    EVT -.-> add
    ENF -.-> editEnf
    ENF -.-> close
    ENF -..-> comment
    ENF -.-> addAction
    CTE -.-> editComment
    ACT -.-> editAction
    ACT -.-> issue
    close -->|"`Disables/*enables*`"| addAction
    close -->|"`Disables/*enables*`"| editEnf
    close -->|"`Disables/*enables*`"| link
    close -->|"`Disables/*enables*`"| editAction
    add -->|Adds| ENF
    addAction -->|Adds| ACT
    close -->|"`Closes/*reopens*`"| ENF
    comment -->|Adds| CTE
    editAction -->|Updates| ACT
    editComment -->|Updates| CTE
    editEnf -->|Updates| ENF
    issue -->|Updates status| ACT
    issue -->|Updates status| ENF
    link -->|Links to| EVT

```

### Consent Order Process Flow Chart

```mermaid
flowchart
    ENF{{Case File}}
    PCO{{"`**Proposed CO**`"}}
    CO{{"`**Consent Order**`"}}
    STP{{Stipulated Penalty}}
    addPCO([Add Proposed CO])
    issuePCO([Issue PCO])
    signed([Signed Order received])
    execute([Execute Order])
    issueCO([Issue CO])
    penalty([Add Penalty])
    ENF -.-> addPCO
    PCO -.-> issuePCO
    PCO -.->|"`*only if Issued*`"| signed
    CO -.-> execute
    CO -.->|"`*only if Executed*`"| issueCO
    CO -.->|"`*only if Executed*`"| penalty
    addPCO -->|Adds| PCO
    signed -->|Adds| CO
    penalty -->|Adds| STP

```

### Enforcement Action Review Process Flow Chart

```mermaid
flowchart
    ACT{{"`**Enforcement Action**`"}}
    REV{{"Enforcement Action Review"}}
    review([Request Review])
    respond([Submit Review])
    issue([Issue or Cancel])
    ACT -.-> review
    ACT -.-> issue
    REV -.-> respond
    issue -->|Disables| review
    respond -->|Updates| ACT
    review -->|Starts| REV

```

## Data Exchange

When an Informal or Formal Enforcement Action (EA) exists and a Compliance Event is linked, the Data Exchange is
enabled for the Case File.

| Enforcement Action type    | Not reportable | Informal reportable | Formal reportable |
|----------------------------|:--------------:|:-------------------:|:-----------------:|
| Letter of Noncompliance    |       ✓        |                     |                   |
| Notice of Violation        |                |          ✓          |                   |
| No Further Action Letter   |       ✓        |                     |                   |
| Combined NOV/NFA Letter    |                |          ✓          |                   |
| Proposed Consent Order     |                |          ✓          |                   |
| Consent Order              |                |                     |         ✓         |
| Administrative Order       |                |                     |         ✓         |
| Order Resolved  (CO or AO) |       ✓        |                     |                   |
| Informational Letter       |       ✓        |                     |                   |

### ICIS-Air Data Exchange Activities

| Item                          | ICIS-Air Data Type    | Pathway Activity *                |
|-------------------------------|-----------------------|-----------------------------------|
| Case File                     | Case File             | *N/A*                             |
| Compliance Event              | Compliance Monitoring | Discovery                         |
| Notice of Violation           | Informal EA           | Notification                      |
| No Further Action Letter      | *N/A*                 | Addressing & Resolving            |
| Combined NOV/NFA Letter       | Informal EA           | Notification/Addressing/Resolving |
| Proposed Consent Order        | Informal EA           | Notification                      |
| Consent Order                 | Formal EA             | Addressing                        |
| Consent Order Resolved        | *N/A*                 | Resolving                         |
| Administrative Order          | Formal EA             | Addressing                        |
| Administrative Order Resolved | *N/A*                 | Resolving                         |

<small>
* Indicates Pathway Activities tracked for each Case File.
</small>
