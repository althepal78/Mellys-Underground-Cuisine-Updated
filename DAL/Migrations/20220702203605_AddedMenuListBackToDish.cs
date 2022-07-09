using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddedMenuListBackToDish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Menu_MenuID",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_MenuID",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "MenuID",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "DishMenu",
                columns: table => new
                {
                    DailyMenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuDishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishMenu", x => new { x.DailyMenuID, x.MenuDishId });
                    table.ForeignKey(
                        name: "FK_DishMenu_Dishes_MenuDishId",
                        column: x => x.MenuDishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishMenu_Menu_DailyMenuID",
                        column: x => x.DailyMenuID,
                        principalTable: "Menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishMenu_MenuDishId",
                table: "DishMenu",
                column: "MenuDishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishMenu");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuID",
                table: "Dishes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuID",
                table: "Dishes",
                column: "MenuID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Menu_MenuID",
                table: "Dishes",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "ID");
        }
    }
}
