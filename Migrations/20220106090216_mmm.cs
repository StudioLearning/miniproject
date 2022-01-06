using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniproject.Migrations
{
    public partial class mmm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentfulID",
                table: "Order",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentState",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentfulID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentState",
                table: "Order");
        }
    }
}
