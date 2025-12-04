using Xunit;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Linq;

namespace ProjetElectionsWinUI.Tests
{
    /// <summary>
    /// Tests unitaires pour DistrictDataProvider.
    /// </summary>
    public class DistrictDataProviderTests
    {
        private ElectionsContext CreateContext() => new ElectionsContext();

        [Fact]
        public void GetAll_ReturnsAtLeastSeededDistricts()
        {
            using var context = CreateContext();
            var provider = new DistrictDataProvider(context);

            var districts = provider.GetAll();

            Assert.NotNull(districts);
            Assert.True(districts.Count >= 1);
        }

        [Fact]
        public void AddAndDeleteDistrict_WorksCorrectly()
        {
            using var context = CreateContext();
            var provider = new DistrictDataProvider(context);

            int before = provider.GetAll().Count;

            var newDistrict = new DistrictElectoral
            {
                NomDistrict = "TestDistrict_Unitaire",
                Population = 12345
            };

            provider.Add(newDistrict);

            var afterAdd = provider.GetAll();
            Assert.Equal(before + 1, afterAdd.Count);

            var created = afterAdd.FirstOrDefault(d => d.NomDistrict == "TestDistrict_Unitaire");
            Assert.NotNull(created);

            provider.Delete(created!.DistrictElectoralId);

            var afterDelete = provider.GetAll();
            Assert.Equal(before, afterDelete.Count);
        }
    }
}
