using Microsoft.EntityFrameworkCore.Migrations;

namespace RentItAPI.Migrations
{
    public partial class ListOfReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ItemId",
                table: "Reservations",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Items_ItemId",
                table: "Reservations",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Items_ItemId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ItemId",
                table: "Reservations");
        }
    }
}
