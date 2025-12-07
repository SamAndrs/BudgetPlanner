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
                name: "StoppedRecurringPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecurringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoppedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoppedRecurringPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurringPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    RecurringId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recurring = table.Column<int>(type: "int", nullable: false),
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
                columns: new[] { "Id", "Amount", "CategoryId", "Description", "PostType", "Recurring", "RecurringId", "RecurringStartDate" },
                values: new object[,]
                {
                    { 1, 2500.0, 8, "Barnbidrag", 0, 3, new Guid("389904d6-c94c-4bd8-8864-27644219f5ad"), new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 8500.0, 6, "Hyra", 1, 3, new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30"), new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1200.0, 3, "Busskort", 1, 3, new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 750.0, 3, "Resor till arbete", 1, 3, new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5"), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 900.0, 15, "Studiebidrag", 0, 3, new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab"), new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 3200.0, 2, "Veckohandling", 1, 2, new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1200.0, 11, "Netflix + Spotify", 1, 3, new Guid("19cf7680-db76-4047-bb8b-79217df278c1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 4500.0, 3, "Bilskatt", 1, 4, new Guid("67f4ec7c-13a9-4973-a884-80b009b9f3b0"), new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                name: "StoppedRecurringPosts");

            migrationBuilder.DropTable(
                name: "Prognoses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
