using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RFIApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TradingName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PostalAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WebAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactedMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfBusiness = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                columns: table => new
                {
                    BankDetailsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SwiftBIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentTerm = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.BankDetailsID);
                    table.ForeignKey(
                        name: "FK_BankDetails_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactTelNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactType = table.Column<int>(type: "int", nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactID);
                    table.ForeignKey(
                        name: "FK_Contacts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionInformation",
                columns: table => new
                {
                    ProductionInformationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductionCapacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExportCapacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfEmployees = table.Column<int>(type: "int", nullable: false),
                    EmployeesAreSubscribedToSocialSecurity = table.Column<bool>(type: "bit", nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionInformation", x => x.ProductionInformationID);
                    table.ForeignKey(
                        name: "FK_ProductionInformation_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_SupplierID",
                table: "BankDetails",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SupplierID",
                table: "Contacts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionInformation_SupplierID",
                table: "ProductionInformation",
                column: "SupplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDetails");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ProductionInformation");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
