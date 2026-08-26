using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShortCodeToLinkClickStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkId",
                table: "LinkClickStats");

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "LinkClickStats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "LinkClickStats");

            migrationBuilder.AddColumn<long>(
                name: "LinkId",
                table: "LinkClickStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
