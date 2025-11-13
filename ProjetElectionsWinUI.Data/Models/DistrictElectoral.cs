using ProjetElectionsWinUi.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetElectionsWinUI.Data.Models
{
    public class DistrictElectoral
    {
        public int DistrictElectoralId { get; set; }   // Clé Primaite (PK)
        public string NomDistrict { get; set; } // Colonne du nom du district (String)
        public int Population { get; set; } // Colonne du nombre de gens d'un district (Int)

        // Relations
        public List<Candidat> Candidats { get; set; } = new(); // Un district peut avoir plusieurs candidats
        public List<Electeur> Electeurs { get; set; } = new(); // Un distric peut avoir plusieurs électeurs
    }
}
