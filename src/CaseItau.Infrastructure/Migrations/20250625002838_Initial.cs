using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseItau.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    TypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Patrimony = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funds_FundTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FundTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FundTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "RENDA FIXA" },
                    { 2L, "ACOES" },
                    { 3L, "MULTI MERCADO" }
                });

            migrationBuilder.InsertData(
                table: "Funds",
                columns: new[] { "Id", "Code", "Name", "Patrimony", "TypeId", "Cnpj" },
                values: new object[,]
                {
                    { 1L, "ITAURF123", "ITAU JUROS RF +", 5498731.54m, 1L, "11222333444455" },
                    { 2L, "ITAUMM999", "ITAU TREND MM", 5m, 3L, "12222333444455" },
                    { 3L, "ITAURF321", "ITAU LONGO PRAZO RF +", 7875421.58m, 1L, "13222333444455" },
                    { 4L, "ITAUAC546", "ITAU ACOES DIVIDENDO", 66421254.83m, 2L, "14222333444455" },
                    { 5L, "ITAURF555", "ITAU JUROS RF +", 0m, 1L, "11222333444777" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funds_Code",
                table: "Funds",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funds_TypeId",
                table: "Funds",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funds");

            migrationBuilder.DropTable(
                name: "FundTypes");
        }
    }
}
