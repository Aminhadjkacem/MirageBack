using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DHP_Manager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Ville = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MatriculeFiscal = table.Column<string>(type: "text", nullable: false),
                    Contact1Name = table.Column<string>(type: "text", nullable: false),
                    Contact1Phone = table.Column<string>(type: "text", nullable: false),
                    Contact2Name = table.Column<string>(type: "text", nullable: false),
                    Contact2Phone = table.Column<string>(type: "text", nullable: false),
                    Contact3Name = table.Column<string>(type: "text", nullable: false),
                    Contact3Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NormalCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CIN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalCustomers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCustomers_MatriculeFiscal",
                table: "CompanyCustomers",
                column: "MatriculeFiscal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NormalCustomers_CIN",
                table: "NormalCustomers",
                column: "CIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyCustomers");

            migrationBuilder.DropTable(
                name: "NormalCustomers");
        }
    }
}
