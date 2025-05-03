using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Paymentstatus
{
    public int Statusid { get; set; }

    public string? Statusname { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
