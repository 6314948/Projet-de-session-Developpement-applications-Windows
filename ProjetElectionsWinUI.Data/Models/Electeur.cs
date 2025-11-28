using ProjetElectionsWinUI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetElectionsWinUi.Data.Models
{
    public class Electeur
    {
        public int ElecteurId { get; set; }   // Clé Primaire (PK)
        public string Nom { get; set; } // Colonne du Nom de l'électeur (String)
        public string Adresse { get; set; } // Colonne de l'adresse de l'électeur (String)
        public DateTime DateNaissance { get; set; } // Colonne de la date de naissance de l'électeur (DateTime)
        public string DateNaissanceDisplay { get => DateNaissance.ToString("dd-MM-yyyy"); }// Propriété pour affichage formaté

        // Clé étrangère (FK) --> Quel district cet électeur appartient
        public int DistrictElectoralId { get; set; }

        // Relation navigation (District = Disctrict de l'électeur)
        public DistrictElectoral District { get; set; }
    }
}
