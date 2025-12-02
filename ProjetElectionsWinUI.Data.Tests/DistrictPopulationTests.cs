using ProjetElectionsWinUI.Data.Models;
using Xunit;

namespace ProjetElectionsWinUI.Data.Tests
{
    /// <summary>
    /// Vérifie qu'un DistrictElectoral peut être créé avec une population valide.
    /// </summary>
    public class DistrictPopulationTests
    {
        [Fact]
        public void District_CanBeCreated_WithValidPopulation()
        {
            // Arrange
            int population = 45000;

            // Act
            var district = new DistrictElectoral
            {
                NomDistrict = "TestDistrict",
                Population = population
            };

            // Assert
            Assert.Equal("TestDistrict", district.NomDistrict);
            Assert.Equal(population, district.Population);
        }
    }
}
