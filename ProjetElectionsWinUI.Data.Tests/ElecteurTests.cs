using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using System;
using Xunit;

namespace ProjetElectionsWinUI.Data.Tests
{
    /// <summary>
    /// Vérifie que l'objet Electeur stocke correctement les données qui lui sont assignées.
    /// </summary>
    public class ElecteurTests
    {
        [Fact]
        public void Electeur_AssignsPropertiesCorrectly()
        {
            // Arrange
            var date = new DateTime(1998, 6, 15);

            var electeur = new Electeur
            {
                ElecteurId = 7,
                Nom = "Alice Martin",
                Adresse = "123 Rue Principale",
                DateNaissance = date,
                DistrictElectoralId = 1
            };

            // Assert
            Assert.Equal(7, electeur.ElecteurId);
            Assert.Equal("Alice Martin", electeur.Nom);
            Assert.Equal("123 Rue Principale", electeur.Adresse);
            Assert.Equal(date, electeur.DateNaissance);
            Assert.Equal(1, electeur.DistrictElectoralId);
        }
    }
}
