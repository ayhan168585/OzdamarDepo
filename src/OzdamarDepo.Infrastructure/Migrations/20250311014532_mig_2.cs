using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzdamarDepo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArtistOrDirector",
                table: "MediaItems",
                newName: "ArtistOrActor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArtistOrActor",
                table: "MediaItems",
                newName: "ArtistOrDirector");
        }
    }
}
