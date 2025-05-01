using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Pricelevel
{
    public int Pricelevelid { get; set; }

    public string Levelname { get; set; } = null!;

    public virtual ICollection<Offerprice> Offerprices { get; set; } = new List<Offerprice>();
}
