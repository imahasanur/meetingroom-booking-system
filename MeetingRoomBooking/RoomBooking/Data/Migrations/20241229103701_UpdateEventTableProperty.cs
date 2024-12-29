using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventTableProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FontColor",
                table: "Event",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fe3ab9c2-671d-41cc-9ca6-fb11b0b32a5f", "AQAAAAIAAYagAAAAEMi4HlJUr5KiEByt/tem7eHh0WFHFtJYMk8voMpzcHEzCUiPb/km7nvL+UT5Zzx7zg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontColor",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c16c0e8e-9bd4-494a-a0dc-06d9e72384c3", "AQAAAAIAAYagAAAAEIF6k2oZd3dKWwwqAt+ihnVG3XVVye4X8z07WYm7I7s+lknVPKRLtJbvXVb3k5glfQ==" });
        }
    }
}
