using System;
using System.Collections.Generic;

namespace gestion_congeAPI.Models;

public partial class Compte
{
    public int IdCompte { get; set; }

    public int IdUser { get; set; }

    public string Email { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Statut { get; set; } = null!;

    public DateTime DateCreation { get; set; }

    public DateTime? DerniereConnexion { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
