using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class iininn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "CarPosts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "CarPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
