using ProjetElectionsWinUI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetElectionsWinUi.Data.Models
{
    public class Candidat
    {
        public int CandidatId { get; set; }    // Clé primaire (PK)
        public string Nom { get; set; } // Colonne du nom du candidat (String)
        public string PartiPolitique { get; set; } // Colonne du nom du parti politique (String)
        public int VotesObtenus { get; set; } // Colone du nomre de votes obtenus (Int)

        // Clé étrangère (FK) --> Quel district ce candidat appartient
        public int DistrictElectoralId { get; set; }

        // Relation navigation (District = Disctrict du Candidat)
        public DistrictElectoral District { get; set; }
    }
}
