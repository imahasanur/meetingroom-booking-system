using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomTableProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FontColor",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c16c0e8e-9bd4-494a-a0dc-06d9e72384c3", "AQAAAAIAAYagAAAAEIF6k2oZd3dKWwwqAt+ihnVG3XVVye4X8z07WYm7I7s+lknVPKRLtJbvXVb3k5glfQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontColor",
                table: "Room");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2b7df31e-78e8-4c19-b705-7b31a3846e06", "AQAAAAIAAYagAAAAEF8JzLfP/dL7BxCnqInTi/injSjzlxComVGrSr1kHmZMOcenkRSuNBCZ/0Tq/RRGag==" });
        }
    }
}
