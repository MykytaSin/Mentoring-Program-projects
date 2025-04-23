using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("purchases")]
public partial class Purchase
{
    [Key]
    [Column("purchaseid")]
    public int Purchaseid { get; set; }

    [Column("userid")]
    public int Userid { get; set; }

    [Column("purchasedatetime", TypeName = "timestamp without time zone")]
    public DateTime? Purchasedatetime { get; set; }

    [Column("totalprice")]
    [Precision(10, 2)]
    public decimal Totalprice { get; set; }

    [Column("purchasestatusid")]
    public int Purchasestatusid { get; set; }

    [ForeignKey("Purchasestatusid")]
    [InverseProperty("Purchases")]
    public virtual Purchasestatus Purchasestatus { get; set; } = null!;

    [InverseProperty("Purchase")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [ForeignKey("Userid")]
    [InverseProperty("Purchases")]
    public virtual User User { get; set; } = null!;
}
