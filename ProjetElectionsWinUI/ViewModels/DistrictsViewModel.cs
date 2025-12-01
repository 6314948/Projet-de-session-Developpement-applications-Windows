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
        // ======================================================
        //   Contexte BD (EF Core)
        // ======================================================
        private readonly ElectionsContext _context;

        // ======================================================
        //   Propriétés observables (MVVM)
        // ======================================================

        /// <summary>
        /// Liste observable affichée dans la ListView.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DistrictElectoral> districts;

        /// <summary>
        /// District sélectionné dans la liste ou en édition.
        /// </summary>
        [ObservableProperty]
        private DistrictElectoral selectedDistrict = new DistrictElectoral();

        /// <summary>
        /// Candidats du district sélectionné.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Candidat> candidatsDuDistrict = new();

        /// <summary>
        /// Texte affichant le gagnant (calcul LINQ).
        /// </summary>
        [ObservableProperty]
        private string gagnantDescription;

        // Quand le district sélectionné change → on recharge les candidats liés
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
            _context = new ElectionsContext();
            LoadDistricts();
        }

        // ======================================================
        //   Chargement des données
        // ======================================================

        private void LoadDistricts()
        {
            var list = _context.Districts.ToList();
            Districts = new ObservableCollection<DistrictElectoral>(list);
        }

        // ======================================================
        //   Validation
        // ======================================================

        private bool ValiderDistrict()
        {
            bool estValide = true;

            // Reset des messages
            NomDistrictErreur = "";
            PopulationErreur = "";

            // Nom obligatoire
            if (string.IsNullOrWhiteSpace(SelectedDistrict.NomDistrict))
            {
                NomDistrictErreur = "Le nom du district est obligatoire.";
                estValide = false;
            }

            // Population > 0
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
            // Empêcher l’ajout d’un district existant
            if (SelectedDistrict != null && SelectedDistrict.DistrictElectoralId != 0)
            {
                NomDistrictErreur = "Ce district existe déjà. Utilisez Modifier pour le mettre à jour ou videz le formulaire pour en créer un nouveau.";
                return;
            }

            if (!ValiderDistrict())
                return;

            // Toujours créer un nouvel objet pour éviter les conflits EF Core
            var nouveauDistrict = new DistrictElectoral
            {
                NomDistrict = SelectedDistrict.NomDistrict,
                Population = SelectedDistrict.Population
            };

            _context.Districts.Add(nouveauDistrict);
            _context.SaveChanges();

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

            _context.Districts.Update(SelectedDistrict);
            _context.SaveChanges();

            LoadDistricts();
            SelectedDistrict = new DistrictElectoral();
        }

        // ======================================================
        //   Commande : Supprimer un district (appel depuis la page)
        // ======================================================
        public void DeleteDistrict()
        {
            if (SelectedDistrict == null || SelectedDistrict.DistrictElectoralId == 0)
                return;

            _context.Districts.Remove(SelectedDistrict);
            _context.SaveChanges();

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

            var candidats = _context.Candidats
                .Where(c => c.DistrictElectoralId == SelectedDistrict.DistrictElectoralId)
                .OrderByDescending(c => c.VotesObtenus)
                .ToList();

            foreach (var c in candidats)
                CandidatsDuDistrict.Add(c);

            // Calcul du gagnant (LINQ)
            if (candidats.Any())
            {
                var gagnant = candidats.First();
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
