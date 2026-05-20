using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddViolationTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViolationTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Deprecated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationTypes", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "ViolationTypes",
                columns: new[] { "Code", "Deprecated", "Description", "Severity" },
                values: new object[,]
                {
                    { "CRIT1", false, "Criteria 1 - Failure to obtain NSR permit and/or install BACT/LAER", "HPV" },
                    { "CRIT2", false, "Criteria 2 - NSR/PSD/SIP, violation of emission limit, emission standard, or surrogate parameter", "HPV" },
                    { "CRIT3", false, "Criteria 3 - NSPS, violation of emission limit, emission standard, surrogate parameter", "HPV" },
                    { "CRIT4", false, "Criteria 4 - NESHAP, violation of emission limit, emission standard, surrogate parameter", "HPV" },
                    { "CRIT5", false, "Criteria 5 - Violation of work practice standard, testing requirements, monitoring requirements, recordkeeping or reporting requirements", "HPV" },
                    { "CRIT6", false, "Criteria 6 - Other HPV", "HPV" },
                    { "DIS", true, "Historic - Discretionary HPV", "HPV" },
                    { "FCIO", false, "Failure to construct, install or operate facility/equipment in accordance with permit or regulation", "FRV" },
                    { "FMPR", false, "Failure to maintain records as required by permit or regulation", "FRV" },
                    { "FOMP", false, "Failure to obtain or maintain a current permit", "FRV" },
                    { "FRPR", false, "Failure to report as required by permit or regulation", "FRV" },
                    { "FTCM", false, "Failure to test or conduct monitoring as required by permit or regulation", "FRV" },
                    { "G10", true, "Historic - Section 112(r) Violation", "HPV" },
                    { "GC1", true, "Historic - Fail to Obtain PSD or NSR Permit", "HPV" },
                    { "GC2", true, "Historic - Violation Of Air Toxics Requirements", "HPV" },
                    { "GC3", true, "Historic - Violation that Affects Synthetic Minor Status", "HPV" },
                    { "GC4", true, "Historic - Enforcement Violation", "HPV" },
                    { "GC5", true, "Historic - Title V Certification Violation", "HPV" },
                    { "GC6", true, "Historic - Title V Permit Application Violation", "HPV" },
                    { "GC7", true, "Historic - Testing, Monitoring, Recordkeeping, or Reporting Violation", "HPV" },
                    { "GC8", true, "Historic - Emission Violation", "HPV" },
                    { "GC9", true, "Historic - Chronic or Recalcitrant Violation", "HPV" },
                    { "M1A", true, "Historic - Any violation of emission limit detected via stack testing", "HPV" },
                    { "M1B", true, "Historic - Violation of emission limits > 15% via sampling", "HPV" },
                    { "M1C", true, "Historic - Violation of emission limits > the SST (Supplemental Significant Threshold)", "HPV" },
                    { "M2A", true, "Historic - Violation of Direct Surrogate for >5% of limit for >3% of OT (operating time)", "HPV" },
                    { "M2B", true, "Historic - Violation of Direct Surrogate for >50% of OT (operating time)", "HPV" },
                    { "M2C", true, "Historic - Violation of Direct Surrogate of >25% for 2 reporting periods", "HPV" },
                    { "M3A", true, "Historic - Violation of non-opacity standard via CEM of >15% for >5% of operating time", "HPV" },
                    { "M3B", true, "Historic - Violation of non- opacity standard via CEM of the SST(Supplemental Significant Threshold)", "HPV" },
                    { "M3C", true, "Historic - Violation of non-opacity standard via CEM of >15% for 2 reporting periods", "HPV" },
                    { "M3D", true, "Historic - Violation of non-opacity standard via CEM of >50% of the operating time during the reporting period", "HPV" },
                    { "M3E", true, "Historic - Violation of non-opacity standard via CEM of >25% during two consecutive reporting periods", "HPV" },
                    { "M3F", true, "Historic - Any violation of non-opacity (>24 hours standard) via CEM", "HPV" },
                    { "M4A", true, "Historic - Violation of opacity standards (0-20%) via Continuous Opacity Monitoring (COM)", "HPV" },
                    { "M4B", true, "Historic - Violations of opacity standards >3% of operating time via Continuous Opacity Monitoring during two consecutive reporting periods", "HPV" },
                    { "M4C", true, "Historic - Violation of opacity standards (> 20%) via Continuous Opacity Monitoring (COM) for >5% of operating Time", "HPV" },
                    { "M4D", true, "Historic - Violation of opacity standards (>20%) via Continuous Opacity Monitoring (COM) for 5% operating time", "HPV" },
                    { "M4E", true, "Historic - Violation of opacity standards (0-20%) via Method 9 VE Readings", "HPV" },
                    { "M4F", true, "Historic - Violation of opacity standards (>20%) via Method 9 VE Readings", "HPV" },
                    { "NMS", true, "Historic - In Violation Not Meeting Schedule (6)", "FRV" },
                    { "RPC", true, "Historic - In Violation with Regard to Procedural Compliance (W)", "FRV" },
                    { "URG", true, "Historic - In Violation Unknown with Regard to Schedule (7)", "FRV" },
                    { "VDOA", false, "Violation of consent decree, court order, or administrative order", "FRV" },
                    { "VEPC", true, "Historic - In Violation with Regard to Both Emissions and Procedural Compliance (B)", "FRV" },
                    { "VFEV", false, "Violation of Federally-Enforceable Rule or Regulation that is not federally-reportable", "NFR" },
                    { "VLSP", false, "Violation of emission limit, emission standard, surrogate parameter", "FRV" },
                    { "VNS1", true, "Historic - In Violation No Schedule (1)", "FRV" },
                    { "VSTL", false, "Violation of State, Tribal or Local Agency Only Regulation", "OTH" },
                    { "VWPS", false, "Violation of Work Practice Standard", "FRV" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViolationTypes");
        }
    }
}
