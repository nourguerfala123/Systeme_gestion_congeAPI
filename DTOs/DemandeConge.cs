using gestion_congeAPI.Models;
using System.ComponentModel.DataAnnotations;
namespace gestion_congeAPI.DTOs
{
    public class DemandeConge
    {
        public int IdConge { get; set; }
        public int IdEmploye { get; set; }
        public string NomEmploye { get; set; }   
        public string Type { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly DateFin { get; set; }
        public int NbrJours { get; set; }
        public string Libelle { get; set; }
        public string Statut { get; set; }
    }
}
