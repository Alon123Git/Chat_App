using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVER_SIDE.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "memberEntity",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _age = table.Column<int>(type: "int", nullable: false),
                    _isManager = table.Column<bool>(type: "bit", nullable: false),
                    _isLogin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memberEntity", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "messageEntity",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _sender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messageEntity", x => x._id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "memberEntity");

            migrationBuilder.DropTable(
                name: "messageEntity");
        }
    }
}
