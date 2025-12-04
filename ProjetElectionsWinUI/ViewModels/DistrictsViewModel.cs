using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Collections.ObjectModel;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class DistrictsViewModel : ObservableObject
    {
        // ======================================================
        //   Providers (accès BD via Data Providers)
        // ======================================================
        private readonly DistrictDataProvider _districtProvider;

        // ======================================================
        //   Propriétés observables (MVVM)
        // ======================================================

        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        [ObservableProperty]
        private DistrictElectoral selectedDistrict = new DistrictElectoral();

        [ObservableProperty]
        private ObservableCollection<Candidat> candidatsDuDistrict = new();

        [ObservableProperty]
        private string gagnantDescription;

        partial void OnSelectedDistrictChanged(DistrictElectoral value)
        {
            LoadCandidatsDuDistrict();
        }

        // ======================================================
        //   Messages d’erreur (validation)
        // ======================================================
        [ObservableProperty] private string nomDistrictErreur;
        [ObservableProperty] private string populationErreur;

        // ======================================================
        //   Constructeur
        // ======================================================
        public DistrictsViewModel()
        {
            var context = new ElectionsContext();
            _districtProvider = new DistrictDataProvider(context);

            LoadDistricts();
        }

        // ======================================================
        //   Chargement des données
        // ======================================================
        private void LoadDistricts()
        {
            var list = _districtProvider.GetAll();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        // ======================================================
        //   Validation
        // ======================================================
        private bool ValiderDistrict()
        {
            bool estValide = true;

            NomDistrictErreur = "";
            PopulationErreur = "";

            if (string.IsNullOrWhiteSpace(SelectedDistrict.NomDistrict))
            {
                NomDistrictErreur = "Le nom du district est obligatoire.";
                estValide = false;
            }

            if (SelectedDistrict.Population <= 0)
            {
                PopulationErreur = "La population doit être un nombre positif.";
                estValide = false;
            }

            return estValide;
        }

        // ======================================================
        //   Commande : Ajouter un district
        // ======================================================
        [RelayCommand]
        private void AddDistrict()
        {
            if (SelectedDistrict != null && SelectedDistrict.DistrictElectoralId != 0)
            {
                NomDistrictErreur = "Ce district existe déjà. Utilisez Modifier pour vider le formulaire.";
                return;
            }

            if (!ValiderDistrict())
                return;

            var nouveau = new DistrictElectoral
            {
                NomDistrict = SelectedDistrict.NomDistrict,
                Population = SelectedDistrict.Population
            };

            _districtProvider.Add(nouveau);

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral();
        }

        // ======================================================
        //   Commande : Modifier un district
        // ======================================================
        [RelayCommand]
        private void UpdateDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            if (!ValiderDistrict())
                return;

            _districtProvider.Update(SelectedDistrict);

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral();
        }

        // ======================================================
        //   Commande : Supprimer un district
        // ======================================================
        public void DeleteDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            _districtProvider.Delete(SelectedDistrict.DistrictElectoralId);

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral();
        }

        // ======================================================
        //   Charger les candidats du district + gagnant
        // ======================================================
        private void LoadCandidatsDuDistrict()
        {
            CandidatsDuDistrict.Clear();
            GagnantDescription = "";

            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
            {
                GagnantDescription = "Aucun district sélectionné.";
                return;
            }

            var candidats = _districtProvider.GetCandidatsByDistrict(SelectedDistrict.DistrictElectoralId);

            foreach (var c in candidats)
                CandidatsDuDistrict.Add(c);

            var gagnant = _districtProvider.GetGagnant(SelectedDistrict.DistrictElectoralId);

            if (gagnant != null)
            {
                GagnantDescription =
                    $"Gagnant : {gagnant.Nom} ({gagnant.PartiPolitique}) - {gagnant.VotesObtenus} votes";
            }
            else
            {
                GagnantDescription = "Aucun candidat pour ce district.";
            }
        }
    }
}
