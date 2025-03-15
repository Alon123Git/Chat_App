using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVER_SIDE.Migrations
{
    /// <inheritdoc />
    public partial class roleProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_role",
                table: "memberEntity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_role",
                table: "memberEntity");
        }
    }
}
