

using System.ComponentModel.DataAnnotations;
namespace gestion_congeAPI.DOTs
{
    public class Creation

    {
        [Required]
        public String nom { get; set; } = null!;
        [Required]
        public String prenom { get; set; } = null!;

        [Required]
        [EmailAddress]
        public String email { get; set; } = null!;
        [Required]
        public String role { get; set; } = null!;

        [Required]
        public String matricule { get; set; } = null!;

        [Required]
        public String poste
        { get; set; } = null!;

        [Required]
        public int id_dept { get; set; }

        [Required]
        public Decimal salaire { get; set; }

        public DateTime dateEmpauche { get; set; }
        [Required]
        [MinLength(6)]
        public string MotDePasse { get; set; } = null!;

    }
}
