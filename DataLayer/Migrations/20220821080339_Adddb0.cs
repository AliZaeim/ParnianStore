using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class Adddb0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InitialInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 21, 12, 33, 38, 914, DateTimeKind.Local).AddTicks(5048));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "URId",
                keyValue: 1,
                column: "RegisterDate",
                value: new DateTime(2022, 8, 21, 12, 33, 38, 914, DateTimeKind.Local).AddTicks(2258));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisteredDate",
                value: new DateTime(2022, 8, 21, 12, 33, 38, 912, DateTimeKind.Local).AddTicks(4905));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InitialInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2021, 11, 22, 10, 33, 1, 495, DateTimeKind.Local).AddTicks(5684));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "URId",
                keyValue: 1,
                column: "RegisterDate",
                value: new DateTime(2021, 11, 22, 10, 33, 1, 495, DateTimeKind.Local).AddTicks(2890));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisteredDate",
                value: new DateTime(2021, 11, 22, 10, 33, 1, 493, DateTimeKind.Local).AddTicks(6094));
        }
    }
}
