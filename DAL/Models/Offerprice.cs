using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Offerprice
{
    public int Offerpriceid { get; set; }

    public int Pricelevelid { get; set; }

    public decimal Price { get; set; }

    public int Offerid { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual Pricelevel Pricelevel { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
