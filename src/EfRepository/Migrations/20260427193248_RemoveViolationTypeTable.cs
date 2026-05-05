using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveViolationTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementCaseFiles_ViolationTypes_ViolationTypeCode",
                table: "EnforcementCaseFiles");

            migrationBuilder.DropTable(
                name: "ViolationTypes");

            migrationBuilder.DropIndex(
                name: "IX_EnforcementCaseFiles_ViolationTypeCode",
                table: "EnforcementCaseFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViolationTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Deprecated = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SeverityCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationTypes", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementCaseFiles_ViolationTypeCode",
                table: "EnforcementCaseFiles",
                column: "ViolationTypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementCaseFiles_ViolationTypes_ViolationTypeCode",
                table: "EnforcementCaseFiles",
                column: "ViolationTypeCode",
                principalTable: "ViolationTypes",
                principalColumn: "Code");
        }
    }
}
