using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomerServiceCampaign.Api.Migrations
{
    /// <inheritdoc />
    public partial class loyalty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "235dbce4-31d7-4c39-b0d3-25172c29e0a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f97982cd-f912-487e-82d3-cad38aabd4cf");

            migrationBuilder.CreateTable(
                name: "LoyaltyCustomers",
                columns: table => new
                {
                    AgentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyCustomers", x => new { x.AgentId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_LoyaltyCustomers_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoyaltyCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7461d364-c25a-4b32-82a5-c3aedbc9b780", null, "sysadmin", "SYSADMIN" },
                    { "d4dd7a01-5633-435c-9cba-e9a21f4616a7", null, "agent", "AGENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyCustomers_CustomerId",
                table: "LoyaltyCustomers",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7461d364-c25a-4b32-82a5-c3aedbc9b780");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4dd7a01-5633-435c-9cba-e9a21f4616a7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "235dbce4-31d7-4c39-b0d3-25172c29e0a4", null, "agent", "AGENT" },
                    { "f97982cd-f912-487e-82d3-cad38aabd4cf", null, "sysadmin", "SYSADMIN" }
                });
        }
    }
}
