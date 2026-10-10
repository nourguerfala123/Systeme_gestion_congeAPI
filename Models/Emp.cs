using System;
using System.Collections.Generic;

namespace gestion_congeAPI.Models;

public partial class Emp
{
    public int IdEmploye { get; set; }

    public string? Matricule { get; set; }

    public string? Poste { get; set; }

    public decimal? Salaire { get; set; }

    public DateTime? DateEmbauche { get; set; }

    public int? IdDept { get; set; }

    public virtual ICollection<Congee> Congees { get; } = new List<Congee>();

    public virtual Dept? IdDeptNavigation { get; set; }

    public virtual User IdEmployeNavigation { get; set; } = null!;
}
