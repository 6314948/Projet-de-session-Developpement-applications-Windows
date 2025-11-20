using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class DistrictsViewModel : ObservableObject
    {
        private readonly ElectionsContext _context;

        // ===============================
        //  Propriétés observables MVVM
        // ===============================

        // Liste des districts affichée dans le DataGrid
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        // District sélectionné dans le DataGrid
        [ObservableProperty]
        private DistrictElectoral selectedDistrict = new DistrictElectoral();

        // Message d'erreur
        [ObservableProperty]
        private string errorMessage = "";

        // ===============================
        //  Constructeur
        // ===============================
        public DistrictsViewModel()
        {
            _context = new ElectionsContext();
            LoadDistricts();
        }

        // ===============================
        //  Charger les districts depuis la BD
        // ===============================
        private void LoadDistricts()
        {
            var list = _context.Districts.ToList();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        // ===============================
        //  Commande : Ajouter
        // ===============================
        [RelayCommand]
        private void AddDistrict()
        {
            ErrorMessage = ""; // reset

            // Validation du nom vide
            if (string.IsNullOrWhiteSpace(SelectedDistrict.NomDistrict))
            {
                ErrorMessage = "Le nom du district ne peut pas être vide.";
                return;
            }

            // Validation population
            if (SelectedDistrict.Population <= 0)
            {
                ErrorMessage = "La population doit être un nombre positif.";
                return;
            }

            // Validation — NOM UNIQUE
            if (_context.Districts.Any(d => d.NomDistrict == SelectedDistrict.NomDistrict))
            {
                ErrorMessage = "Un district avec ce nom existe déjà.";
                return;
            }

            // Ajout dans la BD
            _context.Districts.Add(SelectedDistrict);
            _context.SaveChanges();

            // Ajout dans la liste
            Districts.Add(SelectedDistrict);

            // Reset du formulaire
            SelectedDistrict = new DistrictElectoral();
        }


        // ===============================
        //  Commande : Modifier
        // ===============================
        [RelayCommand]
        private void UpdateDistrict()
        {
            ErrorMessage = ""; // reset

            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
            {
                ErrorMessage = "Veuillez sélectionner un district.";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedDistrict.NomDistrict))
            {
                ErrorMessage = "Le nom du district ne peut pas être vide.";
                return;
            }

            if (SelectedDistrict.Population <= 0)
            {
                ErrorMessage = "La population doit être un nombre positif.";
                return;
            }

            // Validation — nom unique (sauf si c'est le même district)
            bool duplicate = _context.Districts
                .Any(d => d.NomDistrict == SelectedDistrict.NomDistrict
                       && d.DistrictElectoralId != SelectedDistrict.DistrictElectoralId);

            if (duplicate)
            {
                ErrorMessage = "Un autre district possède déjà ce nom.";
                return;
            }

            // Mise à jour
            _context.Districts.Update(SelectedDistrict);
            _context.SaveChanges();

            LoadDistricts();
        }


        // ===============================
        //  Commande : Supprimer
        // ===============================
        [RelayCommand]
        private void DeleteDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            _context.Districts.Remove(SelectedDistrict);
            _context.SaveChanges();

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral(); // reset
        }
    }
}
