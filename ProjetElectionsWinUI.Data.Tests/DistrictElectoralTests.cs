using ProjetElectionsWinUI.Data.Models;
using Xunit;

namespace ProjetElectionsWinUI.Data.Tests
{
    /// <summary>
    /// Vérifie que l'objet DistrictElectoral stocke correctement les valeurs assignées.
    /// </summary>
    public class DistrictElectoralTests
    {
        [Fact]
        public void DistrictElectoral_AssignsPropertiesCorrectly()
        {
            // Arrange : création d'un district avec données connues
            var district = new DistrictElectoral
            {
                DistrictElectoralId = 10,
                NomDistrict = "Hull",
                Population = 72000
            };

            // Act + Assert : vérifier que les valeurs sont bien conservées
            Assert.Equal(10, district.DistrictElectoralId);
            Assert.Equal("Hull", district.NomDistrict);
            Assert.Equal(72000, district.Population);
        }
    }
}
