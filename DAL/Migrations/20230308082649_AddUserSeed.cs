using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "ContactNumber", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "Email", "ImagePath", "IsDeleted", "LastVerificationCode", "ModifiedAt", "ModifiedBy", "Password", "RoleId", "Salt", "Username" },
                values: new object[] { 1, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "test@test.tst", null, false, null, null, null, "TL4DistyAz8iQdWimvq5fabVnrPoXK7OTPKOhJwbrIHs2YwwrWajK7+4usEyPnC7EXyPL1VW7Y9Zg8MaSbIorA==", null, "IL5ihKz0/PvSHHvvfyBtHA==", "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
