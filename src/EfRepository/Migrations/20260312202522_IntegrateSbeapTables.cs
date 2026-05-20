using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class IntegrateSbeapTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPoints_CaseFiles_CaseFileId",
                table: "AuditPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFileComplianceEvents_CaseFiles_CaseFileId",
                table: "CaseFileComplianceEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFiles_AspNetUsers_ClosedById",
                table: "CaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFiles_AspNetUsers_DeletedById",
                table: "CaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFiles_AspNetUsers_ResponsibleStaffId",
                table: "CaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFiles_ViolationTypes_ViolationTypeCode",
                table: "CaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CaseFiles_CaseFileId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementActions_CaseFiles_CaseFileId",
                table: "EnforcementActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseFiles",
                table: "CaseFiles");

            migrationBuilder.RenameTable(
                name: "CaseFiles",
                newName: "EnforcementCaseFiles");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFiles_ViolationTypeCode",
                table: "EnforcementCaseFiles",
                newName: "IX_EnforcementCaseFiles_ViolationTypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFiles_ResponsibleStaffId",
                table: "EnforcementCaseFiles",
                newName: "IX_EnforcementCaseFiles_ResponsibleStaffId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFiles_FacilityId_ActionNumber",
                table: "EnforcementCaseFiles",
                newName: "IX_EnforcementCaseFiles_FacilityId_ActionNumber");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFiles_DeletedById",
                table: "EnforcementCaseFiles",
                newName: "IX_EnforcementCaseFiles_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFiles_ClosedById",
                table: "EnforcementCaseFiles",
                newName: "IX_EnforcementCaseFiles_ClosedById");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lookups",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnforcementCaseFiles",
                table: "EnforcementCaseFiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SbeapCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    SicCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    County = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Location_Street = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Location_Street2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Location_City = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Location_State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Location_PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MailingAddress_Street = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    MailingAddress_Street2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    MailingAddress_City = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    MailingAddress_State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MailingAddress_PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DeleteComments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SbeapCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SbeapCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: false),
                    CaseOpenedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CaseClosedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CaseClosureNotes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ReferralAgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferralDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReferralNotes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DeleteComments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SbeapCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SbeapCases_Lookups_ReferralAgencyId",
                        column: x => x.ReferralAgencyId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SbeapCases_SbeapCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "SbeapCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SbeapContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnteredById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EnteredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Honorific = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Address_Street2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SbeapContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SbeapContacts_AspNetUsers_EnteredById",
                        column: x => x.EnteredById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SbeapContacts_SbeapCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "SbeapCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SbeapActionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionItemTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: false),
                    EnteredById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EnteredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SbeapActionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SbeapActionItems_AspNetUsers_EnteredById",
                        column: x => x.EnteredById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SbeapActionItems_Lookups_ActionItemTypeId",
                        column: x => x.ActionItemTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SbeapActionItems_SbeapCases_CaseworkId",
                        column: x => x.CaseworkId,
                        principalTable: "SbeapCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => new { x.ContactId, x.Id });
                    table.ForeignKey(
                        name: "FK_PhoneNumber_SbeapContacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "SbeapContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SbeapActionItems_ActionItemTypeId",
                table: "SbeapActionItems",
                column: "ActionItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SbeapActionItems_CaseworkId",
                table: "SbeapActionItems",
                column: "CaseworkId");

            migrationBuilder.CreateIndex(
                name: "IX_SbeapActionItems_EnteredById",
                table: "SbeapActionItems",
                column: "EnteredById");

            migrationBuilder.CreateIndex(
                name: "IX_SbeapCases_CustomerId",
                table: "SbeapCases",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SbeapCases_ReferralAgencyId",
                table: "SbeapCases",
                column: "ReferralAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_SbeapContacts_CustomerId",
                table: "SbeapContacts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SbeapContacts_EnteredById",
                table: "SbeapContacts",
                column: "EnteredById");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPoints_EnforcementCaseFiles_CaseFileId",
                table: "AuditPoints",
                column: "CaseFileId",
                principalTable: "EnforcementCaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFileComplianceEvents_EnforcementCaseFiles_CaseFileId",
                table: "CaseFileComplianceEvents",
                column: "CaseFileId",
                principalTable: "EnforcementCaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_EnforcementCaseFiles_CaseFileId",
                table: "Comments",
                column: "CaseFileId",
                principalTable: "EnforcementCaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementActions_EnforcementCaseFiles_CaseFileId",
                table: "EnforcementActions",
                column: "CaseFileId",
                principalTable: "EnforcementCaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementCaseFiles_AspNetUsers_ClosedById",
                table: "EnforcementCaseFiles",
                column: "ClosedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementCaseFiles_AspNetUsers_DeletedById",
                table: "EnforcementCaseFiles",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementCaseFiles_AspNetUsers_ResponsibleStaffId",
                table: "EnforcementCaseFiles",
                column: "ResponsibleStaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementCaseFiles_ViolationTypes_ViolationTypeCode",
                table: "EnforcementCaseFiles",
                column: "ViolationTypeCode",
                principalTable: "ViolationTypes",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPoints_EnforcementCaseFiles_CaseFileId",
                table: "AuditPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFileComplianceEvents_EnforcementCaseFiles_CaseFileId",
                table: "CaseFileComplianceEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_EnforcementCaseFiles_CaseFileId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementActions_EnforcementCaseFiles_CaseFileId",
                table: "EnforcementActions");

            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementCaseFiles_AspNetUsers_ClosedById",
                table: "EnforcementCaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementCaseFiles_AspNetUsers_DeletedById",
                table: "EnforcementCaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementCaseFiles_AspNetUsers_ResponsibleStaffId",
                table: "EnforcementCaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EnforcementCaseFiles_ViolationTypes_ViolationTypeCode",
                table: "EnforcementCaseFiles");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "SbeapActionItems");

            migrationBuilder.DropTable(
                name: "SbeapContacts");

            migrationBuilder.DropTable(
                name: "SbeapCases");

            migrationBuilder.DropTable(
                name: "SbeapCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnforcementCaseFiles",
                table: "EnforcementCaseFiles");

            migrationBuilder.RenameTable(
                name: "EnforcementCaseFiles",
                newName: "CaseFiles");

            migrationBuilder.RenameIndex(
                name: "IX_EnforcementCaseFiles_ViolationTypeCode",
                table: "CaseFiles",
                newName: "IX_CaseFiles_ViolationTypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_EnforcementCaseFiles_ResponsibleStaffId",
                table: "CaseFiles",
                newName: "IX_CaseFiles_ResponsibleStaffId");

            migrationBuilder.RenameIndex(
                name: "IX_EnforcementCaseFiles_FacilityId_ActionNumber",
                table: "CaseFiles",
                newName: "IX_CaseFiles_FacilityId_ActionNumber");

            migrationBuilder.RenameIndex(
                name: "IX_EnforcementCaseFiles_DeletedById",
                table: "CaseFiles",
                newName: "IX_CaseFiles_DeletedById");

            migrationBuilder.RenameIndex(
                name: "IX_EnforcementCaseFiles_ClosedById",
                table: "CaseFiles",
                newName: "IX_CaseFiles_ClosedById");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lookups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseFiles",
                table: "CaseFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPoints_CaseFiles_CaseFileId",
                table: "AuditPoints",
                column: "CaseFileId",
                principalTable: "CaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFileComplianceEvents_CaseFiles_CaseFileId",
                table: "CaseFileComplianceEvents",
                column: "CaseFileId",
                principalTable: "CaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFiles_AspNetUsers_ClosedById",
                table: "CaseFiles",
                column: "ClosedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFiles_AspNetUsers_DeletedById",
                table: "CaseFiles",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFiles_AspNetUsers_ResponsibleStaffId",
                table: "CaseFiles",
                column: "ResponsibleStaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFiles_ViolationTypes_ViolationTypeCode",
                table: "CaseFiles",
                column: "ViolationTypeCode",
                principalTable: "ViolationTypes",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CaseFiles_CaseFileId",
                table: "Comments",
                column: "CaseFileId",
                principalTable: "CaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnforcementActions_CaseFiles_CaseFileId",
                table: "EnforcementActions",
                column: "CaseFileId",
                principalTable: "CaseFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
