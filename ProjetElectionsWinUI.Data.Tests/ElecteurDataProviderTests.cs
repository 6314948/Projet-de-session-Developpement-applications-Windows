using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;

public class ElecteurDataProviderTests
{
    private ElectionsContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ElectionsContext>()
            .UseInMemoryDatabase("ElecteurTestDB")
            .Options;

        return new ElectionsContext(options);
    }

    [Fact]
    public void DeleteElecteur_ShouldRemoveIt()
    {
        var context = GetContext();

        var e = new Electeur
        {
            Nom = "Test",
            Adresse = "123 rue",
            DateNaissance = new DateTime(1990, 1, 1),
            DistrictElectoralId = 1
        };

        context.Electeurs.Add(e);
        context.SaveChanges();

        var provider = new ElecteurDataProvider(context);

        provider.Delete(e.ElecteurId);

        Assert.Empty(provider.GetAll());
    }
}
