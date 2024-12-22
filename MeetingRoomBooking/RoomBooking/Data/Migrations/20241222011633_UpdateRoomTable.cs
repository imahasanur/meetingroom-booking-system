using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumCapacity",
                table: "Room",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumCapacity",
                table: "Room",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ab396b57-eecb-4a51-a3dd-38d827eeef20", "AQAAAAIAAYagAAAAEEAchct1oPh6KQ3ezfWKq5quYDIXDVBVe6lhJhEHwiK/5r/wkiTnYf8KDKs9hDNaEw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumCapacity",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "MinimumCapacity",
                table: "Room");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e1fef2be-8ce7-456b-96c6-3e4f11ccee27", "AQAAAAIAAYagAAAAEDPTxpTL8kIp0q9gs1sufYAVAJUr+RI/VqQBiw6V9KNaHwikohdToV5c7Oa+SajVJQ==" });
        }
    }
}
