using Microsoft.EntityFrameworkCore.Migrations;

namespace RentItAPI.Migrations
{
    public partial class CreatedByPropAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Users_CreatedById",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_CreatedById",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "LastNameId",
                table: "Reservations",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "FirstNameId",
                table: "Reservations",
                newName: "Info");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Reservations",
                newName: "LastNameId");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "Reservations",
                newName: "FirstNameId");

            migrationBuilder.AddColumn<int>(
                name: "EmailId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_CreatedById",
                table: "Businesses",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Users_CreatedById",
                table: "Businesses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
