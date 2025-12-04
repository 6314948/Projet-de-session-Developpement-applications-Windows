using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;

namespace ProjetElectionsWinUI.Data.Data
{
    public class CandidatDataProvider
    {
        private readonly ElectionsContext _context;

        public CandidatDataProvider(ElectionsContext context)
        {
            _context = context;
        }

        // ======================================================
        // 1. GET ALL CANDIDATS (avec District)
        // ======================================================
        public List<Candidat> GetAll()
        {
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
                .Include(c => c.District)
                .FirstOrDefault(c => c.CandidatId == id);
        }

        // ======================================================
        // 3. GET BY DISTRICT
        // ======================================================
        public List<Candidat> GetByDistrict(int districtId)
        {
            return _context.Candidats
                .Include(c => c.District)
                .Where(c => c.DistrictElectoralId == districtId)
                .OrderByDescending(c => c.VotesObtenus)
                .ToList();
        }

        // ======================================================
        // 4. ADD
        // ======================================================
        public void Add(Candidat candidat)
        {
            _context.Candidats.Add(candidat);
            _context.SaveChanges();
        }

        // ======================================================
        // 5. UPDATE
        // ======================================================
        public void Update(Candidat candidat)
        {
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
