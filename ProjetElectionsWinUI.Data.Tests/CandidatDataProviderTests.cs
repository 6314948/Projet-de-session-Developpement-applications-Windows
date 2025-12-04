using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;

public class CandidatDataProviderTests
{
    private ElectionsContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ElectionsContext>()
            .UseInMemoryDatabase("CandidatTestDB")
            .Options;

        return new ElectionsContext(options);
    }

    [Fact]
    public void GetByDistrict_ShouldReturnOnlyCandidatesFromThatDistrict()
    {
        var context = GetContext();

        context.Districts.Add(new DistrictElectoral { DistrictElectoralId = 1, NomDistrict = "Hull", Population = 50000 });
        context.Districts.Add(new DistrictElectoral { DistrictElectoralId = 2, NomDistrict = "Aylmer", Population = 40000 });

        context.Candidats.Add(new Candidat { Nom = "A", DistrictElectoralId = 1 });
        context.Candidats.Add(new Candidat { Nom = "B", DistrictElectoralId = 1 });
        context.Candidats.Add(new Candidat { Nom = "C", DistrictElectoralId = 2 });

        context.SaveChanges();

        var provider = new CandidatDataProvider(context);

        var result = provider.GetByDistrict(1);

        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, c => c.DistrictElectoralId != 1);
    }
}
