using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class inn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "CarPosts");

            migrationBuilder.AddColumn<string>(
                name: "RentalStatus",
                table: "CarPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalStatus",
                table: "CarPosts");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "CarPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
