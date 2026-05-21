using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedReminder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MultiTenantSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Reminders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reminders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reminders",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Medications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medications",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Medications",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Medications");
        }
    }
}
