using System;
using System.Collections.Generic;

namespace gestion_congeAPI.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? Nom { get; set; }

    public string? Prenom { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public virtual Compte? Compte { get; set; }

    public virtual Emp? Emp { get; set; }
}
