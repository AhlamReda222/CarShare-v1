using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicensePath",
                table: "RentalRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProposalDocumentPath",
                table: "RentalRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensePath",
                table: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "ProposalDocumentPath",
                table: "RentalRequests");
        }
    }
}
