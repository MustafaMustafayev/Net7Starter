using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class errorlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("714c41c2-04d2-4d33-9e6f-57596dd5642d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7e456062-e7d2-456b-b3d2-01f0084d3744"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e3464aaf-f55c-4387-80f6-1b3eb3cec131"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f9a73e14-5a8e-4241-844f-78b840368816"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8244adab-e443-41fd-ab59-a954268188cf"));

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    ErrorLogId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: false),
                    StackTrace = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.ErrorLogId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "IsDeleted", "Key", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("714c41c2-04d2-4d33-9e6f-57596dd5642d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Admin", null, null, "İnzibatçı" },
                    { new Guid("7e456062-e7d2-456b-b3d2-01f0084d3744"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "User", null, null, "İstifadəçi" },
                    { new Guid("e3464aaf-f55c-4387-80f6-1b3eb3cec131"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "SuperAdmin", null, null, "Baş inzibatçı" },
                    { new Guid("f9a73e14-5a8e-4241-844f-78b840368816"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, "Guest", null, null, "Qonaq" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactNumber", "CreatedAt", "CreatedById", "DeletedAt", "DeletedBy", "Email", "IsDeleted", "LastVerificationCode", "ModifiedAt", "ModifiedBy", "Password", "ProfileFileId", "RoleId", "Salt", "Username" },
                values: new object[] { new Guid("8244adab-e443-41fd-ab59-a954268188cf"), "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "test@test.tst", false, null, null, null, "QY19qeX6oS6t1TnXKfepHUsulKTEga3v+BlwgoSB1Rwkk4vjFpt+dLOkfpOBeHYTyAJtXYxGJizhAwEm0l/mRA==", null, null, "qq5Vph0zGzTiMEZtIjWZrQ==", "Test" });
        }
    }
}
