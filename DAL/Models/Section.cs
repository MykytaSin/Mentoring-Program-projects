using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Section
{
    public int Sectionid { get; set; }

    public int Manifestid { get; set; }

    public string Sectionname { get; set; } = null!;

    public virtual Manifest Manifest { get; set; } = null!;

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
