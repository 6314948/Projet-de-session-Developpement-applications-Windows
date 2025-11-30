using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data;
using ProjetElectionsWinUI.Data.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjetElectionsWinUI.ViewModels
{
    public partial class DistrictsViewModel : ObservableObject
    {
        // ===============================
        //  Contexte BD (EF Core)
        // ===============================
        private readonly ElectionsContext _context;

        // ===============================
        //  Propriétés observables (MVVM)
        // ===============================

        /// Liste observable utilisée par la ListView de la page.
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        /// District présentement sélectionné dans la liste.
        [ObservableProperty]
        private DistrictElectoral selectedDistrict = new DistrictElectoral();

        /// Liste des candidats du district sélectionné
        [ObservableProperty]
        private ObservableCollection<Candidat> candidatsDuDistrict = new();

        // Description du gagnant (affiché dans la page)
        [ObservableProperty]
        private string gagnantDescription;

        /// Chaque fois que le district sélectionné change, on recharge la liste des candidats du district.
        partial void OnSelectedDistrictChanged(DistrictElectoral value)
        {
            LoadCandidatsDuDistrict();
        }


        // ===============================
        //  Messages d'erreur (Validation)
        // ===============================

        [ObservableProperty]
        private string nomDistrictErreur;

        [ObservableProperty]
        private string populationErreur;

        // ===============================
        //  Constructeur
        // ===============================
        public DistrictsViewModel()
        {
            _context = new ElectionsContext();
            LoadDistricts();
        }

        // ===============================
        //  Charger la liste des districts
        // ===============================
        private void LoadDistricts()
        {
            var list = _context.Districts.ToList();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        // ===============================
        //  Validation des champs
        // ===============================
        private bool ValiderDistrict()
        {
            bool estValide = true;

            // Réinitialiser les messages
            NomDistrictErreur = "";
            PopulationErreur = "";

            // Validation du nom
            if (string.IsNullOrWhiteSpace(SelectedDistrict.NomDistrict))
            {
                NomDistrictErreur = "Le nom du district est obligatoire.";
                estValide = false;
            }

            // Validation de la population
            if (SelectedDistrict.Population <= 0)
            {
                PopulationErreur = "La population doit être un nombre positif.";
                estValide = false;
            }

            return estValide;
        }

        // ===============================
        //  Fonction : Ajouter un district
        // ===============================
        [RelayCommand]
        private void AddDistrict()
        {
            if (!ValiderDistrict())
                return;

            _context.Districts.Add(SelectedDistrict);
            _context.SaveChanges();

            Districts.Add(SelectedDistrict);
            SelectedDistrict = new DistrictElectoral(); // reset du formulaire
        }

        // ===============================
        //  Fonction : Modifier un district
        // ===============================
        [RelayCommand]
        private void UpdateDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            if (!ValiderDistrict())
                return;

            _context.Districts.Update(SelectedDistrict);
            _context.SaveChanges();

            LoadDistricts(); // rafraîchir liste
        }

        // ===============================
        //  Fonction : Supprimer un district
        // ===============================
        public void DeleteDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            _context.Districts.Remove(SelectedDistrict);
            _context.SaveChanges();

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral();
        }

        // ===============================
        //  Fonction : Pour ajouter les candidats du district sélectionné
        // ===============================
        private void LoadCandidatsDuDistrict()
        {
            CandidatsDuDistrict.Clear();
            GagnantDescription = ""; // reset

            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
            {
                GagnantDescription = "Aucun district sélectionné.";
                return;
            }

            var candidats = _context.Candidats
                .Where(c => c.DistrictElectoralId == SelectedDistrict.DistrictElectoralId)
                .OrderByDescending(c => c.VotesObtenus)
                .ToList();

            foreach (var c in candidats)
                CandidatsDuDistrict.Add(c);

            if (candidats.Any())
            {
                var gagnant = candidats.First(); // celui avec le plus de votes
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
