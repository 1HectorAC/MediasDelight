using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediasDelight.Web.Migrations
{
    /// <inheritdoc />
    public partial class EditMediaItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dislikes",
                table: "MediaItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Likes",
                table: "MediaItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "MediaItems");
        }
    }
}
