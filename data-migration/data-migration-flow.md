# Data migration flow

## New tables

* [x] `AspNetRoles` (automatically populated)
* [x] `AspNetUsers`
* [x] `AspNetUserRoles`
* [ ] `AuditPoints`
    * [ ] FCE
    * [ ] Case File
    * [ ] Work Entry
* [ ] `CaseFileComplianceEvents`
* [ ] `CaseFiles`
* [ ] `Comments`
    * [ ] FCE
    * [ ] Case File
    * [ ] Work Entry
* [ ] `EmailLogs`
* [ ] `EnforcementActionReviews`
* [ ] `EnforcementActions`
* [ ] `Fces`
* [x] `Lookups`
* [ ] `StipulatedPenalties`
* [x] `ViolationTypes`
* [ ] `WorkEntries`

## Old tables

* `LK_VIOLATION_TYPE`
* `LOOKUPSSCPNOTIFICATIONS`
* `SSCPACCS`
* `SSCPACCSHISTORY` *(not migrated)*
* `SSCPENFORCEMENTSTIPULATED`
* `SSCPFCEMASTER`
* `SSCPFCE`
* `SSCPINSPECTIONS`
* `SSCPITEMMASTER`
* `SSCPNOTIFICATIONS`
* `SSCPREPORTS`
* `SSCPTESTREPORTS`
* `SSCP_AUDITEDENFORCEMENT`
* `SSCP_ENFORCEMENT` *(not migrated)*
* `SSCP_EnforcementEvents`

## General migration flow

```mermaid
---
title: Lookup tables
---
flowchart LR
    LK_VIOLATION_TYPE --> ViolationTypes
    LOOKUPSSCPNOTIFICATIONS --> Lookups:NotificationType
    LOOKUPEPDPROGRAMS/LOOKUPEPDUNITS --> Lookups:Office
```

```mermaid
---
title: FCEs
---
flowchart LR
    SSCPFCEMASTER --> Fces
    SSCPFCE --> Fces
```

```mermaid
---
title: Compliance Work
---
flowchart LR
    SSCPACCS --> WorkEntries
    SSCPINSPECTIONS --> WorkEntries
    SSCPITEMMASTER --> WorkEntries
    SSCPNOTIFICATIONS --> WorkEntries
    SSCPREPORTS --> WorkEntries
    SSCPTESTREPORTS --> WorkEntries
```

```mermaid
---
title: Compliance Work
---
flowchart LR
    SSCP_AUDITEDENFORCEMENT --> CaseFiles
    SSCP_AUDITEDENFORCEMENT --> EnforcementActionReviews
    SSCP_AUDITEDENFORCEMENT --> EnforcementActions
    SSCPENFORCEMENTSTIPULATED --> StipulatedPenalties
```

```mermaid
---
title: Audit Points
---
flowchart LR
    SSCPITEMMASTER --> AuditPoints
    SSCPFCEMASTER --> AuditPoints
    SSCP_AUDITEDENFORCEMENT --> AuditPoints
```
