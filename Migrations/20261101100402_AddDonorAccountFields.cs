using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeevanSetu.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorAccountFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAccount",
                table: "Donors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Donors",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAccount",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Donors");
        }
    }
}
