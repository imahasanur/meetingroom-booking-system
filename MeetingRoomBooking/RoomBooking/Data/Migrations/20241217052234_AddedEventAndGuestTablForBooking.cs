using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEventAndGuestTablForBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_Event_Room_RoomId1",
                    //    column: x => x.RoomId1,
                    //    principalTable: "Room",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAtUTC = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guest_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eb80b453-2d75-4825-8005-d0cd35b8b253", "AQAAAAIAAYagAAAAEE0+yAUPVb9jH0GtHM5IpiGtj6Wd7ENAVlw4QLsWYnbb2BjTFGWTXsm91rfKhWVJYg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Event_RoomId",
                table: "Event",
                column: "RoomId",
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Event_RoomId1",
            //    table: "Event",
            //    column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_EventId",
                table: "Guest",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c23fda7-ae43-439e-b7f9-7d30868cb399"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bb7ea193-5714-4c9e-a28d-e4c784a6f4eb", "AQAAAAIAAYagAAAAECQ9YrczNK1XuCG/42zXiQ+HLRmCNwqtWWTn4mtmXFxUV7mL9gFns5bL22BwwzNR/g==" });
        }
    }
}
