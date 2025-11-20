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
            if (string.IsNullOrWhiteSpace(SelectedDistrict.NomDistrict))
                return;

            _context.Districts.Add(SelectedDistrict);
            _context.SaveChanges();

            Districts.Add(SelectedDistrict);

            SelectedDistrict = new DistrictElectoral(); // reset
        }

        // ===============================
        //  Commande : Modifier
        // ===============================
        [RelayCommand]
        private void UpdateDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            _context.Districts.Update(SelectedDistrict);
            _context.SaveChanges();

            LoadDistricts(); // refresh list
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
