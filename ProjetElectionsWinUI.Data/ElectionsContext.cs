using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using System.IO;

namespace ProjetElectionsWinUI.Data
{
    public class ElectionsContext : DbContext
    {
        // Tables de la base de données
        public DbSet<DistrictElectoral> Districts { get; set; }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Electeur> Electeurs { get; set; }

        /// <summary>
        /// Configuration de la base de données.
        /// Au départ, j'avais essayé la version du contexte donnée dans les notes de cours,
        /// mais j'avais souvent une erreur comme quoi la BD n'était pas trouvée.
        /// J'ai donc demandé à ChatGPT une autre approche : créer le dossier et le fichier
        /// manuellement avant de configurer SQLite. Cette version fonctionne de façon fiable.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Dossier fixe sur la machine (simple à retrouver pour les tests et la correction)
                string folder = @"C:\ProjetElectionsWinUI_DB";
                Directory.CreateDirectory(folder); // crée le dossier si nécessaire

                string dbPath = Path.Combine(folder, "elections.db");

                // Si le fichier n'existe pas encore, on le crée vide.
                // Ça évite l'erreur "base de données introuvable" au premier lancement.
                if (!File.Exists(dbPath))
                {
                    using (File.Create(dbPath))
                    {
                        // rien à écrire, on fait juste créer le fichier physique
                    }
                }

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        /// <summary>
        /// Configuration des modèles + appel au DataSeeder pour les données de départ.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Données initiales (districts, candidats, électeurs)
            // Le DataSeeder est séparé pour garder ce fichier plus léger.
            DataSeeder.Seed(modelBuilder);
        }
    }
}
