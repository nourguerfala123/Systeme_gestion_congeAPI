using gestion_congeAPI.DOTs;
using gestion_congeAPI.Models;
using gestion_congeAPI.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionCongeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SystemeCongeContext _context;
        private readonly IAuthService _authService;

        public AuthController(SystemeCongeContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Connexion dto)

        {

            try
            {
                
                var utilisateur = await _context.Comptes
                    .FirstOrDefaultAsync(c =>
                        c.Email == dto.email
                    );

                if (utilisateur == null || !BCrypt.Net.BCrypt.Verify(dto.password, utilisateur.MotDePasse))
                {
                    return Unauthorized("Email ou mot de passe incorrect");
                }

                //verification du statut
                if (utilisateur.Statut == "inactif")
                {
                    return Unauthorized(new { Erreur = "Ce compte a été désactivé." });
                }

                if (utilisateur.Statut == "bloqué")
                {
                    return Unauthorized(new { Erreur = "Ce compte a été bloqué." });
                }


                //***********************************
                return Ok(new
                {
                    Message = "Connexion réussie",
                    Id = utilisateur.IdCompte,
                    IdEmploye = utilisateur.IdUser,
                    Email = utilisateur.Email,
                    Role = utilisateur.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Erreur = ex.Message
                });
            }
        }

        [HttpPost("empCreation")]
        public async Task<IActionResult> EmpCreation(Creation dto)
        {
            try
            {
                var result = await _authService.EmpCreation(dto);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erreur = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
    }
}