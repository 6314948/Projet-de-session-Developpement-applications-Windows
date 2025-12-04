using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using Xunit;

public class DistrictDataProviderTests
{
    private ElectionsContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ElectionsContext>()
            .UseInMemoryDatabase("DistrictTestDB")
            .Options;

        return new ElectionsContext(options);
    }

    [Fact]
    public void AddDistrict_ShouldAppearInGetAll()
    {
        var context = GetContext();
        var provider = new DistrictDataProvider(context);

        var district = new DistrictElectoral { NomDistrict = "Test District", Population = 5000 };

        provider.Add(district);

        var result = provider.GetAll();

        Assert.Single(result);
        Assert.Equal("Test District", result[0].NomDistrict);
    }

    [Fact]
    public void UpdateDistrict_ShouldModifyValues()
    {
        var context = GetContext();
        var provider = new DistrictDataProvider(context);

        var district = new DistrictElectoral { NomDistrict = "Old Name", Population = 1000 };
        provider.Add(district);

        var added = provider.GetAll().First();
        added.NomDistrict = "New Name";

        provider.Update(added);

        var updated = provider.GetAll().First();
        Assert.Equal("New Name", updated.NomDistrict);
    }

}
