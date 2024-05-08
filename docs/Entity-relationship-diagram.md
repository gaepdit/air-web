# Entity relationship diagram

## Entities

### Mirrored from IAIP

- FAC: Facility
- MON: Source Monitoring (Performance Test/Stack Test)

### Migrated into the Air Web app

- FCE: Full Compliance Evaluation (FCE)
- WRK: Work Entry
    - CME: Compliance Event
        - ACC: Annual Compliance Certification (ACC)
        - INS: Inspection
        - RMP: RMP Inspection
        - SMR: Source Monitoring Review
        - REP: Report
    - NOT: Notification
    - PX: Permit revocation †
- ENF: Enforcement

† Indicates a change in hierarchy compared to the IAIP.

## SSCP Diagram

```mermaid
erDiagram

FAC["Facility"] {
    string airsNumber PK
}

MON["Source Monitoring"] {
    string referenceNumber PK
    string airsNumber FK
}

FCE["FCE"] {
    int Id PK
    string airsNumber FK
}

WRK["Work Entry"] {
    int Id PK
    string airsNumber FK
}

    CME["Compliance Event"]

        SMR["Source Monitoring Review"] {
            string referenceNumber FK
        }
        ACC["ACC"]
        INS["Inspection"]
        RMP["RMP Inspection"]
        REP["Report"]

    NOT["Notification"]

COM["Comment"] {
    Guid Id PK
    int workEntryId FK
}

ENF["Enforcement"] {
    int Id PK
    string airsNumber FK
    int workEntryId FK
}


SMR |o--|| CME : "is a type of"
ACC |o--|| CME : "is a type of"
INS |o--|| CME : "is a type of"
RMP |o--|| CME : "is a type of"
REP |o--|| CME : "is a type of"

COM }o--|| WRK : "comments on"

CME |o--|| WRK : "is a type of"
NOT |o--|| WRK : "is a type of"

SMR |o--|| MON : "evaluates"

MON }o--|| FAC : "is conducted at"
WRK }o--|| FAC : "is entered for"
FCE }o--|| FAC : "is completed for"
ENF }o--|| FAC : "is issued to"

ENF }o..|{ CME : "addresses"

```
