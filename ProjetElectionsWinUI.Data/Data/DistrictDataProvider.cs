using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;

namespace ProjetElectionsWinUI.Data.Data
{
    public class DistrictDataProvider
    {
        private readonly ElectionsContext _context;

        public DistrictDataProvider(ElectionsContext context)
        {
            _context = context;
        }

        // ================================
        // 1. GET ALL DISTRICTS
        // ================================
        public List<DistrictElectoral> GetAll()
        {
            return _context.Districts
                .OrderBy(d => d.NomDistrict)
                .ToList();
        }

        // ================================
        // 2. GET BY ID
        // ================================
        public DistrictElectoral? GetById(int id)
        {
            return _context.Districts
                .Include(d => d.Candidats)
                .Include(d => d.Electeurs)
                .FirstOrDefault(d => d.DistrictElectoralId == id);
        }

        // ================================
        // 3. ADD
        // ================================
        public void Add(DistrictElectoral district)
        {
            _context.Districts.Add(district);
            _context.SaveChanges();
        }

        // ================================
        // 4. UPDATE
        // ================================
        public void Update(DistrictElectoral district)
        {
            _context.Districts.Update(district);
            _context.SaveChanges();
        }

        // ================================
        // 5. DELETE
        // ================================
        public void Delete(int id)
        {
            var district = _context.Districts.FirstOrDefault(d => d.DistrictElectoralId == id);

            if (district != null)
            {
                _context.Districts.Remove(district);
                _context.SaveChanges();
            }
        }

        // ================================
        // 6. GET CANDIDATS BY DISTRICT
        // ================================
        public List<Candidat> GetCandidatsByDistrict(int districtId)
        {
            return _context.Candidats
                .Where(c => c.DistrictElectoralId == districtId)
                .OrderByDescending(c => c.VotesObtenus)
                .ToList();
        }

        // ================================
        // 7. GET GAGNANT (LINQ)
        // ================================
        public Candidat? GetGagnant(int districtId)
        {
            return _context.Candidats
                .Where(c => c.DistrictElectoralId == districtId)
                .OrderByDescending(c => c.VotesObtenus)
                .FirstOrDefault();
        }
    }
}
