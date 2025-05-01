using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Ticket
{
    public int Ticketid { get; set; }

    public int Eventid { get; set; }

    public int Seatid { get; set; }

    public int Offerpriceid { get; set; }

    public string Ticketcode { get; set; } = null!;

    public DateTime? Createddatetime { get; set; }

    public int Ticketstatusid { get; set; }

    public Guid? Purchaseid { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Offerprice Offerprice { get; set; } = null!;

    public virtual Purchase? Purchase { get; set; }

    public virtual Seat Seat { get; set; } = null!;

    public virtual Ticketstatus Ticketstatus { get; set; } = null!;
}
