# Enforcement Workflow

## Case File

* A new Case File can be entered from a Facility or Compliance Event.
* A Case File can be linked to multiple Compliance Events.
* The Case File can be edited while open.
* Closing/finalizing a Case File disables all editing.
* Reopening a Case File enables all editing.
* Comments can be added and edited.
* A Comment can be deleted *(not shown in diagram)*.
* A Case File can be deleted *(not shown)*.

## Enforcement Action

* An Enforcement Action can be added to an open Case File.
* An Enforcement Action can be edited while the Case File is open.
* An Enforcement Action can be closed as unsent.
* Issuing an Enforcement Action closes it and disables all editing (including deleting).
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.
* An Enforcement Action can be deleted *(not shown)*.

## Enforcement Action Review

* An Enforcement Action can be submitted for review, creating an Enforcement Action Review.
* An Enforcement Action Review can be completed, updating the Enforcement Action.

## Data Exchange

* When an Informal or Formal Enforcement Action (EA) exists and a Compliance Event is linked, the Data Exchange is
  enabled for the Case File *(not shown)*.

| Item                          |  ICIS-Air Data Type   |        Pathway Activity *         |
|-------------------------------|:---------------------:|:---------------------------------:|
| Case File                     |       Case File       |                                   |
| Compliance Event              | Compliance Monitoring |             Discovery             |
| Notice of Violation           |      Informal EA      |           Notification            |
| No Further Action Letter      |                       |      Addressing & Resolving       |
| Combined NOV/NFA Letter       |      Informal EA      | Notification/Addressing/Resolving |
| Proposed Consent Order        |      Informal EA      |           Notification            |
| Consent Order                 |       Formal EA       |            Addressing             |
| Consent Order Resolved        |                       |             Resolving             |
| Administrative Order          |       Formal EA       |            Addressing             |
| Administrative Order Resolved |                       |             Resolving             |

<small>
* Indicates Pathway Activities tracked for each Case File.
</small>

## Flow Chart

```mermaid
flowchart
    EVT{{Compliance Event}}
    FAC{{Facility}}
    ENF{{"`**Case File**`"}}
    CTE{{Enforcement Comment}}
    ACT{{"`**Enforcement Action**`"}}
    REV{{"Enforcement Action Review"}}
    CTA{{Action Comment}}
    STP{{Stipulated Penalty}}
    link([Link Event])
    add([Enter new Case File])
    comment([Add Comment])
    editEnf([Edit])
    addAction([Add Action])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    editAction([Edit Action])
    commentAction([Add Comment])
    editCommentAction([Edit Comment])
    review([Submit for Review])
    respond([Approve/Return])
    issue([Issue])
    penalty([Add Penalty])
    IfCO>If Consent Order]
    FAC -.-> add
    ENF -.-> link
    EVT -.-> add
    ENF -.-> editEnf
    ENF -.-> close
    ENF -..-> comment
    ENF -.-> addAction
    CTE -.-> editComment
    ACT -..-> commentAction
    ACT -.-> editAction
    ACT -.-> review
    ACT -.-> issue
    CTA -.-> editCommentAction
    REV -.-> respond
    ACT -.-> IfCO -.-> penalty
    close -->|"`Disables/*enables*`"| addAction
    close -->|"`Disables/*enables*`"| editEnf
    close -->|"`Disables/*enables*`"| link
    issue -->|Disables| editAction
    issue -->|Disables| review
    add -->|Creates| ENF
    addAction -->|Adds| ACT
    close -->|"`Closes/*reopens*`"| ENF
    close -->|Disables| penalty
    comment -->|Adds| CTE
    commentAction -->|Adds| CTA
    editAction -->|Updates| ACT
    editComment -->|Updates| CTE
    editCommentAction -->|Updates| CTA
    editEnf -->|Updates| ENF
    issue -->|Updates status| ENF
    link -->|Links to| EVT
    penalty -->|Adds| STP
    respond -->|Updates| ACT
    review -->|Starts| REV

```
