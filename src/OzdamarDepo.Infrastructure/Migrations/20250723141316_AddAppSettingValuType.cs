using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzdamarDepo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppSettingValuType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValueType",
                table: "AppSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "AppSettings");
        }
    }
}
