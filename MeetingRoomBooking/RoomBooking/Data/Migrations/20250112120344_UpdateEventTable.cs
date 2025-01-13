using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Event",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "addb5901-f42a-4468-8880-7954e17fac8e", "AQAAAAIAAYagAAAAELLxoPVkA6hpHtANudPynOXZWXg34GxW/E1VwIEu4c5r7FZORTVYidgMK8Fwd61FGQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Event");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a4851221-87ca-4f87-b9ef-e18b97968610", "AQAAAAIAAYagAAAAEOJD3iQXRYzMuCaEoQ3CDUgFIyHuMS7b7Y15ptSWX+9fL4WhRB0pUrIPaSDvJRJaQg==" });
        }
    }
}
