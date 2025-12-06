using Xunit;
using ProjetElectionsWinUI.Data;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Test unitaire simple pour vérifier que le contexte EF Core
    /// peut se connecter à la base SQLite.
    /// </summary>
    public class ElectionsContextTests
    {
        // Méthode utilitaire pour créer le contexte (constructeur sans options).
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void ElectionsContext_CanConnectToDatabase()
        {
            using var context = CreateContext();

            // CanConnect() retourne true si EF Core arrive à ouvrir la BD.
            // (À un moment j'avais une erreur bizarre de fichier introuvable,
            // j'ai donc demandé à ChatGPT une alternative pour créer le fichier
            // manuellement dans OnConfiguring, et ça a réglé le problème.)
            bool canConnect = context.Database.CanConnect();

            Assert.True(canConnect);
        }
    }
}
