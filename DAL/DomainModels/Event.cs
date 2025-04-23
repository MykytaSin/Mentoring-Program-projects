using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("events")]
public partial class Event
{
    [Key]
    [Column("eventid")]
    public int Eventid { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("eventdatetime", TypeName = "timestamp without time zone")]
    public DateTime Eventdatetime { get; set; }

    [Column("venueid")]
    public int Venueid { get; set; }

    [Column("manifestid")]
    public int Manifestid { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("eventstatusid")]
    public int Eventstatusid { get; set; }

    [ForeignKey("Eventstatusid")]
    [InverseProperty("Events")]
    public virtual Eventstatus Eventstatus { get; set; } = null!;

    [ForeignKey("Manifestid")]
    [InverseProperty("Events")]
    public virtual Manifest Manifest { get; set; } = null!;

    [InverseProperty("Event")]
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    [InverseProperty("Event")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [ForeignKey("Venueid")]
    [InverseProperty("Events")]
    public virtual Venue Venue { get; set; } = null!;
}
