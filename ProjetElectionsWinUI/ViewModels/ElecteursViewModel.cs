using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class ElecteursViewModel : ObservableObject
    {
        // ======================================================
        //   Contexte BD (EF Core)
        // ======================================================
        private readonly ElectionsContext _context;

        // ======================================================
        //   Propriétés observables (MVVM)
        // ======================================================

        /// <summary>
        /// Liste complète des électeurs affichée dans la page.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Electeur> electeurs;

        /// <summary>
        /// Électeur sélectionné dans la liste ou dans le formulaire.
        /// </summary>
        [ObservableProperty]
        private Electeur selectedElecteur = new Electeur();

        /// <summary>
        /// Liste des districts pour la ComboBox.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        /// <summary>
        /// Propriété intermédiaire utilisée par le DatePicker (DateTimeOffset).
        /// </summary>
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
            _context = new ElectionsContext();

            LoadDistricts();
            LoadElecteurs();

            // Si un électeur est déjà chargé avec une date valide
            if (SelectedElecteur.DateNaissance != default)
            {
                DateNaissancePicker = new DateTimeOffset(SelectedElecteur.DateNaissance);
            }
        }

        // ======================================================
        //   Synchronisation DatePicker ↔ Modèle
        // ======================================================

        /// <summary>
        /// Quand l’utilisateur change la date dans le DatePicker.
        /// </summary>
        partial void OnDateNaissancePickerChanged(DateTimeOffset value)
        {
            if (SelectedElecteur != null)
                SelectedElecteur.DateNaissance = value.DateTime;
        }

        /// <summary>
        /// Quand on change l’électeur sélectionné dans la liste.
        /// </summary>
        partial void OnSelectedElecteurChanged(Electeur value)
        {
            if (value != null && value.DateNaissance != default)
            {
                DateNaissancePicker = new DateTimeOffset(value.DateNaissance);
            }
            else
            {
                DateNaissancePicker = DateTimeOffset.Now.AddYears(-18);
            }
        }

        // ======================================================
        //   Chargement des données
        // ======================================================

        private void LoadDistricts()
        {
            Districts = new ObservableCollection<DistrictElectoral>(
                _context.Districts.ToList()
            );
        }

        private void LoadElecteurs()
        {
            var list = _context.Electeurs
                .Include(e => e.District)
                .ToList();

            Electeurs = new ObservableCollection<Electeur>(list);
        }

        // ======================================================
        //   Validation
        // ======================================================

        private bool ValiderElecteur()
        {
            bool valide = true;

            // Reset messages
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
                DateErreur = "Vous devez choisir une date valide.";
                valide = false;
            }

            if (SelectedElecteur.DistrictElectoralId == 0)
            {
                DistrictErreur = "Vous devez choisir un district.";
                valide = false;
            }

            return valide;
        }

        // ======================================================
        //   Commande : Ajouter un électeur
        // ======================================================
        [RelayCommand]
        private void AddElecteur()
        {
            // Empêcher d’ajouter un électeur existant
            if (SelectedElecteur != null && SelectedElecteur.ElecteurId != 0)
            {
                NomErreur = "Cet électeur existe déjà. Utilisez Modifier pour l’éditer ou videz le formulaire pour en ajouter un nouveau.";
                return;
            }

            if (!ValiderElecteur())
                return;

            var nouvelElecteur = new Electeur
            {
                Nom = SelectedElecteur.Nom,
                Adresse = SelectedElecteur.Adresse,
                DateNaissance = SelectedElecteur.DateNaissance,
                DistrictElectoralId = SelectedElecteur.DistrictElectoralId
            };

            _context.Electeurs.Add(nouvelElecteur);
            _context.SaveChanges();

            LoadElecteurs();

            SelectedElecteur = new Electeur
            {
                DateNaissance = DateTime.Now.AddYears(-18)
            };

            NomErreur = AdresseErreur = DateErreur = DistrictErreur = "";
        }

        // ======================================================
        //   Commande : Modifier un électeur
        // ======================================================
        [RelayCommand]
        private void UpdateElecteur()
        {
            if (SelectedElecteur == null || SelectedElecteur.ElecteurId == 0)
                return;

            if (!ValiderElecteur())
                return;

            _context.Electeurs.Update(SelectedElecteur);
            _context.SaveChanges();

            LoadElecteurs();

            SelectedElecteur = new Electeur
            {
                DateNaissance = DateTime.Now.AddYears(-18)
            };
        }

        // ======================================================
        //   Commande : Supprimer un électeur
        // ======================================================
        public void SupprimerElecteur()
        {
            if (SelectedElecteur == null || SelectedElecteur.ElecteurId == 0)
                return;

            _context.Electeurs.Remove(SelectedElecteur);
            _context.SaveChanges();

            LoadElecteurs();

            SelectedElecteur = new Electeur
            {
                DateNaissance = DateTime.Now.AddYears(-18)
            };
        }
    }
}
