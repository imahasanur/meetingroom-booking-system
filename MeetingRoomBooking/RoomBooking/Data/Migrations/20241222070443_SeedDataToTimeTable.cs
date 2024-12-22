using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataToTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c1c9df7-4560-4722-8dc2-53ede3a69147", "AQAAAAIAAYagAAAAEJxQqXLJEN4w6mckzrLmqU4a5fhwaJUuh1q5rIs/Bs754XyHkcCsHB5Zyj1Np6KZ9A==" });

            migrationBuilder.InsertData(
                table: "Time",
                columns: new[] { "Id", "CreatedAtUTC", "CreatedBy", "LastUpdatedAtUTC", "MaximumTime", "MinimumTime", "UpdatedBy" },
                values: new object[] { new Guid("6617251d-f526-40cd-b58f-7f2bfb50e8f6"), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", null, 120, 15, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Time",
                keyColumn: "Id",
                keyValue: new Guid("6617251d-f526-40cd-b58f-7f2bfb50e8f6"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "803050e2-5c6d-44e1-a359-5b2a5d4ba61e", "AQAAAAIAAYagAAAAEDOI1GI3zjTpqVv+Qx4UQobNBtGfLoY2V8Ynx+n31oWypAb8N2SRfmPwnY19DgTzeg==" });
        }
    }
}
