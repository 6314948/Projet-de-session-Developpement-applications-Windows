using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Linq;
using Xunit;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Tests unitaires pour CandidatDataProvider.
    /// </summary>
    public class CandidatDataProviderTests
    {
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void AddAndGetByDistrict_WorksCorrectly()
        {
            using var context = CreateContext();

            var districtProvider = new DistrictDataProvider(context);
            var candidatProvider = new CandidatDataProvider(context);

            var anyDistrict = districtProvider.GetAll().First();
            int districtId = anyDistrict.DistrictElectoralId;

            var newCandidat = new Candidat
            {
                Nom = "Candidat Test Unitaire",
                PartiPolitique = "Parti Test",
                VotesObtenus = 10,
                DistrictElectoralId = districtId
            };

            candidatProvider.Add(newCandidat);

            var candidats = candidatProvider.GetByDistrict(districtId);

            var created = candidats.FirstOrDefault(c => c.Nom == "Candidat Test Unitaire");
            Assert.NotNull(created);

            candidatProvider.Delete(created!.CandidatId);
        }
    }
}
