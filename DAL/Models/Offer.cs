using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Offer
{
    public int Eventid { get; set; }

    public string Offername { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Validfrom { get; set; }

    public DateTime? Validto { get; set; }

    public bool? Isactive { get; set; }

    public int Offerid { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<Offerprice> Offerprices { get; set; } = new List<Offerprice>();
}
