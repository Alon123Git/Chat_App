using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVER_SIDE.Migrations
{
    /// <inheritdoc />
    public partial class passwordField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_password",
                table: "memberEntity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_password",
                table: "memberEntity");
        }
    }
}
