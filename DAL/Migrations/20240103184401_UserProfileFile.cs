using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_ProfileFileId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileFileId",
                table: "Users");

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

            migrationBuilder.DropColumn(
                name: "ProfileFileId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "IsDeleted", "Key", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("57670243-473b-4ad8-a37a-91eb7963e84b"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "SuperAdmin", null, null, "Baş inzibatçı" },
                    { new Guid("751a1aa0-58f0-4dbf-a1b3-c17aa96882c1"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "User", null, null, "İstifadəçi" },
                    { new Guid("a9fe89b5-83e9-408a-9cac-ab74d8c71359"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Admin", null, null, "İnzibatçı" },
                    { new Guid("db637744-3a23-4269-9560-ab2302b45f7b"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Guest", null, null, "Qonaq" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactNumber", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "Email", "File", "IsDeleted", "LastVerificationCode", "ModifiedAt", "ModifiedBy", "Password", "RoleId", "Salt", "Username" },
                values: new object[] { new Guid("58c8e76f-eb92-4a85-b254-c8b3c4a1fa11"), "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "test@test.tst", null, false, null, null, null, "ihd5HgzbN5ui17anY2/kLK09RjMuNXQh/HbDqrGj6zWPyszCW5ZFXYHegXKiV+QMlqpVRZHSaCF68EYCOGZQuw==", null, "VpWR9i0ZmOjFnxuckHA5OQ==", "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("57670243-473b-4ad8-a37a-91eb7963e84b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("751a1aa0-58f0-4dbf-a1b3-c17aa96882c1"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a9fe89b5-83e9-408a-9cac-ab74d8c71359"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("db637744-3a23-4269-9560-ab2302b45f7b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("58c8e76f-eb92-4a85-b254-c8b3c4a1fa11"));

            migrationBuilder.DropColumn(
                name: "File",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileFileId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileFileId",
                table: "Users",
                column: "ProfileFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_ProfileFileId",
                table: "Users",
                column: "ProfileFileId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
