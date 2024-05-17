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
    - REV: Permit revocation †
- ENF: Enforcement

† Indicates a change in hierarchy compared to the IAIP.

## SSCP Diagram

```mermaid
erDiagram

FAC["Facility"] {
    string facilityId PK
}

MON["Source Monitoring"] {
    int referenceNumber PK
    string facilityId FK
}

FCE["FCE"] {
    int Id PK
    string facilityId FK
}

WRK["Work Entry"] {
    int Id PK
    string facilityId FK
}

    CME["Compliance Event"]

        SMR["Source Monitoring Review"] {
            int referenceNumber FK
        }
        ACC["ACC"]
        INS["Inspection"]
        RMP["RMP Inspection"]
        REP["Report"]

    NOT["Notification"]
    
    REV["Permit Revocation"]

ENF["Enforcement"] {
    int Id PK
    string facilityId FK
}

CEL["Compliance/Enforcement Linkage"] {
    int enforcementId FK
    int workEntryId FK
}

SMR |o--|| CME : "is a type of"
ACC |o--|| CME : "is a type of"
INS |o--|| CME : "is a type of"
RMP |o--|| CME : "is a type of"
REP |o--|| CME : "is a type of"

CME |o--|| WRK : "is a type of"
NOT |o--|| WRK : "is a type of"
REV |o--|| WRK : "is a type of"

SMR |o--|| MON : "evaluates"

MON }o--|| FAC : "is conducted at"
WRK }o--|| FAC : "is entered for"
FCE }o--|| FAC : "is completed for"
ENF }o--|| FAC : "is issued to"

CEL }o--|| ENF : "addresses"
CEL }o--|| CME : "is linked to"

```
