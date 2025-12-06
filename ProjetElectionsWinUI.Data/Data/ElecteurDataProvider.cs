using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;

namespace ProjetElectionsWinUI.Data.Data
{
    /// <summary>
    /// Classe qui s'occupe de gérer les opérations sur les électeurs.
    /// Le but du provider est d'éviter d'appeler EF Core directement
    /// dans les ViewModels (plus conforme au modèle MVVM).
    /// </summary>
    public class ElecteurDataProvider
    {
        private readonly ElectionsContext _context;

        public ElecteurDataProvider(ElectionsContext context)
        {
            _context = context;
        }

        // ======================================================
        // 1. GET ALL ELECTEURS (avec leur district)
        // ======================================================
        public List<Electeur> GetAll()
        {
            return _context.Electeurs
                .Include(e => e.District)
                .OrderBy(e => e.Nom)
                .ToList();
        }

        // ======================================================
        // 2. GET BY ID
        // ======================================================
        public Electeur? GetById(int id)
        {
            return _context.Electeurs
                .Include(e => e.District)
                .FirstOrDefault(e => e.ElecteurId == id);
        }

        // ======================================================
        // 3. GET BY DISTRICT
        // ======================================================
        public List<Electeur> GetByDistrict(int districtId)
        {
            // Sert surtout si on veut lister les électeurs d’un district précis.
            return _context.Electeurs
                .Include(e => e.District)
                .Where(e => e.DistrictElectoralId == districtId)
                .OrderBy(e => e.Nom)
                .ToList();
        }

        // ======================================================
        // 4. ADD
        // ======================================================
        public void Add(Electeur electeur)
        {
            _context.Electeurs.Add(electeur);
            _context.SaveChanges();
        }

        // ======================================================
        // 5. UPDATE
        // ======================================================
        public void Update(Electeur electeur)
        {
            // EF Core détecte automatiquement que l’objet existe,
            // donc Update suffit pour réappliquer les modifications.
            _context.Electeurs.Update(electeur);
            _context.SaveChanges();
        }

        // ======================================================
        // 6. DELETE
        // ======================================================
        public void Delete(int id)
        {
            var electeur = _context.Electeurs
                .FirstOrDefault(e => e.ElecteurId == id);

            if (electeur != null)
            {
                _context.Electeurs.Remove(electeur);
                _context.SaveChanges();
            }
        }
    }
}
