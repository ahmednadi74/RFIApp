using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RFIApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifySupplierAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeesAreSubscribedToSocialSecurity",
                table: "ProductionInformation");

            migrationBuilder.DropColumn(
                name: "NoOfEmployees",
                table: "ProductionInformation");

            migrationBuilder.AddColumn<bool>(
                name: "EmployeesAreSubscribedToSocialSecurity",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NoOfEmployees",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeesAreSubscribedToSocialSecurity",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "NoOfEmployees",
                table: "Suppliers");

            migrationBuilder.AddColumn<bool>(
                name: "EmployeesAreSubscribedToSocialSecurity",
                table: "ProductionInformation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NoOfEmployees",
                table: "ProductionInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
