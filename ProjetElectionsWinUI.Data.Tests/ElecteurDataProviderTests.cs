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
    /// On teste ici surtout l'ajout et la suppression d'un électeur.
    /// </summary>
    public class ElecteurDataProviderTests
    {
        // Comme dans les autres tests, le contexte est très simple à instancier.
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void AddAndDeleteElecteur_WorksCorrectly()
        {
            using var context = CreateContext();

            var districtProvider = new DistrictDataProvider(context);
            var electeurProvider = new ElecteurDataProvider(context);

            // On prend un district existant (ils viennent du DataSeeder).
            var anyDistrict = districtProvider.GetAll().First();
            int districtId = anyDistrict.DistrictElectoralId;

            // Création d'un électeur de test.
            var newElecteur = new Electeur
            {
                Nom = "Électeur Test Unitaire",
                Adresse = "123 Rue des Tests",
                DateNaissance = new DateTime(1995, 1, 1),
                DistrictElectoralId = districtId
            };

            // Ajout dans la BD
            electeurProvider.Add(newElecteur);

            // On vérifie qu'il existe maintenant dans la liste
            var electeurs = electeurProvider.GetAll();
            var created = electeurs.FirstOrDefault(e => e.Nom == "Électeur Test Unitaire");

            Assert.NotNull(created);

            // Suppression de l'électeur créé
            // (Même chose qu'avec DistrictDataProvideTests)
            electeurProvider.Delete(created!.ElecteurId);

            // La liste ne doit plus contenir cet électeur
            var electeursAfterDelete = electeurProvider.GetAll();
            Assert.DoesNotContain(electeursAfterDelete, e => e.ElecteurId == created.ElecteurId);
        }
    }
}
