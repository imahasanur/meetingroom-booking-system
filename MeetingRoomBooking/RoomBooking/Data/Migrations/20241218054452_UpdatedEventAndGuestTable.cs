using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEventAndGuestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Event_RoomId",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "28cb79cd-d9ff-4d38-8545-8d7615f30acb", "AQAAAAIAAYagAAAAELS4YVAF0ssnWON3ly6Dv/ycSyCF3qSOpecFv/3AkzpDFHsFNizKBla+G60MT5o1Qw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Event_RoomId",
                table: "Event",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Event_RoomId",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "af505680-8dc4-4289-a108-204dc5c74ef4", "AQAAAAIAAYagAAAAEHHAFB3Gd+UsISmuzHBR8g5cEkyE4QyqM3u99jmiLgJrk2obpj+pes1bWZ2GaTl8DA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Event_RoomId",
                table: "Event",
                column: "RoomId",
                unique: true);
        }
    }
}
