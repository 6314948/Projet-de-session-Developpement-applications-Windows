using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjetElectionsWinUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInitiale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictElectoralId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomDistrict = table.Column<string>(type: "TEXT", nullable: false),
                    Population = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.DistrictElectoralId);
                });

            migrationBuilder.CreateTable(
                name: "Candidats",
                columns: table => new
                {
                    CandidatId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    PartiPolitique = table.Column<string>(type: "TEXT", nullable: false),
                    VotesObtenus = table.Column<int>(type: "INTEGER", nullable: false),
                    DistrictElectoralId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidats", x => x.CandidatId);
                    table.ForeignKey(
                        name: "FK_Candidats_Districts_DistrictElectoralId",
                        column: x => x.DistrictElectoralId,
                        principalTable: "Districts",
                        principalColumn: "DistrictElectoralId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Electeurs",
                columns: table => new
                {
                    ElecteurId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Adresse = table.Column<string>(type: "TEXT", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DistrictElectoralId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electeurs", x => x.ElecteurId);
                    table.ForeignKey(
                        name: "FK_Electeurs_Districts_DistrictElectoralId",
                        column: x => x.DistrictElectoralId,
                        principalTable: "Districts",
                        principalColumn: "DistrictElectoralId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "DistrictElectoralId", "NomDistrict", "Population" },
                values: new object[,]
                {
                    { 1, "Hull", 72000 },
                    { 2, "Gatineau", 290000 },
                    { 3, "Aylmer", 45000 }
                });

            migrationBuilder.InsertData(
                table: "Candidats",
                columns: new[] { "CandidatId", "DistrictElectoralId", "Nom", "PartiPolitique", "VotesObtenus" },
                values: new object[,]
                {
                    { 1, 1, "Maude Marquis-Bissonnette", "Action Gatineau", 0 },
                    { 2, 2, "Mario Aubé", "Équipe Mario Aubé", 0 },
                    { 3, 3, "Rémi Bergeron", "Indépendant", 0 }
                });

            migrationBuilder.InsertData(
                table: "Electeurs",
                columns: new[] { "ElecteurId", "Adresse", "DateNaissance", "DistrictElectoralId", "Nom" },
                values: new object[,]
                {
                    { 1, "123 Rue Principale", new DateTime(1990, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Martin Dupont" },
                    { 2, "456 Avenue du Lac", new DateTime(1985, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Julie Tremblay" },
                    { 3, "789 Boulevard des Allumettières", new DateTime(2000, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Karim Bennani" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidats_DistrictElectoralId",
                table: "Candidats",
                column: "DistrictElectoralId");

            migrationBuilder.CreateIndex(
                name: "IX_Electeurs_DistrictElectoralId",
                table: "Electeurs",
                column: "DistrictElectoralId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidats");

            migrationBuilder.DropTable(
                name: "Electeurs");

            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
