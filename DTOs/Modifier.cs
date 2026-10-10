using System.ComponentModel.DataAnnotations;

namespace gestion_congeAPI.DTOs
{
    public class Modifier
    {
        public int IdEmploye { get; set; }
        public string? nom { get; set; }
        public string? prenom { get; set; }
        public string? email { get; set; }
        public string? poste { get; set; }
        public decimal salaire { get; set; }
        public int IdDept { get; set; }
        public string? matricule { get; set; }    
        public DateTime? dateEmbauche { get; set; }
    }
}
