using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaxMinTimeFromEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumTime",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "MinimumTime",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b88390e9-d391-4ece-bc1e-786bf0533803", "AQAAAAIAAYagAAAAEHD4YjTSqu9zG8CEHLdPe0VamXSi7DTQJE8zDpv/17BSh7j+MZZQrda12UurQE+bbQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumTime",
                table: "Event",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumTime",
                table: "Event",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "addb5901-f42a-4468-8880-7954e17fac8e", "AQAAAAIAAYagAAAAELLxoPVkA6hpHtANudPynOXZWXg34GxW/E1VwIEu4c5r7FZORTVYidgMK8Fwd61FGQ==" });
        }
    }
}
