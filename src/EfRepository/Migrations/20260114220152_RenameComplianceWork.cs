using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class RenameComplianceWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPoints_ComplianceWork_WorkEntryId",
                table: "AuditPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ComplianceWork_WorkEntryId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "WorkEntryType",
                table: "ComplianceWork",
                newName: "ComplianceWorkType");

            migrationBuilder.RenameColumn(
                name: "WorkEntryId",
                table: "Comments",
                newName: "ComplianceWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_WorkEntryId",
                table: "Comments",
                newName: "IX_Comments_ComplianceWorkId");

            migrationBuilder.RenameColumn(
                name: "WorkEntryId",
                table: "AuditPoints",
                newName: "ComplianceWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditPoints_WorkEntryId",
                table: "AuditPoints",
                newName: "IX_AuditPoints_ComplianceWorkId");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AuditPoints",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPoints_ComplianceWork_ComplianceWorkId",
                table: "AuditPoints",
                column: "ComplianceWorkId",
                principalTable: "ComplianceWork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ComplianceWork_ComplianceWorkId",
                table: "Comments",
                column: "ComplianceWorkId",
                principalTable: "ComplianceWork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPoints_ComplianceWork_ComplianceWorkId",
                table: "AuditPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ComplianceWork_ComplianceWorkId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ComplianceWorkType",
                table: "ComplianceWork",
                newName: "WorkEntryType");

            migrationBuilder.RenameColumn(
                name: "ComplianceWorkId",
                table: "Comments",
                newName: "WorkEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ComplianceWorkId",
                table: "Comments",
                newName: "IX_Comments_WorkEntryId");

            migrationBuilder.RenameColumn(
                name: "ComplianceWorkId",
                table: "AuditPoints",
                newName: "WorkEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditPoints_ComplianceWorkId",
                table: "AuditPoints",
                newName: "IX_AuditPoints_WorkEntryId");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AuditPoints",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPoints_ComplianceWork_WorkEntryId",
                table: "AuditPoints",
                column: "WorkEntryId",
                principalTable: "ComplianceWork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ComplianceWork_WorkEntryId",
                table: "Comments",
                column: "WorkEntryId",
                principalTable: "ComplianceWork",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
