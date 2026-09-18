using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddEnforcementActionNumberIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActions_FacilityId_ActionNumber",
                table: "EnforcementActions",
                columns: new[] { "FacilityId", "ActionNumber" },
                unique: true,
                filter: "[ActionNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EnforcementActions_FacilityId_ActionNumber",
                table: "EnforcementActions");
        }
    }
}
