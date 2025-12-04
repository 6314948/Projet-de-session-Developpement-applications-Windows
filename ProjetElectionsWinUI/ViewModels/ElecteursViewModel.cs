using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Data;
using ProjetElectionsWinUI.Data.Models;
using System;
using System.Collections.ObjectModel;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class ElecteursViewModel : ObservableObject
    {
        // ======================================================
        //   Providers (accès BD)
        // ======================================================
        private readonly ElecteurDataProvider _electeurProvider;
        private readonly DistrictDataProvider _districtProvider;

        // ======================================================
        //   Propriétés observables (MVVM)
        // ======================================================
        [ObservableProperty]
        private ObservableCollection<Electeur> electeurs;

        [ObservableProperty]
        private Electeur selectedElecteur = new Electeur();

        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        [ObservableProperty]
        private DateTimeOffset dateNaissancePicker = DateTimeOffset.Now.AddYears(-18);

        // ======================================================
        //   Messages d’erreur (validation)
        // ======================================================
        [ObservableProperty] private string nomErreur;
        [ObservableProperty] private string adresseErreur;
        [ObservableProperty] private string dateErreur;
        [ObservableProperty] private string districtErreur;

        // ======================================================
        //   Constructeur
        // ======================================================
        public ElecteursViewModel()
        {
            var context = new ElectionsContext();
            _electeurProvider = new ElecteurDataProvider(context);
            _districtProvider = new DistrictDataProvider(context);

            LoadDistricts();
            LoadElecteurs();
        }

        // ======================================================
        //   Synchronisation DatePicker ↔ Modèle
        // ======================================================
        partial void OnDateNaissancePickerChanged(DateTimeOffset value)
        {
            if (SelectedElecteur != null)
                SelectedElecteur.DateNaissance = value.DateTime;
        }

        partial void OnSelectedElecteurChanged(Electeur value)
        {
            if (value != null && value.DateNaissance != default)
                DateNaissancePicker = new DateTimeOffset(value.DateNaissance);
            else
                DateNaissancePicker = DateTimeOffset.Now.AddYears(-18);
        }

        // ======================================================
        //   Chargement des données
        // ======================================================
        private void LoadDistricts()
        {
            var list = _districtProvider.GetAll();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        private void LoadElecteurs()
        {
            var list = _electeurProvider.GetAll();
            Electeurs = new ObservableCollection<Electeur>(list);
        }

        // ======================================================
        //   Validation
        // ======================================================
        private bool ValiderElecteur()
        {
            bool valide = true;

            NomErreur = AdresseErreur = DateErreur = DistrictErreur = "";

            if (string.IsNullOrWhiteSpace(SelectedElecteur.Nom))
            {
                NomErreur = "Le nom est obligatoire.";
                valide = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedElecteur.Adresse))
            {
                AdresseErreur = "L'adresse est obligatoire.";
                valide = false;
            }

            if (SelectedElecteur.DateNaissance < new DateTime(1900, 1, 1) ||
                SelectedElecteur.DateNaissance > DateTime.Today)
            {
                DateErreur = "Date invalide, veuillez entrer une date valide.";
                valide = false;
            }

            if (SelectedElecteur.DistrictElectoralId == 0)
            {
                DistrictErreur = "Choisissez un district.";
                valide = false;
            }

            return valide;
        }

        // ======================================================
        //   Ajouter un électeur
        // ======================================================
        [RelayCommand]
        private void AddElecteur()
        {
            if (SelectedElecteur != null && SelectedElecteur.ElecteurId != 0)
            {
                NomErreur = "Cet électeur existe déjà. Utilisez Modifier pour vider le formulaire.";
                return;
            }

            if (!ValiderElecteur())
                return;

            var nouvel = new Electeur
            {
                Nom = SelectedElecteur.Nom,
                Adresse = SelectedElecteur.Adresse,
                DateNaissance = SelectedElecteur.DateNaissance,
                DistrictElectoralId = SelectedElecteur.DistrictElectoralId
            };

            _electeurProvider.Add(nouvel);
            LoadElecteurs();

            SelectedElecteur = new Electeur
            {
                DateNaissance = DateTime.Now.AddYears(-18)
            };
        }

        // ======================================================
        //   Modifier un électeur
        // ======================================================
        [RelayCommand]
        private void UpdateElecteur()
        {
            if (SelectedElecteur == null || SelectedElecteur.ElecteurId == 0)
                return;

            if (!ValiderElecteur())
                return;

            _electeurProvider.Update(SelectedElecteur);
            LoadElecteurs();

            SelectedElecteur = new Electeur
            {
                DateNaissance = DateTime.Now.AddYears(-18)
            };
        }

        // ======================================================
        //   Supprimer un électeur
        // ======================================================
        public void SupprimerElecteur()
        {
            if (SelectedElecteur == null || SelectedElecteur.ElecteurId == 0)
                return;

            _electeurProvider.Delete(SelectedElecteur.ElecteurId);
            LoadElecteurs();

            SelectedElecteur = new Electeur
            {
                DateNaissance = DateTime.Now.AddYears(-18)
            };
        }
    }
}
