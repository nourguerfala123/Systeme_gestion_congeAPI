using System;
using System.Collections.Generic;

namespace gestion_congeAPI.Models;

public partial class Congee
{
    public int IdConge { get; set; }

    public int? IdEmploye { get; set; }

    public DateTime? DateDebut { get; set; }

    public DateTime? DateFin { get; set; }

    public int? NbrJours { get; set; }

    public string? Libelle { get; set; }

    public string? Statut { get; set; }

    public string? Type { get; set; }

    public virtual Emp? IdEmployeNavigation { get; set; }
}
