using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetPlanner.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prognoses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyExpense = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalSum = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prognoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurringPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recurring = table.Column<int>(type: "int", nullable: false),
                    PostType = table.Column<int>(type: "int", nullable: false),
                    RecurringStartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringPosts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recurring = table.Column<int>(type: "int", nullable: false),
                    RecurringGroupID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostType = table.Column<int>(type: "int", nullable: false),
                    PrognosisId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPosts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetPosts_Prognoses_PrognosisId",
                        column: x => x.PrognosisId,
                        principalTable: "Prognoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Alla" },
                    { 2, "Mat" },
                    { 3, "Transport" },
                    { 4, "Kläder" },
                    { 5, "Skatt" },
                    { 6, "Hem" },
                    { 7, "Hobby" },
                    { 8, "Barn" },
                    { 9, "TV" },
                    { 10, "SaaS" },
                    { 11, "Prenumerationer" },
                    { 12, "Husdjur" },
                    { 13, "Underhållning" },
                    { 14, "Lön" },
                    { 15, "Bidrag" },
                    { 16, "Extrainkomst" },
                    { 17, "Okänd" }
                });

            migrationBuilder.InsertData(
                table: "RecurringPosts",
                columns: new[] { "Id", "Amount", "CategoryId", "Description", "PostType", "Recurring", "RecurringStartDate" },
                values: new object[,]
                {
                    { 1, 28500.0, 14, "Lön", 0, 3, new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 8500.0, 6, "Hyra", 1, 3, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1200.0, 3, "Busskort", 1, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 750.0, 3, "Resor till arbete", 1, 3, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 900.0, 15, "Studiebidrag", 0, 3, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 3200.0, 2, "Veckohandling", 1, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1200.0, 11, "Netflix + Spotify", 1, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 4500.0, 3, "Bilskatt", 1, 4, new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPosts_CategoryId",
                table: "BudgetPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPosts_PrognosisId",
                table: "BudgetPosts",
                column: "PrognosisId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringPosts_CategoryId",
                table: "RecurringPosts",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetPosts");

            migrationBuilder.DropTable(
                name: "RecurringPosts");

            migrationBuilder.DropTable(
                name: "Prognoses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
