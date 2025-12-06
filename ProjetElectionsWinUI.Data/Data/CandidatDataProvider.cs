using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;

namespace ProjetElectionsWinUI.Data.Data
{
    /// <summary>
    /// Classe responsable d'accéder aux données des candidats.
    /// On isole la logique EF Core ici pour garder les ViewModels plus simples.
    /// </summary>
    public class CandidatDataProvider
    {
        private readonly ElectionsContext _context;

        public CandidatDataProvider(ElectionsContext context)
        {
            // On garde une seule instance du contexte ici,
            // car les ViewModels créent eux-mêmes le provider.
            _context = context;
        }

        // ======================================================
        // 1. GET ALL CANDIDATS (avec leur district lié)
        // ======================================================
        public List<Candidat> GetAll()
        {
            // Include permet de charger le district du candidat.
            // Ça évite de devoir aller le chercher séparément dans le ViewModel.
            return _context.Candidats
                .Include(c => c.District)
                .OrderBy(c => c.Nom)
                .ToList();
        }

        // ======================================================
        // 2. GET BY ID
        // ======================================================
        public Candidat? GetById(int id)
        {
            return _context.Candidats
                .Include(c => c.District)    // utile pour l'affichage
                .FirstOrDefault(c => c.CandidatId == id);
        }

        // ======================================================
        // 3. GET BY DISTRICT
        //    Pratique pour afficher les candidats d’un district
        //    (ex : DistrictsPage)
        // ======================================================
        public List<Candidat> GetByDistrict(int districtId)
        {
            return _context.Candidats
                .Include(c => c.District)
                .Where(c => c.DistrictElectoralId == districtId)
                .OrderByDescending(c => c.VotesObtenus) // le premier = gagnant
                .ToList();
        }

        // ======================================================
        // 4. ADD
        // ======================================================
        public void Add(Candidat candidat)
        {
            // Rien de spécial ici : EF suit l’objet et le sauvegarde.
            _context.Candidats.Add(candidat);
            _context.SaveChanges();
        }

        // ======================================================
        // 5. UPDATE
        // ======================================================
        public void Update(Candidat candidat)
        {
            // EF détecte que l’objet existe et applique les modifications.
            _context.Candidats.Update(candidat);
            _context.SaveChanges();
        }

        // ======================================================
        // 6. DELETE
        // ======================================================
        public void Delete(int id)
        {
            var candidat = _context.Candidats
                .FirstOrDefault(c => c.CandidatId == id);

            if (candidat != null)
            {
                _context.Candidats.Remove(candidat);
                _context.SaveChanges();
            }
        }
    }
}
