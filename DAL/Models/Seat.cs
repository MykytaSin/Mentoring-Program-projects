using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Seat
{
    public int Seatid { get; set; }

    public int Rownumber { get; set; }

    public int Seatnumber { get; set; }

    public int Seattypeid { get; set; }

    public int Sectionid { get; set; }

    public virtual Seattype Seattype { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
