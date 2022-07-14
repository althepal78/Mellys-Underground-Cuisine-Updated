using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class fixingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Servings",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "date",
                table: "Dishes");

            migrationBuilder.AddColumn<int>(
                name: "Servings",
                table: "MenuDishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Servings",
                table: "MenuDishes");

            migrationBuilder.AddColumn<int>(
                name: "Servings",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "Dishes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
