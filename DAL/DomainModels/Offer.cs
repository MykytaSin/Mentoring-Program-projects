using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("offers")]
public partial class Offer
{
    [Key]
    [Column("offerid")]
    public int Offerid { get; set; }

    [Column("eventid")]
    public int Eventid { get; set; }

    [Column("offername")]
    [StringLength(100)]
    public string Offername { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("validfrom", TypeName = "timestamp without time zone")]
    public DateTime? Validfrom { get; set; }

    [Column("validto", TypeName = "timestamp without time zone")]
    public DateTime? Validto { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [ForeignKey("Eventid")]
    [InverseProperty("Offers")]
    public virtual Event Event { get; set; } = null!;

    [InverseProperty("Offer")]
    public virtual ICollection<Offerprice> Offerprices { get; set; } = new List<Offerprice>();
}
