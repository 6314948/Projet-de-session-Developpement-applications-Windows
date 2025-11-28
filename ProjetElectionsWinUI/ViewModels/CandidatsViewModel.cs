using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class CandidatsViewModel : ObservableObject
    {
        // ===============================
        //  Contexte BD (EF Core)
        // ===============================
        private readonly ElectionsContext _context;

        // ===============================
        //  Propriétés observables
        // ===============================

        /// <summary>
        /// Liste des candidats affichés dans la ListView.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Candidat> candidats;

        /// <summary>
        /// Candidat sélectionné dans la liste ou en édition.
        /// </summary>
        [ObservableProperty]
        private Candidat selectedCandidat = new Candidat();

        /// <summary>
        /// Liste des districts pour la ComboBox.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // ===============================
        //  Messages d'erreur (validation)
        // ===============================

        [ObservableProperty]
        private string nomErreur;

        [ObservableProperty]
        private string partiErreur;

        [ObservableProperty]
        private string votesErreur;

        [ObservableProperty]
        private string districtErreur;

        // ===============================
        //  Constructeur
        // ===============================
        public CandidatsViewModel()
        {
            _context = new ElectionsContext();
            LoadDistricts();
            LoadCandidats();
        }

        // ===============================
        //  Chargement des données
        // ===============================
        private void LoadDistricts()
        {
            var list = _context.Districts.ToList();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        private void LoadCandidats()
        {
            // Include pour charger aussi le District (pour l'affichage dans la liste)
            var list = _context.Candidats
                .Include(c => c.District)
                .ToList();

            Candidats = new ObservableCollection<Candidat>(list);
        }

        // ===============================
        //  Validation
        // ===============================
        private bool ValiderCandidat()
        {
            bool estValide = true;

            // Reset des erreurs
            NomErreur = "";
            PartiErreur = "";
            VotesErreur = "";
            DistrictErreur = "";

            // Nom obligatoire
            if (string.IsNullOrWhiteSpace(SelectedCandidat.Nom))
            {
                NomErreur = "Le nom du candidat est obligatoire.";
                estValide = false;
            }

            // Parti obligatoire
            if (string.IsNullOrWhiteSpace(SelectedCandidat.PartiPolitique))
            {
                PartiErreur = "Le parti politique est obligatoire.";
                estValide = false;
            }

            // Votes >= 0
            if (SelectedCandidat.VotesObtenus < 0)
            {
                VotesErreur = "Le nombre de votes doit être positif ou nul.";
                estValide = false;
            }

            // District obligatoire
            if (SelectedCandidat.DistrictElectoralId == 0)
            {
                DistrictErreur = "Vous devez choisir un district.";
                estValide = false;
            }

            return estValide;
        }

        // ===============================
        //  Commande : Ajouter un candidat
        // ===============================
        [RelayCommand]
        private void AddCandidat()
        {
            if (!ValiderCandidat())
                return;

            _context.Candidats.Add(SelectedCandidat);
            _context.SaveChanges();

            // Recharger pour mettre à jour aussi la propriété District (Include)
            LoadCandidats();

            SelectedCandidat = new Candidat(); // reset formulaire
        }

        // ===============================
        //  Commande : Modifier un candidat
        // ===============================
        [RelayCommand]
        private void UpdateCandidat()
        {
            if (SelectedCandidat == null || SelectedCandidat.CandidatId == 0)
                return;

            if (!ValiderCandidat())
                return;

            _context.Candidats.Update(SelectedCandidat);
            _context.SaveChanges();

            LoadCandidats();
        }

        // ===============================
        //  Méthode appelée depuis la page pour supprimer
        // ===============================
        public void SupprimerCandidat()
        {
            if (SelectedCandidat == null || SelectedCandidat.CandidatId == 0)
                return;

            _context.Candidats.Remove(SelectedCandidat);
            _context.SaveChanges();

            LoadCandidats();
            SelectedCandidat = new Candidat();
        }
    }
}
