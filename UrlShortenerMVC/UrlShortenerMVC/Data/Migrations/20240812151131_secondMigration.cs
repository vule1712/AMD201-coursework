using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortenerMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class secondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UrlShortenerModels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UrlShortenerModels_UserId",
                table: "UrlShortenerModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlShortenerModels_AspNetUsers_UserId",
                table: "UrlShortenerModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlShortenerModels_AspNetUsers_UserId",
                table: "UrlShortenerModels");

            migrationBuilder.DropIndex(
                name: "IX_UrlShortenerModels_UserId",
                table: "UrlShortenerModels");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UrlShortenerModels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
