using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFieldNameofAspNetUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "AspNetUsers",
                newName: "CreatedAtUTC");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "AspNetUsers",
                newName: "LastUpdatedAtUTC");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "87d723e8-a559-4174-854e-f4852d6e340e", "AQAAAAIAAYagAAAAEBLpRZ/P5OC1o6olNG+saFU0gs1RxtlFsKtbMr0cssOrFFfwYd92tBrLNAWZ1s2N+w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAtUTC",
                table: "AspNetUsers",
                newName: "CreatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAtUTC",
                table: "AspNetUsers",
                newName: "UpdatedAtUtc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b88390e9-d391-4ece-bc1e-786bf0533803", "AQAAAAIAAYagAAAAEHD4YjTSqu9zG8CEHLdPe0VamXSi7DTQJE8zDpv/17BSh7j+MZZQrda12UurQE+bbQ==" });
        }
    }
}
