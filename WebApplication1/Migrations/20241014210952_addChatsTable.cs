using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVER_SIDE.Migrations
{
    /// <inheritdoc />
    public partial class addChatsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chatEntity",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _memberBelong_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatEntity", x => x._id);
                    table.ForeignKey(
                        name: "FK_chatEntity_memberEntity__memberBelong_id",
                        column: x => x._memberBelong_id,
                        principalTable: "memberEntity",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chatEntity__memberBelong_id",
                table: "chatEntity",
                column: "_memberBelong_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatEntity");
        }
    }
}
