using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RFIApp.Migrations
{
    public partial class AddFileAttachmentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileAttachmentName",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileAttachmentName",
                table: "Suppliers");
        }
    }
}
