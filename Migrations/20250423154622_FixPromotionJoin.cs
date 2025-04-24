using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class FixPromotionJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[] { 1, "Іграшковий кіт", "Кіт", 300m });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Description", "DiscountPercentage", "EndDate", "StartDate", "Title" },
                values: new object[] { 1, null, 10, new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 4, 23, 0, 0, 0, 0, DateTimeKind.Local), "Весняна знижка" });

            migrationBuilder.InsertData(
                table: "ProductPromotions",
                columns: new[] { "ProductId", "PromotionId" },
                values: new object[] { 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductPromotions",
                keyColumns: new[] { "ProductId", "PromotionId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
