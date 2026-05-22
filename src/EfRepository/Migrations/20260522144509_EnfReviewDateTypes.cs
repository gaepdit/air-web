using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWeb.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class EnfReviewDateTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReviewRequestedDate",
                table: "EnforcementActions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "RequestedDate",
                table: "EnforcementActionReviews",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewRequestedDate",
                table: "EnforcementActions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestedDate",
                table: "EnforcementActionReviews",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
