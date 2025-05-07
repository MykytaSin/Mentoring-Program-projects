using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Manifesttype
{
    public int Manifesttypeid { get; set; }

    public string Manifesttypename { get; set; } = null!;

    public virtual ICollection<Manifest> Manifests { get; set; } = new List<Manifest>();
}
