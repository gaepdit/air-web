using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataExchangeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActionNumber",
                table: "Fces",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataExchangeStatusDate",
                table: "Fces",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActionNumber",
                table: "EnforcementActions",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataExchangeStatusDate",
                table: "EnforcementActions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacilityId",
                table: "EnforcementActions",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ActionNumber",
                table: "ComplianceWork",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataExchangeStatusDate",
                table: "ComplianceWork",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActionNumber",
                table: "CaseFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataExchangeStatusDate",
                table: "CaseFiles",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fces_FacilityId_ActionNumber",
                table: "Fces",
                columns: new[] { "FacilityId", "ActionNumber" },
                unique: true,
                filter: "[ActionNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_FacilityId_ActionNumber",
                table: "ComplianceWork",
                columns: new[] { "FacilityId", "ActionNumber" },
                unique: true,
                filter: "[ActionNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFiles_FacilityId_ActionNumber",
                table: "CaseFiles",
                columns: new[] { "FacilityId", "ActionNumber" },
                unique: true,
                filter: "[ActionNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fces_FacilityId_ActionNumber",
                table: "Fces");

            migrationBuilder.DropIndex(
                name: "IX_ComplianceWork_FacilityId_ActionNumber",
                table: "ComplianceWork");

            migrationBuilder.DropIndex(
                name: "IX_CaseFiles_FacilityId_ActionNumber",
                table: "CaseFiles");

            migrationBuilder.DropColumn(
                name: "ActionNumber",
                table: "Fces");

            migrationBuilder.DropColumn(
                name: "DataExchangeStatusDate",
                table: "Fces");

            migrationBuilder.DropColumn(
                name: "DataExchangeStatusDate",
                table: "EnforcementActions");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "EnforcementActions");

            migrationBuilder.DropColumn(
                name: "ActionNumber",
                table: "ComplianceWork");

            migrationBuilder.DropColumn(
                name: "DataExchangeStatusDate",
                table: "ComplianceWork");

            migrationBuilder.DropColumn(
                name: "DataExchangeStatusDate",
                table: "CaseFiles");

            migrationBuilder.AlterColumn<short>(
                name: "ActionNumber",
                table: "EnforcementActions",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "ActionNumber",
                table: "CaseFiles",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
