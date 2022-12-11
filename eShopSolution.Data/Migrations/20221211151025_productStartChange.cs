using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class productStartChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStart_AppUsers_AppUserId",
                table: "ProductStart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStart",
                table: "ProductStart");

            migrationBuilder.DropIndex(
                name: "IX_ProductStart_AppUserId",
                table: "ProductStart");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ProductStart");

            migrationBuilder.RenameTable(
                name: "ProductStart",
                newName: "ProductStarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStarts",
                table: "ProductStarts",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "1d9bb962-2e5d-4eeb-9b8f-292417b9c9a0");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1fe3f8ac-41b6-422c-b1a6-16881f6ff21c", "AQAAAAEAACcQAAAAEMvb2xzrcWRQ2w9kp5wfbHu5OTNNGs8d7vX7prkR6an/1fhwy0t6NJew+b0ICMEltQ==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Stock" },
                values: new object[] { new DateTime(2022, 12, 11, 22, 10, 24, 770, DateTimeKind.Local).AddTicks(3300), 4 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "Stock" },
                values: new object[] { new DateTime(2022, 12, 11, 22, 10, 24, 779, DateTimeKind.Local).AddTicks(8390), 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "Stock" },
                values: new object[] { new DateTime(2022, 12, 11, 22, 10, 24, 779, DateTimeKind.Local).AddTicks(8610), 7 });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStarts_UserId",
                table: "ProductStarts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStarts_AppUsers_UserId",
                table: "ProductStarts",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStarts_AppUsers_UserId",
                table: "ProductStarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStarts",
                table: "ProductStarts");

            migrationBuilder.DropIndex(
                name: "IX_ProductStarts_UserId",
                table: "ProductStarts");

            migrationBuilder.RenameTable(
                name: "ProductStarts",
                newName: "ProductStart");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "ProductStart",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStart",
                table: "ProductStart",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "79dfe2f1-fe5d-41e9-99f9-dd4e27c87fe5");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7e129315-95eb-42a2-ad2d-e6b19706e009", "AQAAAAEAACcQAAAAEKiUxcpN+BsSCpp5kfFSGDvfCuFQ4puXM5Vc1FkYZyEsztImbHz/g40ii0jjsSmtng==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Stock" },
                values: new object[] { new DateTime(2022, 12, 11, 22, 8, 2, 517, DateTimeKind.Local).AddTicks(3630), 4 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "Stock" },
                values: new object[] { new DateTime(2022, 12, 11, 22, 8, 2, 526, DateTimeKind.Local).AddTicks(5090), 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "Stock" },
                values: new object[] { new DateTime(2022, 12, 11, 22, 8, 2, 526, DateTimeKind.Local).AddTicks(5300), 7 });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStart_AppUserId",
                table: "ProductStart",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStart_AppUsers_AppUserId",
                table: "ProductStart",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
