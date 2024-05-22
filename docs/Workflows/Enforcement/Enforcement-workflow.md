# Enforcement Workflow

## Enforcement Case

* A new Enforcement can be entered from a Facility or Compliance Event.
* An Enforcement can be linked to multiple Compliance Events.
* The Enforcement can be edited while open.
* Closing an Enforcement disables all editing.
* Reopening an Enforcement enables all editing.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.
* An Enforcement can be deleted *(not shown)*.

## Enforcement Action/Document

* An Enforcement Action can be added to an Enforcement.
* An Enforcement Action can be started from another Enforcement Action where a linkage exists (e.g., an NFA from an NOV
  or a public notice from a CO).
* An Enforcement Action can be edited while the Enforcement is open.
* Issuing an Enforcement Action closes it and disables all editing.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.
* An Enforcement Action can be deleted *(not shown)*.

## Enforcement Action Review

* An Enforcement Action can be submitted for review, creating an Enforcement Action Review.
* An Enforcement Action Review can be completed, updating the Enforcement Action.

## Data Exchange/Internal Auditing

* Submitting to EPA will enable the Data Exchange *(not shown)*.
* Any of the following will update the Data Exchange and generate an audit point *(not shown)*:
    * Adding or editing an Enforcement or Enforcement Action.
    * Closing or reopening the Enforcement.
    * Linking a Compliance Event.
    * Issuing an Enforcement Action.

## Flow Chart

```mermaid
flowchart
    EVT{{Compliance Event}}
    FAC{{Facility}}
    ENF{{"`**Enforcement Case**`"}}
    CTE{{Enforcement Comment}}
    ACT{{"`**Enforcement Action/Document**`"}}
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
    addLinkedAction([Add Linked Action])
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
    ACT -.-> addLinkedAction
    CTE -.-> editComment
    ACT -..-> commentAction
    ACT -.-> editAction
    ACT -.-> review
    ACT -.-> issue
    CTA -.-> editCommentAction
    REV -.-> respond
    ACT -.-> IfCO -.-> penalty

    close -->|"`Disables/*enables*`"| addAction
    close -->|"`Disables/*enables*`"| addLinkedAction
    close -->|"`Disables/*enables*`"| editEnf
    close -->|"`Disables/*enables*`"| link
    issue -->|Disables| editAction
    issue -->|Disables| review

    add -->|Creates| ENF
    addAction -->|Adds| ACT
    addLinkedAction -->|Adds| ACT
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
