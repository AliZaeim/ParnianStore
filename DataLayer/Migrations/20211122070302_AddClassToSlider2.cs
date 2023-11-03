using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class AddClassToSlider2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SiiderCommentClass",
                table: "Sliders",
                newName: "SliderCommentClass");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SliderCommentClass",
                table: "Sliders",
                newName: "SiiderCommentClass");

            migrationBuilder.UpdateData(
                table: "InitialInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2021, 11, 22, 10, 27, 27, 848, DateTimeKind.Local).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "URId",
                keyValue: 1,
                column: "RegisterDate",
                value: new DateTime(2021, 11, 22, 10, 27, 27, 848, DateTimeKind.Local).AddTicks(3788));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisteredDate",
                value: new DateTime(2021, 11, 22, 10, 27, 27, 846, DateTimeKind.Local).AddTicks(7063));
        }
    }
}
