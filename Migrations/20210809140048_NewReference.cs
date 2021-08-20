using Microsoft.EntityFrameworkCore.Migrations;

namespace RentItAPI.Migrations
{
    public partial class NewReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BusinessId",
                table: "Reservations",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Businesses_BusinessId",
                table: "Reservations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Businesses_BusinessId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BusinessId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Reservations");
        }
    }
}
