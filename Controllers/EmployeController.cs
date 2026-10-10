using gestion_congeAPI.DOTs;
using gestion_congeAPI.DTOs;
using gestion_congeAPI.Models;
using gestion_congeAPI.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionCongeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly SystemeCongeContext _context;

        public EmployeesController(SystemeCongeContext context)
        {
            _context = context;
        }
        //get all employees with satatut of compte

        // GET api/employees
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employes = await _context.Emps
                .Include(e => e.IdEmployeNavigation)
                .Include(e => e.IdDeptNavigation)
                .Join(
                    _context.Comptes,
                    emp => emp.IdEmploye,
                    compte => compte.IdUser,
                    (emp, compte) => new
                    {
                        emp.IdEmploye,
                        Nom = emp.IdEmployeNavigation.Nom,
                        Prenom = emp.IdEmployeNavigation.Prenom,
                        Email = emp.IdEmployeNavigation.Email,
                        Role = emp.IdEmployeNavigation.Role,
                        emp.Matricule,
                        emp.Poste,
                        emp.Salaire,
                        emp.DateEmbauche,
                        emp.IdDept,
                        DeptNom = emp.IdDeptNavigation.NomDept,
                        Statut = compte.Statut   
                    })
                .ToListAsync();

            return Ok(employes);
        }




        // GET api/employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employe = await _context.Emps
                .Include(e => e.IdEmployeNavigation)
                .Include(e => e.IdDeptNavigation)
                .Where(e => e.IdEmploye == id)
                .Select(e => new
                {
                    e.IdEmploye,
                    Nom = e.IdEmployeNavigation.Nom,
                    Prenom = e.IdEmployeNavigation.Prenom,
                    Email = e.IdEmployeNavigation.Email,
                    Role = e.IdEmployeNavigation.Role,
                    e.Matricule,
                    e.Poste,
                    e.Salaire,
                    e.DateEmbauche,
                    e.IdDept,
                    DeptNom = e.IdDeptNavigation.NomDept
                })
                .FirstOrDefaultAsync();

            if (employe == null)
                return NotFound(new { Erreur = "Employé introuvable." });

            return Ok(employe);
        }
        // PUT api/employees
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Modifier dto)
        {
            var employee = await _context.Emps
                .Include(e => e.IdEmployeNavigation)
                .FirstOrDefaultAsync(e => e.IdEmploye == id);

            if (employee == null)
                return NotFound(new { erreur = $"Employee with id {id} not found." });

            if (employee.IdEmployeNavigation == null)
                return BadRequest(new { erreur = $"Aucun utilisateur lié trouvé pour l'employé {id}." });

            var deptExists = await _context.Depts.AnyAsync(d => d.IdDept == dto.IdDept);
            if (!deptExists)
                return BadRequest(new { erreur = $"Le département avec id_dept={dto.IdDept} n'existe pas." });

            employee.IdEmployeNavigation.Nom = dto.nom;
            employee.IdEmployeNavigation.Prenom = dto.prenom;
            employee.IdEmployeNavigation.Email = dto.email;
            employee.Poste = dto.poste;
            employee.Salaire = dto.salaire;
            employee.IdDept = dto.IdDept;
            employee.Matricule = dto.matricule;        
            employee.DateEmbauche = dto.dateEmbauche;

            try
            {
                await _context.SaveChangesAsync();

                // On renvoie un DTO propre, pas l'entité EF avec ses navigations
                var result = new Modifier
                {
                    IdEmploye = employee.IdEmploye,
                    nom = employee.IdEmployeNavigation.Nom,
                    prenom = employee.IdEmployeNavigation.Prenom,
                    email = employee.IdEmployeNavigation.Email,
                    poste = employee.Poste,
                    salaire = employee.Salaire ?? 0,      
                    IdDept = employee.IdDept ?? 0
                };

                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { erreur = inner });
            }
        }

        //get dept 
        // GET api/employees/depts
        [HttpGet("depts")]
        public async Task<IActionResult> GetDepts()
        {
            var depts = await _context.Depts
                .Select(d => new
                {
                    d.IdDept,
                    d.NomDept
                })
                .ToListAsync();

            return Ok(depts);
        }






        // DELETE api/employees/with satut
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _context.Emps.FindAsync(id);
            if (emp == null)
                return NotFound(new { Erreur = "Employé introuvable." });

            var compte = await _context.Comptes.FirstOrDefaultAsync(c => c.IdUser == id);
            if (compte == null)
                return NotFound(new { Erreur = "Compte introuvable pour cet employé." });

            compte.Statut = "inactif";

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Employé désactivé avec succès." });
        }
    }
}