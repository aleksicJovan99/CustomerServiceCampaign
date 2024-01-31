using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomerServiceCampaign.Api.Migrations
{
    /// <inheritdoc />
    public partial class edidrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3faa546-f036-4176-8fe0-74ac3c93305b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf97f7e8-1632-4251-a82a-9f8a108f292a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "235dbce4-31d7-4c39-b0d3-25172c29e0a4", null, "agent", "AGENT" },
                    { "f97982cd-f912-487e-82d3-cad38aabd4cf", null, "sysadmin", "SYSADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "235dbce4-31d7-4c39-b0d3-25172c29e0a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f97982cd-f912-487e-82d3-cad38aabd4cf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b3faa546-f036-4176-8fe0-74ac3c93305b", null, "SysAdmin", "SYSADMIN" },
                    { "bf97f7e8-1632-4251-a82a-9f8a108f292a", null, "Agent", "AGENT" }
                });
        }
    }
}
