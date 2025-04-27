using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("purchasestatuses")]
[Index("Purchasestatusname", Name = "purchasestatuses_purchasestatusname_key", IsUnique = true)]
public partial class Purchasestatus
{
    [Key]
    [Column("purchasestatusid")]
    public int Purchasestatusid { get; set; }

    [Column("purchasestatusname")]
    [StringLength(50)]
    public string Purchasestatusname { get; set; } = null!;

    [InverseProperty("Purchasestatus")]
    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
