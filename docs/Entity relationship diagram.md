# Entity relationship diagram

## Entities

### Mirrored from IAIP

- F: Facility
- T: Performance Test

### Migrated into the Air Web app

- V: Full Compliance Evaluation (FCE)
- W: Work Entry
    - C: Compliance Event
        - A: Annual Compliance Certification (ACC)
        - I: Inspection
        - M: RMP Inspection
        - P: Performance Test Review
        - R: Report
    - N: Notification
    - X: Permit revocation †
- E: Enforcement

† Indicates a change in hierarchy compared to the IAIP.

## Diagram

```mermaid
erDiagram

F["Facility"] {
    string airsNumber PK
}

T["Performance Test"] {
    string referenceNumber PK
    string airsNumber FK
}

W["Work Entry"] {
    int Id PK
    string airsNumber FK
}

V["FCE"] {
    int Id PK
    string airsNumber FK
}

    C["Compliance Event"]

        P["Performance Test Review"] {
            string referenceNumber FK
        }
        A["ACC"]
        I["Inspection"]
        M["RMP Inspection"]
        R["Report"]

    N["Notification"]

E["Enforcement"] {
    int Id PK
    string airsNumber FK
    int workEntryId FK
}


P }o--|| C : "is a type of"
A }o--|| C : "is a type of"
I }o--|| C : "is a type of"
M }o--|| C : "is a type of"
R }o--|| C : "is a type of"

C }o--|| W : "is a type of"
N }o--|| W : "is a type of"

P |o--|| T : "evaluates"

T }o--|| F : "is conducted at"
W }o--|| F : "is entered for"
V }o--|| F : "is completed for"
E }o--|| F : "is issued to"

E }o--|{ C : "addresses"

```
