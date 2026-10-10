using gestion_congeAPI.DOTs;
using gestion_congeAPI.DTOs;
using gestion_congeAPI.Models;
using gestion_congeAPI.service;
using Microsoft.EntityFrameworkCore;
namespace gestion_congeAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly SystemeCongeContext _context;

        public AuthService(SystemeCongeContext context)
        {
            _context = context;
        }

        public async Task<string> EmpCreation(Creation dto)
        {
            bool emailExiste = await _context.Comptes.AnyAsync(c => c.Email == dto.email);

            if (emailExiste)
            {
                throw new Exception("Un compte avec cet email existe déjà.");
            }

            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new User
                {
                    Nom = dto.nom,
                    Prenom = dto.prenom,
                    Email = dto.email,
                    Role = dto.role
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                Console.WriteLine($"DEBUG id_dept reçu : {dto.id_dept}");
                // verification de l'id_dept s'il existe oui ou non


                var deptExiste = await _context.Depts.FindAsync(dto.id_dept);
                if (deptExiste == null)
                {
                    throw new Exception($"Le département avec l'id {dto.id_dept} n'existe pas.");
                }




                var emp = new Emp
                {
                    IdEmploye = user.IdUser,
                    Matricule = dto.matricule,
                    Poste = dto.poste,
                    Salaire = dto.salaire,
                    DateEmbauche = dto.dateEmpauche,
                    IdDept = dto.id_dept
                };
                _context.Emps.Add(emp);

                string motDePasseHache = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse);

                var compte = new Compte
                {
                    IdUser = user.IdUser,
                    Email = dto.email,
                    MotDePasse = motDePasseHache,
                    Role = dto.role,
                    Statut = "Actif",
                    DateCreation = DateTime.Now
                };
                _context.Comptes.Add(compte);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Compte créé avec succès.";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

       
       
       
    }
    
}

