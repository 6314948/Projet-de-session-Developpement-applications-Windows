using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using System;

namespace ProjetElectionsWinUI.Data
{
    public class ElectionsContext : DbContext
    {
        // 1. Déclare les tables
        public DbSet<DistrictElectoral> Districts { get; set; }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Electeur> Electeurs { get; set; }

        // 2. Configure SQLite (chemin vers le fichier .db)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Crée un fichier elections.db dans le dossier de l'application WinUI
            optionsBuilder.UseSqlite("Data Source=elections.db");
        }

        // 3. Données initiales (Seed Data)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Districts
            modelBuilder.Entity<DistrictElectoral>().HasData(
                new DistrictElectoral { DistrictElectoralId = 1, NomDistrict = "Hull", Population = 72000 },
                new DistrictElectoral { DistrictElectoralId = 2, NomDistrict = "Gatineau", Population = 290000 },
                new DistrictElectoral { DistrictElectoralId = 3, NomDistrict = "Aylmer", Population = 45000 }
            );

            // Candidats
            modelBuilder.Entity<Candidat>().HasData(
                new Candidat { CandidatId = 1, Nom = "Maude Marquis-Bissonnette", PartiPolitique = "Action Gatineau", VotesObtenus = 0, DistrictElectoralId = 1 },
                new Candidat { CandidatId = 2, Nom = "Mario Aubé", PartiPolitique = "Équipe Mario Aubé", VotesObtenus = 0, DistrictElectoralId = 2 },
                new Candidat { CandidatId = 3, Nom = "Rémi Bergeron", PartiPolitique = "Indépendant", VotesObtenus = 0, DistrictElectoralId = 3 }
            );

            // Electeurs
            modelBuilder.Entity<Electeur>().HasData(
                new Electeur { ElecteurId = 1, Nom = "Martin Dupont", Adresse = "123 Rue Principale", DateNaissance = new DateTime(1990, 5, 12), DistrictElectoralId = 1 },
                new Electeur { ElecteurId = 2, Nom = "Julie Tremblay", Adresse = "456 Avenue du Lac", DateNaissance = new DateTime(1985, 8, 23), DistrictElectoralId = 2 },
                new Electeur { ElecteurId = 3, Nom = "Karim Bennani", Adresse = "789 Boulevard des Allumettières", DateNaissance = new DateTime(2000, 2, 3), DistrictElectoralId = 3 }
            );
        }
    }
}
