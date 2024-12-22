using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Time",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinimumTime = table.Column<int>(type: "int", nullable: false),
                    MaximumTime = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "803050e2-5c6d-44e1-a359-5b2a5d4ba61e", "AQAAAAIAAYagAAAAEDOI1GI3zjTpqVv+Qx4UQobNBtGfLoY2V8Ynx+n31oWypAb8N2SRfmPwnY19DgTzeg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Time");

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
                values: new object[] { "ab396b57-eecb-4a51-a3dd-38d827eeef20", "AQAAAAIAAYagAAAAEEAchct1oPh6KQ3ezfWKq5quYDIXDVBVe6lhJhEHwiK/5r/wkiTnYf8KDKs9hDNaEw==" });
        }
    }
}
