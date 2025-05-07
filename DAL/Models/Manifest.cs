using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Manifest
{
    public int Manifestid { get; set; }

    public int Venueid { get; set; }

    public int Manifesttypeid { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual Manifesttype Manifesttype { get; set; } = null!;

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual Venue Venue { get; set; } = null!;
}
