
using System.ComponentModel.DataAnnotations;
namespace gestion_congeAPI.DOTs
{
    public class Connexion
    {
        [Required]
        [EmailAddress]
        public String email { get; set; } = null!;

        [Required]
        public String password { get; set; } = null!;

    }
}
