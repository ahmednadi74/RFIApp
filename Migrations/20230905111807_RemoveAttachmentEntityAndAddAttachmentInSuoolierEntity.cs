using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RFIApp.Migrations
{
    public partial class RemoveAttachmentEntityAndAddAttachmentInSuoolierEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.AddColumn<string>(
                name: "FileAttachmentPath",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileAttachmentPath",
                table: "Suppliers");

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    AttachmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileAttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.AttachmentID);
                    table.ForeignKey(
                        name: "FK_Attachment_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_SupplierID",
                table: "Attachment",
                column: "SupplierID",
                unique: true);
        }
    }
}
