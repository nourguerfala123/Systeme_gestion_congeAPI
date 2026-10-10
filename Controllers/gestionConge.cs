
using gestion_congeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestion_congeAPI.DTOs;

namespace gestion_congeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeptController : ControllerBase
    {
        private readonly SystemeCongeContext _context;
        public DeptController(SystemeCongeContext context)
        {
            _context = context;
        }
        //*********************************************
        [HttpGet]
        public async Task<IActionResult> GetAllDept()
        {
            var departements = await _context.Depts
                .Select(d => new
                {
                    idDept = d.IdDept,
                    nomDept = d.NomDept,
                    description = d.Description
                })
                .ToListAsync();

            return Ok(departements);
        }
        //**************************************


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeptById(int id)
        {
            var dept = await _context.Depts
                .Where(d => d.IdDept == id)
                .Select(d => new
                {
                    idDept = d.IdDept,
                    nomDept = d.NomDept,
                    description = d.Description
                })
                .FirstOrDefaultAsync();

            if (dept == null)
                return NotFound(new { erreur = $"Departement avec id {id} introuvable." });

            return Ok(dept);
        }

        //*************************************

        [HttpPost("Creation")]
        public async Task<IActionResult> CreerDept([FromBody] AjouterDept dto)
        {
            if (dto == null)
                return BadRequest("Données invalides.");

            if (string.IsNullOrWhiteSpace(dto.NomDept))
                return BadRequest("Le nom du département est obligatoire.");

            bool existe = await _context.Depts
                .AnyAsync(d => d.NomDept.ToLower() == dto.NomDept.ToLower());

            if (existe)
                return Conflict("Un département avec ce nom existe déjà.");

            var nouveauDept = new Dept
            {
                NomDept = dto.NomDept,
                Description = dto.Description
            };

            _context.Depts.Add(nouveauDept);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Département créé avec succès.",
                idDept = nouveauDept.IdDept
            });
        }

        //***********************
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ModifierDept dto)
        {
            var dept = await _context.Depts
                .FirstOrDefaultAsync(d => d.IdDept == id);

            if (dept == null)
            {
                return NotFound(new { erreur = $"Departement avec id {id} introuvable." });
            }

            dept.NomDept = dto.NomDept;
            dept.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(dept);
        }

        //la suppression de departement 

        [HttpDelete("{id}")]
        public async Task<IActionResult> SupprimerDept(int id)
        {
            var dept = await _context.Depts
                .FirstOrDefaultAsync(d => d.IdDept == id);

            if (dept == null)
                return NotFound(new { erreur = $"Departement avec id {id} introuvable." });

            bool aDesEmployes = await _context.Emps.AnyAsync(e => e.IdDept == id);

            if (aDesEmployes)
                return Conflict(new { erreur = "Impossible de supprimer : des employés sont encore rattachés à ce département." });

            _context.Depts.Remove(dept);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Département supprimé avec succès." });
        }



        //*****************************************



    }
}