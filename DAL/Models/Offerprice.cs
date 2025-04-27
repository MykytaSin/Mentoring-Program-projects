using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("offerprices")]
[Index("Offerid", "Pricelevelid", Name = "uq_offer_pricelevel", IsUnique = true)]
public partial class Offerprice
{
    [Key]
    [Column("offerpriceid")]
    public int Offerpriceid { get; set; }

    [Column("offerid")]
    public int Offerid { get; set; }

    [Column("pricelevelid")]
    public int Pricelevelid { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [ForeignKey("Offerid")]
    [InverseProperty("Offerprices")]
    public virtual Offer Offer { get; set; } = null!;

    [ForeignKey("Pricelevelid")]
    [InverseProperty("Offerprices")]
    public virtual Pricelevel Pricelevel { get; set; } = null!;

    [InverseProperty("Offerprice")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
