using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class removefileid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Files");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "Salt" },
                values: new object[] { "iLpYvgwCLVaFyxy4SE2H/yezr7RWB2q0nTvGM6hEB58e3hl5r9bTc69DlOivZpDyt674bfwH0Fs/1xzZEqiGeQ==", "B68b2qf691UQl2ZOGxjZsA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "Salt" },
                values: new object[] { "B7o9mQ8ElDbXd4EEmE4eLqAJ/++qaLCvs6iM0JrvU97RIFn1HmCnD8q7yiQ+/8wsrTDGvg9dYb6zJOJzUaWVuQ==", "W7vy4Gq8krbOZsFQ0AQ5Xg==" });
        }
    }
}
