using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class AddClassToSlider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiiderCommentClass",
                table: "Sliders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SliderLinkClass",
                table: "Sliders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SliderTitleClass",
                table: "Sliders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiiderCommentClass",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "SliderLinkClass",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "SliderTitleClass",
                table: "Sliders");

            migrationBuilder.UpdateData(
                table: "InitialInfos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2021, 10, 18, 14, 52, 7, 605, DateTimeKind.Local).AddTicks(8253));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "URId",
                keyValue: 1,
                column: "RegisterDate",
                value: new DateTime(2021, 10, 18, 14, 52, 7, 605, DateTimeKind.Local).AddTicks(5458));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegisteredDate",
                value: new DateTime(2021, 10, 18, 14, 52, 7, 603, DateTimeKind.Local).AddTicks(7073));
        }
    }
}
