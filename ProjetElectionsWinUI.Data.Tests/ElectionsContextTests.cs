using Xunit;
using ProjetElectionsWinUI.Data;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Test du contexte de base de données (ElectionsContext).
    /// Vérifie que la connexion à SQLite fonctionne correctement.
    /// </summary>
    public class ElectionsContextTests
    {
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void ElectionsContext_CanConnectToDatabase()
        {
            using var context = CreateContext();

            bool canConnect = context.Database.CanConnect();

            Assert.True(canConnect);
        }
    }
}
