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
        // ======================================================
        //   Contexte de la base de données (EF Core)
        // ======================================================
        private readonly ElectionsContext _context;

        // ======================================================
        //   Propriétés observables (MVVM)
        // ======================================================

        /// <summary>
        /// Liste des candidats affichés dans la page.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Candidat> candidats;

        /// <summary>
        /// Candidat actuellement sélectionné dans la liste ou dans le formulaire.
        /// </summary>
        [ObservableProperty]
        private Candidat selectedCandidat = new Candidat();

        /// <summary>
        /// Liste des districts chargée pour la ComboBox.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // ======================================================
        //   Messages d’erreur pour la validation
        // ======================================================
        [ObservableProperty] private string nomErreur;
        [ObservableProperty] private string partiErreur;
        [ObservableProperty] private string votesErreur;
        [ObservableProperty] private string districtErreur;

        // ======================================================
        //   Constructeur
        // ======================================================
        public CandidatsViewModel()
        {
            _context = new ElectionsContext();
            LoadDistricts();
            LoadCandidats();
        }

        // ======================================================
        //   Chargement des données
        // ======================================================

        private void LoadDistricts()
        {
            var list = _context.Districts.ToList();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        private void LoadCandidats()
        {
            // Include pour charger aussi le district lié
            var list = _context.Candidats
                .Include(c => c.District)
                .ToList();

            Candidats = new ObservableCollection<Candidat>(list);
        }

        // ======================================================
        //   Validation des champs du formulaire
        // ======================================================

        private bool ValiderCandidat()
        {
            bool estValide = true;

            // Reset des messages d'erreur
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

        // ======================================================
        //      Commande : Ajouter un candidat
        // ======================================================
        [RelayCommand]
        private void AddCandidat()
        {
            // Empêche l’ajout d’un candidat déjà existant
            if (SelectedCandidat != null && SelectedCandidat.CandidatId != 0)
            {
                NomErreur = "Ce candidat existe déjà. Utilisez Modifier pour mettre à jour ou videz le formulaire pour ajouter un nouveau.";
                return;
            }

            // Validation du formulaire
            if (!ValiderCandidat())
                return;

            // Important : créer un nouvel objet pour éviter les conflits EF Core
            var nouveauCandidat = new Candidat
            {
                Nom = SelectedCandidat.Nom,
                PartiPolitique = SelectedCandidat.PartiPolitique,
                VotesObtenus = SelectedCandidat.VotesObtenus,
                DistrictElectoralId = SelectedCandidat.DistrictElectoralId
            };

            _context.Candidats.Add(nouveauCandidat);
            _context.SaveChanges();

            // Rafraîchir l’affichage
            LoadCandidats();

            // Reset du formulaire
            SelectedCandidat = new Candidat();

            // Réinitialiser les erreurs
            NomErreur = PartiErreur = VotesErreur = DistrictErreur = "";
        }

        // ======================================================
        //      Commande : Modifier un candidat
        // ======================================================
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

            // Reset du formulaire
            SelectedCandidat = new Candidat();
        }

        // ======================================================
        //      Commande : Supprimer un candidat
        //      (appelée depuis la View, avec confirmation)
        // ======================================================
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
