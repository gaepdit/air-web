using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureSparseColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, drop indexes that interfere with setting up sparse columns.
            // Ref: https://github.com/dotnet/efcore/issues/38760
            migrationBuilder.DropIndex(
                name: "IX_ComplianceWork_DeletedById",
                table: "ComplianceWork");
            migrationBuilder.DropIndex(
                name: "IX_Fces_DeletedById",
                table: "Fces");
            migrationBuilder.DropIndex(
                name: "IX_Comments_CaseFileId",
                table: "Comments");
            migrationBuilder.DropIndex(
                name: "IX_Comments_FceId",
                table: "Comments");
            migrationBuilder.DropIndex(
                name: "IX_AuditPoints_CaseFileId",
                table: "AuditPoints");
            migrationBuilder.DropIndex(
                name: "IX_AuditPoints_FceId",
                table: "AuditPoints");

            // Next, set up sparse columns.
            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "StipulatedPenalties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "StipulatedPenalties",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "Fces",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Fces",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeleteComments",
                table: "Fces",
                type: "nvarchar(max)",
                maxLength: 7000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 7000,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "WeatherConditions",
                table: "ComplianceWork",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportingPeriodComment",
                table: "ComplianceWork",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceNumber",
                table: "ComplianceWork",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReceivedByComplianceDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PostmarkDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PhysicalShutdownDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PermitRevocationDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionStarted",
                table: "ComplianceWork",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "InspectionReason",
                table: "ComplianceWork",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "InspectionGuide",
                table: "ComplianceWork",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionEnded",
                table: "ComplianceWork",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "ComplianceWork",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "ComplianceWork",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeleteComments",
                table: "ComplianceWork",
                type: "nvarchar(max)",
                maxLength: 7000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 7000,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "AccReportingYear",
                table: "ComplianceWork",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "FceId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Comments",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "CaseFileId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "MoreInfo",
                table: "AuditPoints",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "FceId",
                table: "AuditPoints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "CaseFileId",
                table: "AuditPoints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            // Finally, recreate the indexes.
            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_DeletedById",
                table: "ComplianceWork",
                column: "DeletedById");
            migrationBuilder.CreateIndex(
                name: "IX_Fces_DeletedById",
                table: "Fces",
                column: "DeletedById");
            migrationBuilder.CreateIndex(
                name: "IX_Comments_CaseFileId",
                table: "Comments",
                column: "CaseFileId");
            migrationBuilder.CreateIndex(
                name: "IX_Comments_FceId",
                table: "Comments",
                column: "FceId");
            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_CaseFileId",
                table: "AuditPoints",
                column: "CaseFileId");
            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_FceId",
                table: "AuditPoints",
                column: "FceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // I'm assuming the indexes need to be dropped and recreated here as well.
            migrationBuilder.DropIndex(
                name: "IX_ComplianceWork_DeletedById",
                table: "ComplianceWork");
            migrationBuilder.DropIndex(
                name: "IX_Fces_DeletedById",
                table: "Fces");
            migrationBuilder.DropIndex(
                name: "IX_Comments_CaseFileId",
                table: "Comments");
            migrationBuilder.DropIndex(
                name: "IX_Comments_FceId",
                table: "Comments");
            migrationBuilder.DropIndex(
                name: "IX_AuditPoints_CaseFileId",
                table: "AuditPoints");
            migrationBuilder.DropIndex(
                name: "IX_AuditPoints_FceId",
                table: "AuditPoints");

            // Disable sparse columns.
            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "StipulatedPenalties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "StipulatedPenalties",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "Fces",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Fces",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeleteComments",
                table: "Fces",
                type: "nvarchar(max)",
                maxLength: 7000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 7000,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "WeatherConditions",
                table: "ComplianceWork",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportingPeriodComment",
                table: "ComplianceWork",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceNumber",
                table: "ComplianceWork",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReceivedByComplianceDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PostmarkDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PhysicalShutdownDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PermitRevocationDate",
                table: "ComplianceWork",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionStarted",
                table: "ComplianceWork",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "InspectionReason",
                table: "ComplianceWork",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "InspectionGuide",
                table: "ComplianceWork",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InspectionEnded",
                table: "ComplianceWork",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "ComplianceWork",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "ComplianceWork",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeleteComments",
                table: "ComplianceWork",
                type: "nvarchar(max)",
                maxLength: 7000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 7000,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "AccReportingYear",
                table: "ComplianceWork",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "FceId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedById",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Comments",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "CaseFileId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "MoreInfo",
                table: "AuditPoints",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "FceId",
                table: "AuditPoints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "CaseFileId",
                table: "AuditPoints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            // Recreate the indexes.
            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_DeletedById",
                table: "ComplianceWork",
                column: "DeletedById");
            migrationBuilder.CreateIndex(
                name: "IX_Fces_DeletedById",
                table: "Fces",
                column: "DeletedById");
            migrationBuilder.CreateIndex(
                name: "IX_Comments_CaseFileId",
                table: "Comments",
                column: "CaseFileId");
            migrationBuilder.CreateIndex(
                name: "IX_Comments_FceId",
                table: "Comments",
                column: "FceId");
            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_CaseFileId",
                table: "AuditPoints",
                column: "CaseFileId");
            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_FceId",
                table: "AuditPoints",
                column: "FceId");
        }
    }
}
