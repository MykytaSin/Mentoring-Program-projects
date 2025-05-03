using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Purchase
{
    public int Userid { get; set; }

    public DateTime? Purchasedatetime { get; set; }

    public decimal Totalprice { get; set; }

    public int Purchasestatusid { get; set; }

    public Guid Purchaseid { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Purchasestatus Purchasestatus { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User User { get; set; } = null!;
}
