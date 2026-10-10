using System;
using System.Collections.Generic;

namespace gestion_congeAPI.Models;

public partial class Dept
{
    public int IdDept { get; set; }

    public string? NomDept { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Emp> Emps { get; } = new List<Emp>();
}
