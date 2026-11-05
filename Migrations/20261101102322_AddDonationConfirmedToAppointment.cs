using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JeevanSetu.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationConfirmedToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DonationConfirmed",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationConfirmed",
                table: "Appointments");
        }
    }
}
