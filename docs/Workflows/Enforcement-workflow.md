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

## Enforcement Action

* An Enforcement Action can be added to an Enforcement.
* An Enforcement Action can be edited while the Enforcement is open.
* Issuing an Enforcement Action closes it and disables all editing.
* Comments can be added and edited.
* A Comment can be deleted *(not shown)*.
* An Enforcement Action can be deleted *(not shown)*.

## Enforcement Action Review

* An Enforcement Action can be submitted for review, creating an Enforcement Action Review.
* An Enforcement Action Review can be completed, updating the Enforcement Action.

## Data Exchange/Internal Auditing

* Any of the following will update the Data Exchange and generate an audit point *(not shown)*:
    * Adding or editing an Enforcement or Enforcement Action.
    * Closing or reopening the Enforcement.
    * Linking a Compliance Event.
    * Issuing an Enforcement Action.

```mermaid
flowchart
    EVT{{Compliance Event}}
    FAC{{Facility}}
    ENF{{"`**Enforcement**`"}}
    CMT{{Enforcement Comment}}
    ACT{{"`**Enforcement Action**`"}}
    REV{{"Enforcement Action Review"}}
    CMA{{Action Comment}}

    link([Link Event])
    add([Enter new LON/Case File])
    comment([Add Comment])
    editEnf([Edit])
    addAction([Add Enforcement Action])
    close(["`Close/*Reopen*`"])
    editComment([Edit Comment])
    editAction([Edit Action])
    commentAction([Add Comment])
    editCommentAction([Edit Action Comment])
    review([Submit for Review])
    respond([Approve/Return])
    issue([Issue Action])

    ENF -.-> link
    FAC -.-> add
    EVT -.-> add
    ENF -.-> editEnf
    ENF -.-> close
    ENF -..-> comment
    ENF -.-> addAction
    CMT -.-> editComment
    ACT -.-> commentAction
    ACT -.-> editAction
    ACT -.-> review
    ACT -.-> issue
    CMA -.-> editCommentAction
    REV -.-> respond

    close -->|"`Disables/*enables*`"| editEnf
    close -->|"`Disables/*enables*`"| addAction
    close -->|"`Disables/*enables*`"| link
    issue -->|Disables| editAction
    issue -->|Disables| commentAction
    issue -->|Disables| review

    link -->|Links To| EVT
    add -->|Creates| ENF
    editEnf -->|Updates| ENF
    editAction -->|Updates| ACT
    issue -->|Closes| ACT
    close -->|"`Closes/*reopens*`"| ENF
    editComment -->|Updates| CMT
    editCommentAction -->|Updates| CMA
    comment -->|Adds| CMT
    commentAction -->|Adds| CMA
    addAction -->|Adds| ACT
    review -->|Starts| REV
    respond -->|Updates| ACT

```
