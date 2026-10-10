using gestion_congeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using gestion_congeAPI.DTOs;

namespace gestion_congeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongesController : ControllerBase
    {
        private readonly SystemeCongeContext _context;

        public CongesController(SystemeCongeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetConges()
        {
            DateOnly dateParDefaut = default; 
            var conges = await _context.Congees
                .Include(c => c.IdEmployeNavigation)
                    .ThenInclude(e => e.IdEmployeNavigation)
                .Select(c => new DemandeConge
                {
                    IdConge = c.IdConge,
                    IdEmploye = c.IdEmploye ?? 0,
                    NomEmploye = c.IdEmployeNavigation.IdEmployeNavigation.Nom,
                    Type = c.Type,
                    DateDebut = c.DateDebut.HasValue
                        ? DateOnly.FromDateTime(c.DateDebut.Value)
                        : dateParDefaut,
                    DateFin = c.DateFin.HasValue
                        ? DateOnly.FromDateTime(c.DateFin.Value)
                        : dateParDefaut,
                    NbrJours = c.NbrJours ?? 0,
                    Libelle = c.Libelle,
                    Statut = c.Statut
                })
                .ToListAsync();

            return Ok(conges);
        }
        [HttpPut("{id}/statut")]
        public async Task<IActionResult> PutStatut(int id, [FromBody] modifierStatut dto)
        {
            // Vérification du statut
            var statutsAutorises = new[] { "En attente", "Accepté", "Refusé" };

            if (!statutsAutorises.Contains(dto.statut))
            {
                return BadRequest("Statut invalide. Valeurs autorisées : En attente, Accepté, Refusé.");
            }

            // Recherche de la demande de congé
            var conge = await _context.Congees.FindAsync(id);

            if (conge == null)
            {
                return NotFound($"Aucune demande de congé trouvée avec l'id {id}.");
            }

            // Vérifier si la demande est déjà traitée
            if (conge.Statut != "En attente")
            {
                return Conflict($"Cette demande a déjà été traitée (statut actuel : {conge.Statut}). Modification refusée.");
            }

            // Modifier le statut
            conge.Statut = dto.statut;

            await _context.SaveChangesAsync();

            return Ok(conge);
        }
        //***************************************

        [HttpPost("CreationConge")]
       // [HttpPost]
        public async Task<ActionResult<DemandeConge>> CreerDemandeConge(CreationConge dto)
        {
            // 1. Vérifier que l'employé existe
            var employe = await _context.Emps.FindAsync(dto.IdEmploye);
            if (employe == null)
            {
                return NotFound(new { message = "Employé introuvable." });
            }

            // 2. Valider les dates (DateOnly se compare directement, pas besoin de .Date)
            if (dto.DateFin < dto.DateDebut)
            {
                return BadRequest(new { message = "La date de fin doit être postérieure à la date de début." });
            }

            if (dto.DateDebut < DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest(new { message = "La date de début ne peut pas être dans le passé." });
            }

            // 3. Convertir les DateOnly du DTO en DateTime pour comparer avec l'entité (DateTime?)
            DateTime dateDebutDt = dto.DateDebut.ToDateTime(TimeOnly.MinValue);
            DateTime dateFinDt = dto.DateFin.ToDateTime(TimeOnly.MinValue);

            // 4. Vérifier les conflits (chevauchement de dates)
            bool conflit = await _context.Congees.AnyAsync(c =>
                c.IdEmploye == dto.IdEmploye &&
                c.Statut != "refuse" &&
                c.DateDebut <= dateFinDt &&
                c.DateFin >= dateDebutDt);

            if (conflit)
            {
                return BadRequest(new { message = "Une demande de congé existe déjà sur cette période." });
            }

            // 5. Calculer le nombre de jours 
            int nbrJours = dto.DateFin.DayNumber - dto.DateDebut.DayNumber + 1;

       
            var conge = new Congee
            {
                IdEmploye = dto.IdEmploye,
                DateDebut = dateDebutDt,
                DateFin = dateFinDt,
                NbrJours = nbrJours,
                Libelle = dto.Libelle,
                Type = dto.Type,
                Statut = "En attente"
            };

            _context.Congees.Add(conge);
            await _context.SaveChangesAsync();

            var response = new DemandeConge
            {
                IdConge = conge.IdConge,
                IdEmploye = conge.IdEmploye ?? 0,
                DateDebut = DateOnly.FromDateTime(conge.DateDebut ?? DateTime.MinValue),
                DateFin = DateOnly.FromDateTime(conge.DateFin ?? DateTime.MinValue),
                NbrJours = conge.NbrJours ?? 0,
                Libelle = conge.Libelle,
                Statut = conge.Statut,
                Type = conge.Type
            };

            return CreatedAtAction(nameof(GetConge), new { id = conge.IdConge }, response);

        }

        [HttpGet("{idEmploye}")]
        public async Task<ActionResult<DemandeConge>> GetConge(int idEmploye)
        {
            var conge = await _context.Congees
                .Where(c => c.IdEmploye == idEmploye)
                .OrderByDescending(c => c.DateDebut)
                .FirstOrDefaultAsync();

            if (conge == null) return NotFound();

            return new DemandeConge
            {
                IdConge = conge.IdConge,
                IdEmploye = conge.IdEmploye ?? 0,
                DateDebut = DateOnly.FromDateTime(conge.DateDebut ?? DateTime.MinValue),
                DateFin = DateOnly.FromDateTime(conge.DateFin ?? DateTime.MinValue),
                NbrJours = conge.NbrJours ?? 0,
                Libelle = conge.Libelle,
                Statut = conge.Statut,
                Type = conge.Type
            };
        }
        



       
    }
        
    }
