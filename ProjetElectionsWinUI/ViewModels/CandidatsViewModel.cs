using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;
using System.Collections.ObjectModel;

namespace ProjetElectionsWinUI.ViewModels
{
    /// <summary>
    /// ViewModel responsable de la gestion des candidats :
    /// chargement, validation et actions CRUD.
    /// </summary>
    public partial class CandidatsViewModel : ObservableObject
    {
        // ===============================
        //   Providers (accès BD)
        // ===============================
        private readonly CandidatDataProvider _candidatProvider;
        private readonly DistrictDataProvider _districtProvider;

        // ===============================
        //   Propriétés observables (MVVM)
        // ===============================

        // Liste affichée dans la page
        [ObservableProperty]
        private ObservableCollection<Candidat> candidats;

        // Objet sélectionné dans le formulaire ou la liste
        [ObservableProperty]
        private Candidat selectedCandidat = new Candidat();

        // Pour la ComboBox des districts
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // ===============================
        //   Messages d’erreur (validation)
        // ===============================
        [ObservableProperty] private string nomErreur;
        [ObservableProperty] private string partiErreur;
        [ObservableProperty] private string votesErreur;
        [ObservableProperty] private string districtErreur;

        // ===============================
        //   Constructeur
        // ===============================
        public CandidatsViewModel()
        {
            // Un seul contexte pour les deux providers
            var context = new ElectionsContext();
            _candidatProvider = new CandidatDataProvider(context);
            _districtProvider = new DistrictDataProvider(context);

            // Chargement initial
            LoadDistricts();
            LoadCandidats();
        }

        // ===============================
        //   Chargement des données
        // ===============================
        private void LoadDistricts()
        {
            var list = _districtProvider.GetAll();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        private void LoadCandidats()
        {
            var list = _candidatProvider.GetAll();
            Candidats = new ObservableCollection<Candidat>(list);
        }

        // ===============================
        //   Validation du formulaire
        // ===============================
        private bool ValiderCandidat()
        {
            bool estValide = true;

            // Reset des erreurs
            NomErreur = "";
            PartiErreur = "";
            VotesErreur = "";
            DistrictErreur = "";

            if (string.IsNullOrWhiteSpace(SelectedCandidat.Nom))
            {
                NomErreur = "Le nom est obligatoire.";
                estValide = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedCandidat.PartiPolitique))
            {
                PartiErreur = "Le parti est obligatoire.";
                estValide = false;
            }

            if (SelectedCandidat.VotesObtenus < 0)
            {
                VotesErreur = "Les votes doivent être positifs.";
                estValide = false;
            }

            if (SelectedCandidat.DistrictElectoralId == 0)
            {
                DistrictErreur = "Sélectionnez un district.";
                estValide = false;
            }

            return estValide;
        }

        // ===============================
        //   Ajouter un candidat
        // ===============================
        [RelayCommand]
        private void AddCandidat()
        {
            // Si l’ID n’est pas 0 → l'objet existe déjà → pas un nouvel ajout
            if (SelectedCandidat != null && SelectedCandidat.CandidatId != 0)
            {
                NomErreur = "Vous ne pouvez pas ajouter un Candidat qui est sélectionné dans la liste. Utilisez Modifier pour vider le formulaire.";
                return;
            }

            if (!ValiderCandidat())
                return;

            // Important : créer un nouvel objet pour éviter les problèmes de tracking EF
            var nouveau = new Candidat
            {
                Nom = SelectedCandidat.Nom,
                PartiPolitique = SelectedCandidat.PartiPolitique,
                VotesObtenus = SelectedCandidat.VotesObtenus,
                DistrictElectoralId = SelectedCandidat.DistrictElectoralId
            };

            _candidatProvider.Add(nouveau);

            LoadCandidats();
            SelectedCandidat = new Candidat();
        }

        // ===============================
        //   Modifier un candidat
        // ===============================
        [RelayCommand]
        private void UpdateCandidat()
        {
            if (SelectedCandidat == null || SelectedCandidat.CandidatId == 0)
                return;

            if (!ValiderCandidat())
                return;

            _candidatProvider.Update(SelectedCandidat);
            LoadCandidats();

            // On remet un objet vide pour éviter de rester sur l'ancien
            SelectedCandidat = new Candidat();
        }

        // ===============================
        //   Supprimer un candidat
        // ===============================
        public void SupprimerCandidat()
        {
            if (SelectedCandidat == null || SelectedCandidat.CandidatId == 0)
                return;

            _candidatProvider.Delete(SelectedCandidat.CandidatId);
            LoadCandidats();

            SelectedCandidat = new Candidat();
        }
    }
}
