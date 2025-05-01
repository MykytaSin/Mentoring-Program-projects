using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Venue
{
    public int Venueid { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Manifest> Manifests { get; set; } = new List<Manifest>();
}
