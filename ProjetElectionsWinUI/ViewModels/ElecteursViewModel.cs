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
        private readonly ElectionsContext _context;

        // ===========================
        //   Propriétés observables
        // ===========================
        [ObservableProperty]
        private ObservableCollection<Electeur> electeurs;

        [ObservableProperty]
        private Electeur selectedElecteur = new Electeur();

        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // ===========================
        //   Messages d'erreur
        // ===========================
        [ObservableProperty] private string nomErreur;
        [ObservableProperty] private string adresseErreur;
        [ObservableProperty] private string dateErreur;
        [ObservableProperty] private string districtErreur;

        // ===========================
        //   Constructeur
        // ===========================
        public ElecteursViewModel()
        {
            _context = new ElectionsContext();
            LoadDistricts();
            LoadElecteurs();
        }

        // ===========================
        //   Chargement des données
        // ===========================
        private void LoadDistricts()
        {
            Districts = new ObservableCollection<DistrictElectoral>(_context.Districts.ToList());
        }

        private void LoadElecteurs()
        {
            var list = _context.Electeurs
                .Include(e => e.District)
                .ToList();

            Electeurs = new ObservableCollection<Electeur>(list);
        }

        // ===========================
        //          Validation
        // ===========================
        private bool ValiderElecteur()
        {
            bool valide = true;

            // Réinitialiser les messages d'erreur
            NomErreur = AdresseErreur = DateErreur = DistrictErreur = "";

            // Nom obligatoire
            if (string.IsNullOrWhiteSpace(SelectedElecteur.Nom))
            {
                NomErreur = "Le nom est obligatoire.";
                valide = false;
            }

            // Adresse obligatoire
            if (string.IsNullOrWhiteSpace(SelectedElecteur.Adresse))
            {
                AdresseErreur = "L'adresse est obligatoire.";
                valide = false;
            }

            // Date valide + pas dans le futur
            if (SelectedElecteur.DateNaissance == default)
            {
                DateErreur = "Vous devez choisir une date valide.";
                valide = false;
            }
            else if (SelectedElecteur.DateNaissance > DateTime.Today)
            {
                DateErreur = "La date de naissance ne peut pas être dans le futur.";
                valide = false;
            }

            // District obligatoire
            if (SelectedElecteur.DistrictElectoralId == 0)
            {
                DistrictErreur = "Vous devez choisir un district.";
                valide = false;
            }

            return valide;
        }


        // ===========================
        //          CRUD
        // ===========================
        [RelayCommand]
        private void AddElecteur()
        {
            if (!ValiderElecteur())
                return;

            _context.Electeurs.Add(SelectedElecteur);
            _context.SaveChanges();

            LoadElecteurs();
            SelectedElecteur = new Electeur();
        }

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
        }

        public void SupprimerElecteur()
        {
            if (SelectedElecteur == null || SelectedElecteur.ElecteurId == 0)
                return;

            _context.Electeurs.Remove(SelectedElecteur);
            _context.SaveChanges();

            LoadElecteurs();
            SelectedElecteur = new Electeur();
        }
    }
}
