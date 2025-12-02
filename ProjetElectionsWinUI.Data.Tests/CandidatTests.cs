using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using Xunit;

namespace ProjetElectionsWinUI.Data.Tests
{
    /// <summary>
    /// Vérifie que l'objet Candidat stocke correctement les propriétés assignées.
    /// </summary>
    public class CandidatTests
    {
        [Fact]
        public void Candidat_AssignsPropertiesCorrectly()
        {
            // Arrange
            var candidat = new Candidat
            {
                CandidatId = 5,
                Nom = "Jean Tremblay",
                PartiPolitique = "Indépendant",
                VotesObtenus = 100,
                DistrictElectoralId = 2
            };

            // Assert
            Assert.Equal(5, candidat.CandidatId);
            Assert.Equal("Jean Tremblay", candidat.Nom);
            Assert.Equal("Indépendant", candidat.PartiPolitique);
            Assert.Equal(100, candidat.VotesObtenus);
            Assert.Equal(2, candidat.DistrictElectoralId);
        }
    }
}
