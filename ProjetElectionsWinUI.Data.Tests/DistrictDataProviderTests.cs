using Xunit;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Linq;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Tests unitaires pour DistrictDataProvider.
    /// On teste surtout que les opérations de base (CRUD) fonctionnent comme prévu.
    /// </summary>
    public class DistrictDataProviderTests
    {
        // Comme dans les autres tests : un petit helper pour créer le contexte.
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void GetAll_ReturnsAtLeastSeededDistricts()
        {
            using var context = CreateContext();
            var provider = new DistrictDataProvider(context);

            var districts = provider.GetAll();

            // Normalement il y a 3 districts seedés dans DataSeeder.
            Assert.NotNull(districts);
            Assert.True(districts.Count >= 1);
        }

        [Fact]
        public void AddAndDeleteDistrict_WorksCorrectly()
        {
            using var context = CreateContext();
            var provider = new DistrictDataProvider(context);

            int before = provider.GetAll().Count;

            // Création d’un district de test
            var newDistrict = new DistrictElectoral
            {
                NomDistrict = "TestDistrict_Unitaire",
                Population = 12345
            };

            provider.Add(newDistrict);

            // Vérifier que l'ajout a fonctionné
            var afterAdd = provider.GetAll();
            Assert.Equal(before + 1, afterAdd.Count);

            var created = afterAdd.FirstOrDefault(d => d.NomDistrict == "TestDistrict_Unitaire");
            Assert.NotNull(created);

            // Suppression du district créé
            // (À la base je savais pas si c'était une bonne idée d'effacer
            // pendant un test, j’ai vérifié avec ChatGPT et c’est recommandé
            // pour garder la base propre entre les tests.)
            provider.Delete(created!.DistrictElectoralId);

            var afterDelete = provider.GetAll();
            Assert.Equal(before, afterDelete.Count);
        }
    }
}
