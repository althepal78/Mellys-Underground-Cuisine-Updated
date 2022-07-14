using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class maybeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishMenu");

            migrationBuilder.CreateTable(
                name: "MenuDishes",
                columns: table => new
                {
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuDishes", x => new { x.DishId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_MenuDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_MenuId",
                table: "MenuDishes",
                column: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuDishes");

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
    }
}
