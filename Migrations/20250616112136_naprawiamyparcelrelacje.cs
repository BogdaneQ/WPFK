using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFK.Migrations
{
    /// <inheritdoc />
    public partial class naprawiamyparcelrelacje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Users_UserId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "Recipient",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Parcels");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Parcels",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "Parcels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "Parcels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_RecipientId",
                table: "Parcels",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_SenderId",
                table: "Parcels",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Users_RecipientId",
                table: "Parcels",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Users_SenderId",
                table: "Parcels",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Users_UserId",
                table: "Parcels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Users_RecipientId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Users_SenderId",
                table: "Parcels");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Users_UserId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_RecipientId",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_SenderId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Parcels");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Parcels",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                table: "Parcels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Parcels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Users_UserId",
                table: "Parcels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
