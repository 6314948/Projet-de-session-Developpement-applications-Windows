using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System;
using System.Linq;
using Xunit;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Tests unitaires pour ElecteurDataProvider.
    /// </summary>
    public class ElecteurDataProviderTests
    {
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void AddAndDeleteElecteur_WorksCorrectly()
        {
            using var context = CreateContext();

            var districtProvider = new DistrictDataProvider(context);
            var electeurProvider = new ElecteurDataProvider(context);

            var anyDistrict = districtProvider.GetAll().First();
            int districtId = anyDistrict.DistrictElectoralId;

            var newElecteur = new Electeur
            {
                Nom = "Électeur Test Unitaire",
                Adresse = "123 Rue des Tests",
                DateNaissance = new DateTime(1995, 1, 1),
                DistrictElectoralId = districtId
            };

            electeurProvider.Add(newElecteur);

            var electeurs = electeurProvider.GetAll();

            var created = electeurs.FirstOrDefault(e => e.Nom == "Électeur Test Unitaire");
            Assert.NotNull(created);

            electeurProvider.Delete(created!.ElecteurId);

            var electeursAfterDelete = electeurProvider.GetAll();
            Assert.DoesNotContain(electeursAfterDelete, e => e.ElecteurId == created.ElecteurId);
        }
    }
}
