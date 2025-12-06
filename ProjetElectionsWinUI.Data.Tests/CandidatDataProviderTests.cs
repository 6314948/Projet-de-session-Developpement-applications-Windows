using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Linq;
using Xunit;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Tests unitaires pour le CandidatDataProvider.
    /// Ce fichier vérifie surtout que l'ajout et la récupération des candidats fonctionnent.
    /// </summary>
    public class CandidatDataProviderTests
    {
        // Petite méthode utilitaire pour créer un contexte.
        // Le constructeur du contexte ne prend pas d'options,
        // donc c'est assez simple pour les tests.
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void AddAndGetByDistrict_WorksCorrectly()
        {
            using var context = CreateContext();

            var districtProvider = new DistrictDataProvider(context);
            var candidatProvider = new CandidatDataProvider(context);

            // On prend un district existant (ils sont seedés au démarrage).
            var anyDistrict = districtProvider.GetAll().First();
            int districtId = anyDistrict.DistrictElectoralId;

            // Création d'un candidat de test.
            var newCandidat = new Candidat
            {
                Nom = "Candidat Test Unitaire",
                PartiPolitique = "Parti Test",
                VotesObtenus = 10,
                DistrictElectoralId = districtId
            };

            // Ajout du candidat dans la BD
            candidatProvider.Add(newCandidat);

            // On vérifie maintenant qu'on peut bien le récupérer via le provider.
            var candidats = candidatProvider.GetByDistrict(districtId);

            var created = candidats.FirstOrDefault(c => c.Nom == "Candidat Test Unitaire");

            // Le candidat doit exister dans la liste.
            Assert.NotNull(created);

            // Nettoyage : on supprime le candidat pour éviter de polluer la BD.
            // (J'avais demandé à ChatGPT comment faire un "clean-up" propre dans un test.)
            candidatProvider.Delete(created!.CandidatId);
        }
    }
}
