using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoomImage",
                table: "Room",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4ec7e9d3-ba03-4ee0-8767-7f6d34fa986e", "AQAAAAIAAYagAAAAELhev7EWZE1bMOS+JYyZoatZ6jCQppy6JrRO76659AZuhBayeAagKlYmdg0j9uwfxA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "RoomImage",
                table: "Room");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "87d723e8-a559-4174-854e-f4852d6e340e", "AQAAAAIAAYagAAAAEBLpRZ/P5OC1o6olNG+saFU0gs1RxtlFsKtbMr0cssOrFFfwYd92tBrLNAWZ1s2N+w==" });
        }
    }
}
