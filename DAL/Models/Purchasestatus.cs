using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Purchasestatus
{
    public int Purchasestatusid { get; set; }

    public string Purchasestatusname { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
