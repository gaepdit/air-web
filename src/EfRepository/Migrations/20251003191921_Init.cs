using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Sender = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Recipients = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CopyRecipients = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TextBody = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: true),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViolationTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SeverityCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Deprecated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    AccountCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AccountUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProfileUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MostRecentLogin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Lookups_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ResponsibleStaffId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    ViolationTypeCode = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    CaseFileStatus = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: false),
                    DiscoveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DayZero = table.Column<DateOnly>(type: "date", nullable: true),
                    EnforcementDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PollutantIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirPrograms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionNumber = table.Column<short>(type: "smallint", nullable: true),
                    DataExchangeStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeleteComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseFiles_AspNetUsers_ClosedById",
                        column: x => x.ClosedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseFiles_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseFiles_AspNetUsers_ResponsibleStaffId",
                        column: x => x.ResponsibleStaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseFiles_ViolationTypes_ViolationTypeCode",
                        column: x => x.ViolationTypeCode,
                        principalTable: "ViolationTypes",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "ComplianceWork",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    WorkEntryType = table.Column<string>(type: "nvarchar(29)", maxLength: 29, nullable: false),
                    ResponsibleStaffId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AcknowledgmentLetterDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsComplianceEvent = table.Column<bool>(type: "bit", nullable: false),
                    DataExchangeStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ReceivedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AccReportingYear = table.Column<int>(type: "int", nullable: true),
                    PostmarkDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PostmarkedOnTime = table.Column<bool>(type: "bit", nullable: true),
                    SignedByRo = table.Column<bool>(type: "bit", nullable: true),
                    OnCorrectForms = table.Column<bool>(type: "bit", nullable: true),
                    IncludesAllTvConditions = table.Column<bool>(type: "bit", nullable: true),
                    CorrectlyCompleted = table.Column<bool>(type: "bit", nullable: true),
                    ReportsDeviations = table.Column<bool>(type: "bit", nullable: true),
                    IncludesPreviouslyUnreportedDeviations = table.Column<bool>(type: "bit", nullable: true),
                    ReportsAllKnownDeviations = table.Column<bool>(type: "bit", nullable: true),
                    ResubmittalRequired = table.Column<bool>(type: "bit", nullable: true),
                    EnforcementNeeded = table.Column<bool>(type: "bit", nullable: true),
                    InspectionReason = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    InspectionStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectionEnded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeatherConditions = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    InspectionGuide = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FacilityOperating = table.Column<bool>(type: "bit", nullable: true),
                    DeviationsNoted = table.Column<bool>(type: "bit", nullable: true),
                    FollowupTaken = table.Column<bool>(type: "bit", nullable: true),
                    ReportingPeriodType = table.Column<string>(type: "nvarchar(29)", maxLength: 29, nullable: true),
                    ReportingPeriodStart = table.Column<DateOnly>(type: "date", nullable: true),
                    ReportingPeriodEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    ReportingPeriodComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReportComplete = table.Column<bool>(type: "bit", nullable: true),
                    ReferenceNumber = table.Column<int>(type: "int", nullable: true),
                    ReceivedByComplianceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NotificationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PermitRevocationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PhysicalShutdownDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeleteComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceWork", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplianceWork_AspNetUsers_ClosedById",
                        column: x => x.ClosedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComplianceWork_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComplianceWork_AspNetUsers_ResponsibleStaffId",
                        column: x => x.ResponsibleStaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComplianceWork_Lookups_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ReviewedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OnsiteInspection = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    DataExchangeStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeleteComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fces_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fces_AspNetUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EnforcementActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseFileId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewRequestedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionNumber = table.Column<short>(type: "smallint", nullable: true),
                    DataExchangeStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ExecutedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AppealedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResolvedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReceivedFromFacility = table.Column<DateOnly>(type: "date", nullable: true),
                    ReceivedFromDirectorsOffice = table.Column<DateOnly>(type: "date", nullable: true),
                    OrderId = table.Column<short>(type: "smallint", nullable: true),
                    OrderNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    PenaltyComment = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    StipulatedPenaltiesDefined = table.Column<bool>(type: "bit", nullable: true),
                    ResponseRequested = table.Column<bool>(type: "bit", nullable: true),
                    ResponseReceived = table.Column<DateOnly>(type: "date", nullable: true),
                    ResponseComment = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    LetterOfNoncompliance_ResolvedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NoticeOfViolation_ResponseRequested = table.Column<bool>(type: "bit", nullable: true),
                    ProposedConsentOrder_ResponseRequested = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeleteComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnforcementActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnforcementActions_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnforcementActions_AspNetUsers_CurrentReviewerId",
                        column: x => x.CurrentReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnforcementActions_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnforcementActions_CaseFiles_CaseFileId",
                        column: x => x.CaseFileId,
                        principalTable: "CaseFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseFileComplianceEvents",
                columns: table => new
                {
                    CaseFilesId = table.Column<int>(type: "int", nullable: false),
                    ComplianceEventsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFileComplianceEvents", x => new { x.CaseFilesId, x.ComplianceEventsId });
                    table.ForeignKey(
                        name: "FK_CaseFileComplianceEvents_CaseFiles_CaseFilesId",
                        column: x => x.CaseFilesId,
                        principalTable: "CaseFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseFileComplianceEvents_ComplianceWork_ComplianceEventsId",
                        column: x => x.ComplianceEventsId,
                        principalTable: "ComplianceWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    What = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WhoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    When = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MoreInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    CaseFileId = table.Column<int>(type: "int", nullable: true),
                    FceId = table.Column<int>(type: "int", nullable: true),
                    WorkEntryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPoints_AspNetUsers_WhoId",
                        column: x => x.WhoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditPoints_CaseFiles_CaseFileId",
                        column: x => x.CaseFileId,
                        principalTable: "CaseFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPoints_ComplianceWork_WorkEntryId",
                        column: x => x.WorkEntryId,
                        principalTable: "ComplianceWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPoints_Fces_FceId",
                        column: x => x.FceId,
                        principalTable: "Fces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: false),
                    CommentById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommentedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FceId = table.Column<int>(type: "int", nullable: true),
                    WorkEntryId = table.Column<int>(type: "int", nullable: true),
                    CaseFileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_CommentById",
                        column: x => x.CommentById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_CaseFiles_CaseFileId",
                        column: x => x.CaseFileId,
                        principalTable: "CaseFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_ComplianceWork_WorkEntryId",
                        column: x => x.WorkEntryId,
                        principalTable: "ComplianceWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Fces_FceId",
                        column: x => x.FceId,
                        principalTable: "Fces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnforcementActionReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnforcementActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestedOfId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    ReviewComments = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnforcementActionReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnforcementActionReviews_AspNetUsers_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnforcementActionReviews_AspNetUsers_RequestedOfId",
                        column: x => x.RequestedOfId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnforcementActionReviews_AspNetUsers_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnforcementActionReviews_EnforcementActions_EnforcementActionId",
                        column: x => x.EnforcementActionId,
                        principalTable: "EnforcementActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StipulatedPenalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsentOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ReceivedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 7000, nullable: true),
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
                    table.PrimaryKey("PK_StipulatedPenalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StipulatedPenalties_EnforcementActions_ConsentOrderId",
                        column: x => x.ConsentOrderId,
                        principalTable: "EnforcementActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_CaseFileId",
                table: "AuditPoints",
                column: "CaseFileId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_FceId",
                table: "AuditPoints",
                column: "FceId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_WhoId",
                table: "AuditPoints",
                column: "WhoId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPoints_WorkEntryId",
                table: "AuditPoints",
                column: "WorkEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFileComplianceEvents_ComplianceEventsId",
                table: "CaseFileComplianceEvents",
                column: "ComplianceEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFiles_ClosedById",
                table: "CaseFiles",
                column: "ClosedById");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFiles_DeletedById",
                table: "CaseFiles",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFiles_ResponsibleStaffId",
                table: "CaseFiles",
                column: "ResponsibleStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFiles_ViolationTypeCode",
                table: "CaseFiles",
                column: "ViolationTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CaseFileId",
                table: "Comments",
                column: "CaseFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentById",
                table: "Comments",
                column: "CommentById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FceId",
                table: "Comments",
                column: "FceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WorkEntryId",
                table: "Comments",
                column: "WorkEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_ClosedById",
                table: "ComplianceWork",
                column: "ClosedById");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_DeletedById",
                table: "ComplianceWork",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_NotificationTypeId",
                table: "ComplianceWork",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceWork_ResponsibleStaffId",
                table: "ComplianceWork",
                column: "ResponsibleStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActionReviews_EnforcementActionId",
                table: "EnforcementActionReviews",
                column: "EnforcementActionId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActionReviews_RequestedById",
                table: "EnforcementActionReviews",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActionReviews_RequestedOfId",
                table: "EnforcementActionReviews",
                column: "RequestedOfId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActionReviews_ReviewedById",
                table: "EnforcementActionReviews",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActions_ApprovedById",
                table: "EnforcementActions",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActions_CaseFileId",
                table: "EnforcementActions",
                column: "CaseFileId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActions_CurrentReviewerId",
                table: "EnforcementActions",
                column: "CurrentReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementActions_DeletedById",
                table: "EnforcementActions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Fces_DeletedById",
                table: "Fces",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Fces_ReviewedById",
                table: "Fces",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_StipulatedPenalties_ConsentOrderId",
                table: "StipulatedPenalties",
                column: "ConsentOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditPoints");

            migrationBuilder.DropTable(
                name: "CaseFileComplianceEvents");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "EnforcementActionReviews");

            migrationBuilder.DropTable(
                name: "StipulatedPenalties");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ComplianceWork");

            migrationBuilder.DropTable(
                name: "Fces");

            migrationBuilder.DropTable(
                name: "EnforcementActions");

            migrationBuilder.DropTable(
                name: "CaseFiles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ViolationTypes");

            migrationBuilder.DropTable(
                name: "Lookups");
        }
    }
}
