using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class EnforcementTemporalTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "EnforcementCaseFiles")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EnforcementCaseFiles_History")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterTable(
                name: "EnforcementActions")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EnforcementActions_History")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodEnd",
                table: "EnforcementCaseFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodStart",
                table: "EnforcementCaseFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodEnd",
                table: "EnforcementActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodStart",
                table: "EnforcementActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodEnd",
                table: "EnforcementCaseFiles")
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.DropColumn(
                name: "PeriodStart",
                table: "EnforcementCaseFiles")
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.DropColumn(
                name: "PeriodEnd",
                table: "EnforcementActions")
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.DropColumn(
                name: "PeriodStart",
                table: "EnforcementActions")
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.AlterTable(
                name: "EnforcementCaseFiles")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "EnforcementCaseFiles_History")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterTable(
                name: "EnforcementActions")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "EnforcementActions_History")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
