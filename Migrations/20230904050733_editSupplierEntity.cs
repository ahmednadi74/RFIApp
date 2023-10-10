using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RFIApp.Migrations
{
    public partial class editSupplierEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LeanID",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "LeanID",
                table: "Suppliers");
        }
    }
}
