using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Payment
{
    public Guid Paymentid { get; set; }

    public int Paymentstatusid { get; set; }

    public DateOnly Creationdate { get; set; }

    public DateOnly? Lastupdated { get; set; }

    public Guid Purchaseid { get; set; }

    public virtual Paymentstatus Paymentstatus { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
