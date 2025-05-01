using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Event
{
    public int Eventid { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Eventdatetime { get; set; }

    public int Venueid { get; set; }

    public int Manifestid { get; set; }

    public string? Description { get; set; }

    public int Eventstatusid { get; set; }

    public virtual Eventstatus Eventstatus { get; set; } = null!;

    public virtual Manifest Manifest { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Venue Venue { get; set; } = null!;
}
