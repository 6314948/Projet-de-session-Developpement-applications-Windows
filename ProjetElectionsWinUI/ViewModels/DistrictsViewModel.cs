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
    /// ViewModel responsable de la gestion des districts.
    /// On charge la liste des districts, on valide les données du formulaire
    /// et on utilise le DistrictDataProvider pour faire les opérations CRUD.
    /// </summary>
    public partial class DistrictsViewModel : ObservableObject
    {
        // ===============================
        //   Provider (accès BD)
        // ===============================
        private readonly DistrictDataProvider _districtProvider;

        // ===============================
        //   Propriétés observables
        // ===============================

        // Liste affichée dans la ListView
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // District sélectionné pour le formulaire
        [ObservableProperty]
        private DistrictElectoral selectedDistrict = new DistrictElectoral();

        // Liste des candidats du district sélectionné
        [ObservableProperty]
        private ObservableCollection<Candidat> candidatsDuDistrict = new();

        // Description du gagnant (affiché dans la page)
        [ObservableProperty]
        private string gagnantDescription;

        // Quand l'utilisateur change de district → recharge la liste des candidats associés
        partial void OnSelectedDistrictChanged(DistrictElectoral value)
        {
            LoadCandidatsDuDistrict();
        }

        // ===============================
        //   Messages d’erreur (MVVM)
        // ===============================
        [ObservableProperty] private string nomDistrictErreur;
        [ObservableProperty] private string populationErreur;

        // ===============================
        //   Constructeur
        // ===============================
        public DistrictsViewModel()
        {
            var context = new ElectionsContext();
            _districtProvider = new DistrictDataProvider(context);

            LoadDistricts();
        }

        // ===============================
        //   Chargement de la liste
        // ===============================
        private void LoadDistricts()
        {
            var list = _districtProvider.GetAll();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        // ===============================
        //   Validation des champs
        // ===============================
        private bool ValiderDistrict()
        {
            bool estValide = true;

            // Reset des messages d’erreur
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

        // ===============================
        //   Ajouter un district
        // ===============================
        [RelayCommand]
        private void AddDistrict()
        {
            // Empêche d’ajouter un district déjà existant
            if (SelectedDistrict != null && SelectedDistrict.DistrictElectoralId != 0)
            {
                NomDistrictErreur = "Vous ne pouvez pas ajouter un District qui est sélectionné dans la liste. Utilisez Modifier pour vider le formulaire.";
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

        // ===============================
        //   Modifier un district
        // ===============================
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

        // ===============================
        //   Supprimer un district
        // ===============================
        public void DeleteDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            _districtProvider.Delete(SelectedDistrict.DistrictElectoralId);

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral();
        }

        // ===============================
        //   Candidats + gagnant du district
        // ===============================
        private void LoadCandidatsDuDistrict()
        {
            CandidatsDuDistrict.Clear();
            GagnantDescription = "";

            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
            {
                GagnantDescription = "Aucun district sélectionné.";
                return;
            }

            // On récupère les candidats via le provider
            var candidats = _districtProvider.GetCandidatsByDistrict(SelectedDistrict.DistrictElectoralId);

            foreach (var c in candidats)
                CandidatsDuDistrict.Add(c);

            // On détermine le gagnant
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
