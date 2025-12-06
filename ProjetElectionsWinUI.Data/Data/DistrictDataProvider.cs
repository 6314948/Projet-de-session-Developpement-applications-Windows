using Microsoft.EntityFrameworkCore;
using ProjetElectionsWinUi.Data.Models;
using ProjetElectionsWinUI.Data.Models;

namespace ProjetElectionsWinUI.Data.Data
{
    /// <summary>
    /// Classe qui gère l'accès aux données des districts.
    /// On utilise un DataProvider pour séparer EF Core du ViewModel (MVVM).
    /// </summary>
    public class DistrictDataProvider
    {
        private readonly ElectionsContext _context;

        public DistrictDataProvider(ElectionsContext context)
        {
            // Le context est créé dans le ViewModel, donc on le reçoit ici.
            _context = context;
        }

        // ================================================================
        // 1. GET ALL DISTRICTS
        //    Retourne la liste des districts triés par nom.
        // ================================================================
        public List<DistrictElectoral> GetAll()
        {
            return _context.Districts
                .OrderBy(d => d.NomDistrict)
                .ToList();
        }

        // ================================================================
        // 2. GET BY ID (avec Include)
        //    Le Include permet de charger aussi les candidats et électeurs
        //    liés au district.
        // ================================================================
        public DistrictElectoral? GetById(int id)
        {
            return _context.Districts
                .Include(d => d.Candidats)
                .Include(d => d.Electeurs)
                .FirstOrDefault(d => d.DistrictElectoralId == id);
        }

        // ================================================================
        // 3. ADD
        //    Ajoute un nouveau district dans la base.
        // ================================================================
        public void Add(DistrictElectoral district)
        {
            _context.Districts.Add(district);
            _context.SaveChanges();
        }

        // ================================================================
        // 4. UPDATE
        //    Modifie un district existant (EF Core suit l'objet automatiquement).
        // ================================================================
        public void Update(DistrictElectoral district)
        {
            _context.Districts.Update(district);
            _context.SaveChanges();
        }

        // ================================================================
        // 5. DELETE
        //    Supprime un district en fonction de son ID.
        // ================================================================
        public void Delete(int id)
        {
            var district = _context.Districts.FirstOrDefault(d => d.DistrictElectoralId == id);

            if (district != null)
            {
                _context.Districts.Remove(district);
                _context.SaveChanges();
            }
        }

        // ================================================================
        // 6. GET CANDIDATS BY DISTRICT
        //    Retourne tous les candidats d'un district donné.
        // ================================================================
        public List<Candidat> GetCandidatsByDistrict(int districtId)
        {
            return _context.Candidats
                .Where(c => c.DistrictElectoralId == districtId)
                .OrderByDescending(c => c.VotesObtenus) // Tri par nombre de votes
                .ToList();
        }

        // ================================================================
        // 7. GET GAGNANT
        //    Retourne le candidat avec le plus de votes.
        //    (Logique LINQ assez simple, mais j'ai vérifié avec ChatGPT
        //    pour être sûr que OrderByDescending + FirstOrDefault était
        //    la façon correcte de faire.)
        // ================================================================
        public Candidat? GetGagnant(int districtId)
        {
            return _context.Candidats
                .Where(c => c.DistrictElectoralId == districtId)
                .OrderByDescending(c => c.VotesObtenus)
                .FirstOrDefault();
        }
    }
}
