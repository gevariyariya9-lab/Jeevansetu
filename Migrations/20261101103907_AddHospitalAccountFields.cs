using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeevanSetu.Migrations
{
    /// <inheritdoc />
    public partial class AddHospitalAccountFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAccount",
                table: "Hospitals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Hospitals",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Hospitals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAccount",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Hospitals");
        }
    }
}
