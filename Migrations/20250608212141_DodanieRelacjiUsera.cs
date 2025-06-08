using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFK.Migrations
{
    /// <inheritdoc />
    public partial class DodanieRelacjiUsera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Parcels_UserId",
                table: "Parcels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Users_UserId",
                table: "Parcels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Users_UserId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_UserId",
                table: "Parcels");
        }
    }
}
