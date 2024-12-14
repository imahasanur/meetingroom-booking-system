using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConCurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "Room",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4b26b41c-9d1d-47b2-9a19-bdec15ce5d9d", "AQAAAAIAAYagAAAAEPCudr6l2TqPSmm+fFJeA01lbTwfIfZegbhAc2BlsPVlYeUW9lMTrXKEcoVgxDBEBQ==" });
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
