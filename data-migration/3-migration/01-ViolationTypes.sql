use [air-web]
go

-- Data pulled from airbranch.dbo.LK_VIOLATION_TYPE

INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'FCIO', N'Failure to construct, install or operate facility/equipment in accordance with permit or regulation', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'FMPR', N'Failure to maintain records as required by permit or regulation', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'FOMP', N'Failure to obtain or maintain a current permit', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'FRPR', N'Failure to report as required by permit or regulation', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'FTCM', N'Failure to test or conduct monitoring as required by permit or regulation', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VDOA', N'Violation of consent decree, court order, or administrative order', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VLSP', N'Violation of emission limit, emission standard, surrogate parameter', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VWPS', N'Violation of Work Practice Standard', N'FRV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'CRIT1', N'Criteria 1 - Failure to obtain NSR permit and/or install BACT/LAER', N'HPV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'CRIT2', N'Criteria 2 - NSR/PSD/SIP, violation of emission limit, emission standard, or surrogate parameter', N'HPV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'CRIT3', N'Criteria 3 - NSPS, violation of emission limit, emission standard, surrogate parameter', N'HPV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'CRIT4', N'Criteria 4 - NESHAP, violation of emission limit, emission standard, surrogate parameter', N'HPV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'CRIT5', N'Criteria 5 - Violation of work practice standard, testing requirements, monitoring requirements, recordkeeping or reporting requirements', N'HPV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'CRIT6', N'Criteria 6 - Other HPV', N'HPV', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VFEV', N'Violation of Federally-Enforceable Rule or Regulation that is not federally-reportable', N'NFR', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VSTL', N'Violation of State, Tribal or Local Agency Only Regulation', N'OTH', 0);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'NMS', N'Historic - In Violation Not Meeting Schedule (6)', N'FRV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'RPC', N'Historic - In Violation with Regard to Procedural Compliance (W)', N'FRV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'URG', N'Historic - In Violation Unknown with Regard to Schedule (7)', N'FRV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VEPC', N'Historic - In Violation with Regard to Both Emissions and Procedural Compliance (B)', N'FRV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'VNS1', N'Historic - In Violation No Schedule (1)', N'FRV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'DIS', N'Historic - Discretionary HPV', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'G10', N'Historic - Section 112(r) Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC1', N'Historic - Fail to Obtain PSD or NSR Permit', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC2', N'Historic - Violation Of Air Toxics Requirements', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC3', N'Historic - Violation that Affects Synthetic Minor Status', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC4', N'Historic - Enforcement Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC5', N'Historic - Title V Certification Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC6', N'Historic - Title V Permit Application Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC7', N'Historic - Testing, Monitoring, Recordkeeping, or Reporting Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC8', N'Historic - Emission Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'GC9', N'Historic - Chronic or Recalcitrant Violation', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M1A', N'Historic - Any violation of emission limit detected via stack testing', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M1B', N'Historic - Violation of emission limits > 15% via sampling', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M1C', N'Historic - Violation of emission limits > the SST (Supplemental Significant Threshold)', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M2A', N'Historic - Violation of Direct Surrogate for >5% of limit for >3% of OT (operating time)', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M2B', N'Historic - Violation of Direct Surrogate for >50% of OT (operating time)', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M2C', N'Historic - Violation of Direct Surrogate of >25% for 2 reporting periods', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M3A', N'Historic - Violation of non-opacity standard via CEM of >15% for >5% of operating time', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M3B', N'Historic - Violation of non- opacity standard via CEM of the SST(Supplemental Significant Threshold)', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M3C', N'Historic - Violation of non-opacity standard via CEM of >15% for 2 reporting periods', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M3D', N'Historic - Violation of non-opacity standard via CEM of >50% of the operating time during the reporting period', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M3E', N'Historic - Violation of non-opacity standard via CEM of >25% during two consecutive reporting periods', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M3F', N'Historic - Any violation of non-opacity (>24 hours standard) via CEM', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M4A', N'Historic - Violation of opacity standards (0-20%) via Continuous Opacity Monitoring (COM)', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M4B', N'Historic - Violations of opacity standards >3% of operating time via Continuous Opacity Monitoring during two consecutive reporting periods', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M4C', N'Historic - Violation of opacity standards (> 20%) via Continuous Opacity Monitoring (COM) for >5% of operating Time', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M4D', N'Historic - Violation of opacity standards (>20%) via Continuous Opacity Monitoring (COM) for 5% operating time', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M4E', N'Historic - Violation of opacity standards (0-20%) via Method 9 VE Readings', N'HPV', 1);
INSERT INTO dbo.ViolationTypes (Code, Description, SeverityCode, Deprecated) VALUES (N'M4F', N'Historic - Violation of opacity standards (>20%) via Method 9 VE Readings', N'HPV', 1);
