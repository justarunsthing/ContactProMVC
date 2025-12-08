using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactProMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDemoUser",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDemoUser",
                table: "AspNetUsers");
        }
    }
}
