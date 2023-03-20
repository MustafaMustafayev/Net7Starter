using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixUserSeedRoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RoleId", "Salt" },
                values: new object[] { "ubrqza+H+TTd509jCr10sm6okt9Q6jN008/EmIRhJrkvE1RLOsdO1a+YabnyX5/o98w3yl/q82a9KzTWxtkN0g==", 1, "4o/c4bemK0sq9GUXhKKrbw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RoleId", "Salt" },
                values: new object[] { "TL4DistyAz8iQdWimvq5fabVnrPoXK7OTPKOhJwbrIHs2YwwrWajK7+4usEyPnC7EXyPL1VW7Y9Zg8MaSbIorA==", null, "IL5ihKz0/PvSHHvvfyBtHA==" });
        }
    }
}
