using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using System;
using System.IO;

namespace ProjetElectionsWinUI.Data
{
    public class ElectionsContext : DbContext
    {
        public DbSet<DistrictElectoral> Districts { get; set; }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Electeur> Electeurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string folder = @"C:\ProjetElectionsWinUI_DB";
                Directory.CreateDirectory(folder);

                string dbPath = Path.Combine(folder, "elections.db");

                // Crée le fichier s'il n'existe pas
                if (!File.Exists(dbPath))
                {
                    using (File.Create(dbPath)) { }
                }

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🌱 Appel au DataSeeder externe
            DataSeeder.Seed(modelBuilder);
        }
    }
}
