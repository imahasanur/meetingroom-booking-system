using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomTableWithColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2b7df31e-78e8-4c19-b705-7b31a3846e06", "AQAAAAIAAYagAAAAEF8JzLfP/dL7BxCnqInTi/injSjzlxComVGrSr1kHmZMOcenkRSuNBCZ/0Tq/RRGag==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Room");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c1c9df7-4560-4722-8dc2-53ede3a69147", "AQAAAAIAAYagAAAAEJxQqXLJEN4w6mckzrLmqU4a5fhwaJUuh1q5rIs/Bs754XyHkcCsHB5Zyj1Np6KZ9A==" });
        }
    }
}
