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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalIncome = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalSum = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prognoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Reccuring = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    PrognosisId = table.Column<int>(type: "int", nullable: true)
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
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Food" },
                    { 2, "Transport" },
                    { 3, "Clothing" },
                    { 4, "Taxes" },
                    { 5, "House" },
                    { 6, "Hobbies" },
                    { 7, "Kids" },
                    { 8, "TV" },
                    { 9, "SaaS" },
                    { 10, "Subscriptions" },
                    { 11, "Salary" },
                    { 12, "Allowance" },
                    { 13, "ExtraIncome" },
                    { 14, "Undefined" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPosts_CategoryId",
                table: "BudgetPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPosts_PrognosisId",
                table: "BudgetPosts",
                column: "PrognosisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetPosts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Prognoses");
        }
    }
}
