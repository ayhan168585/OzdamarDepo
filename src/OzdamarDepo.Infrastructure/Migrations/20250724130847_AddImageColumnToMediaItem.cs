using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzdamarDepo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageColumnToMediaItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "MediaItems",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "MediaItems");
        }
    }
}
