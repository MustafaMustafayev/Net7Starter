using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("74999160-6c56-4a87-993a-81eab7163649"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8e53f86b-e20b-457b-964f-0b3f02ad9f31"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c67d6690-11b9-46da-85d0-9e9f5969b273"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f2409b40-f456-4ee4-ab27-48ed7829f607"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55e3cfce-a356-4399-a444-3abf3a0f266a"));

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "IsDeleted", "Key", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("1d790f4e-b4ec-48bd-bcc3-4d36b4b945bb"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Guest", null, null, "Qonaq" },
                    { new Guid("74a8b9b5-1a5d-4fd4-846b-a4847fea68ab"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Admin", null, null, "İnzibatçı" },
                    { new Guid("b4d788b6-ee5b-43b5-91bd-e6cb42d216be"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "SuperAdmin", null, null, "Baş inzibatçı" },
                    { new Guid("c973316c-080a-4e87-921e-f34ef8610523"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "User", null, null, "İstifadəçi" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactNumber", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "Email", "IsDeleted", "LastVerificationCode", "ModifiedAt", "ModifiedBy", "Password", "ProfileFileId", "RoleId", "Salt", "Username" },
                values: new object[] { new Guid("c1c11647-a8de-4e0c-8089-d1a8ba60860a"), "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "test@test.tst", false, null, null, null, "nx8pR24Lb01xMWW33FyMBviRIS8aTMWN0MnUSvGm/OVit1CTZukfsh/whYPccrdjoV2PiJIqhjqvuMHXVrR1gA==", null, null, "G1DfB9Y7+zLpG0L/i7IAQA==", "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1d790f4e-b4ec-48bd-bcc3-4d36b4b945bb"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("74a8b9b5-1a5d-4fd4-846b-a4847fea68ab"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b4d788b6-ee5b-43b5-91bd-e6cb42d216be"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c973316c-080a-4e87-921e-f34ef8610523"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1c11647-a8de-4e0c-8089-d1a8ba60860a"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "IsDeleted", "Key", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("74999160-6c56-4a87-993a-81eab7163649"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Admin", null, null, "İnzibatçı" },
                    { new Guid("8e53f86b-e20b-457b-964f-0b3f02ad9f31"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "SuperAdmin", null, null, "Baş inzibatçı" },
                    { new Guid("c67d6690-11b9-46da-85d0-9e9f5969b273"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Guest", null, null, "Qonaq" },
                    { new Guid("f2409b40-f456-4ee4-ab27-48ed7829f607"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "User", null, null, "İstifadəçi" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactNumber", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "Email", "IsDeleted", "LastVerificationCode", "ModifiedAt", "ModifiedBy", "Password", "ProfileFileId", "RoleId", "Salt", "Username" },
                values: new object[] { new Guid("55e3cfce-a356-4399-a444-3abf3a0f266a"), "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "test@test.tst", false, null, null, null, "61r1Cg+N6nkPd/MSQPtwkPB0YrhqpigVL+ITcX2LGoWvbMukBAqRniIlRiUje34NQ2FjWVJIZkkOpvwZj4IhTg==", null, null, "NQyUdl7XwIALLQrZKeJZjQ==", "Test" });
        }
    }
}
