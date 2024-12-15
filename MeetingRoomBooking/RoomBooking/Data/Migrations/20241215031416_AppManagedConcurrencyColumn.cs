using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class AppManagedConcurrencyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConcurrencyToken",
                table: "Room",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bb7ea193-5714-4c9e-a28d-e4c784a6f4eb", "AQAAAAIAAYagAAAAECQ9YrczNK1XuCG/42zXiQ+HLRmCNwqtWWTn4mtmXFxUV7mL9gFns5bL22BwwzNR/g==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Room");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a8e8bfdc-bbdb-438d-9c13-5d0b43e3eedd", "AQAAAAIAAYagAAAAEFRtqicaqy3JTCIM0OqCOS9mD1lmm5dthe4HZt/V24QkdyFO8BSzSU6hGGLp2C1Cng==" });
        }
    }
}
