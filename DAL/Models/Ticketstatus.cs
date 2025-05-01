using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Ticketstatus
{
    public int Ticketstatusid { get; set; }

    public string Ticketstatusname { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
