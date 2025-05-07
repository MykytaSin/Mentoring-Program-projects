using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Seattype
{
    public int Seattypeid { get; set; }

    public string Seattypename { get; set; } = null!;

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
