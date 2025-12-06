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
    /// <summary>
    /// ViewModel responsable de la gestion des électeurs.
    /// On gère le CRUD via un DataProvider et on garde la logique
    /// de validation + synchronisation avec le DatePicker.
    /// </summary>
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

        // Liste affichée dans la page
        [ObservableProperty]
        private ObservableCollection<Electeur> electeurs;

        // Électeur sélectionné dans la liste ou dans le formulaire
        [ObservableProperty]
        private Electeur selectedElecteur = new Electeur();

        // Liste des districts pour la ComboBox
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // Propriété intermédiaire pour le DatePicker
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

        // DatePicker -> modèle
        partial void OnDateNaissancePickerChanged(DateTimeOffset value)
        {
            if (SelectedElecteur != null)
                SelectedElecteur.DateNaissance = value.DateTime;
        }

        // Sélection -> DatePicker
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
        //   Validation des champs du formulaire
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
                DateErreur = "Date de naissance invalide.";
                valide = false;
            }

            if (SelectedElecteur.DistrictElectoralId == 0)
            {
                DistrictErreur = "Veuillez choisir un district.";
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
            // Si l'électeur a déjà un ID → il existe déjà
            if (SelectedElecteur != null && SelectedElecteur.ElecteurId != 0)
            {
                NomErreur = "Vous ne pouvez pas ajouter un Electeur qui est sélectionné dans la liste. Utilisez Modifier pour vider le formulaire.";
                return;
            }

            if (!ValiderElecteur())
                return;

            // Création d’un nouvel objet pour éviter les conflits EF Core
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
        //   Modifier un électeur existant
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
