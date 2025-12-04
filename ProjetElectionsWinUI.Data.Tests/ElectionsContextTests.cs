using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUI.Data;

public class ElectionsContextTests
{
    [Fact]
    public void Context_ShouldInitializeDbSets()
    {
        var options = new DbContextOptionsBuilder<ElectionsContext>()
            .UseInMemoryDatabase("ContextTestDB")
            .Options;

        var context = new ElectionsContext(options);

        Assert.NotNull(context.Candidats);
        Assert.NotNull(context.Districts);
        Assert.NotNull(context.Electeurs);
    }
}
