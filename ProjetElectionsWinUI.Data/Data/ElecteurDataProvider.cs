using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;

namespace ProjetElectionsWinUI.Data.Data
{
    public class ElecteurDataProvider
    {
        private readonly ElectionsContext _context;

        public ElecteurDataProvider(ElectionsContext context)
        {
            _context = context;
        }

        // ======================================================
        // 1. GET ALL ELECTEURS (avec District)
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
