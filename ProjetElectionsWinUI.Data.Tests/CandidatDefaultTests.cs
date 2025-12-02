using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using Xunit;

namespace ProjetElectionsWinUI.Data.Tests
{
    /// <summary>
    /// Vérifie que la propriété VotesObtenus d'un candidat vaut 0 par défaut.
    /// </summary>
    public class CandidatDefaultTests
    {
        [Fact]
        public void Candidat_VotesObtenus_DefaultsToZero()
        {
            // Arrange : création sans initialiser VotesObtenus
            var candidat = new Candidat();

            // Assert : EF initialise les entiers à 0 par défaut
            Assert.Equal(0, candidat.VotesObtenus);
        }
    }
}
