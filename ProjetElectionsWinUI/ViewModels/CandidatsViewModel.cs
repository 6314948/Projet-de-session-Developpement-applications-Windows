using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Collections.ObjectModel;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class CandidatsViewModel : ObservableObject
    {
        // ======================================================
        //   Providers (accès aux données)
        // ======================================================
        private readonly CandidatDataProvider _candidatProvider;
        private readonly DistrictDataProvider _districtProvider;

        // ======================================================
        //   Propriétés observables (MVVM)
        // ======================================================

        [ObservableProperty]
        private ObservableCollection<Candidat> candidats;

        [ObservableProperty]
        private Candidat selectedCandidat = new Candidat();

        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // ======================================================
        //   Messages d’erreur (validation)
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
            var context = new ElectionsContext();
            _candidatProvider = new CandidatDataProvider(context);
            _districtProvider = new DistrictDataProvider(context);

            LoadDistricts();
            LoadCandidats();
        }

        // ======================================================
        //   Chargement des données
        // ======================================================
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

        // ======================================================
        //   Validation
        // ======================================================
        private bool ValiderCandidat()
        {
            bool estValide = true;

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

        // ======================================================
        //   Ajouter un candidat
        // ======================================================
        [RelayCommand]
        private void AddCandidat()
        {
            if (SelectedCandidat != null && SelectedCandidat.CandidatId != 0)
            {
                NomErreur = "Ce candidat existe déjà. Utilisez Modifier pour vider le formulaire.";
                return;
            }

            if (!ValiderCandidat())
                return;

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

        // ======================================================
        //   Modifier un candidat
        // ======================================================
        [RelayCommand]
        private void UpdateCandidat()
        {
            if (SelectedCandidat == null || SelectedCandidat.CandidatId == 0)
                return;

            if (!ValiderCandidat())
                return;

            _candidatProvider.Update(SelectedCandidat);
            LoadCandidats();

            SelectedCandidat = new Candidat();
        }

        // ======================================================
        //   Supprimer un candidat
        // ======================================================
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
