using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("tickets")]
[Index("Ticketcode", Name = "tickets_ticketcode_key", IsUnique = true)]
[Index("Eventid", "Seatid", Name = "uq_event_seat", IsUnique = true)]
public partial class Ticket
{
    [Key]
    [Column("ticketid")]
    public int Ticketid { get; set; }

    [Column("purchaseid")]
    public int? Purchaseid { get; set; }

    [Column("eventid")]
    public int Eventid { get; set; }

    [Column("seatid")]
    public int Seatid { get; set; }

    [Column("offerpriceid")]
    public int Offerpriceid { get; set; }

    [Column("ticketcode")]
    [StringLength(100)]
    public string Ticketcode { get; set; } = null!;

    [Column("createddatetime", TypeName = "timestamp without time zone")]
    public DateTime? Createddatetime { get; set; }

    [Column("ticketstatusid")]
    public int Ticketstatusid { get; set; }

    [ForeignKey("Eventid")]
    [InverseProperty("Tickets")]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("Offerpriceid")]
    [InverseProperty("Tickets")]
    public virtual Offerprice Offerprice { get; set; } = null!;

    [ForeignKey("Purchaseid")]
    [InverseProperty("Tickets")]
    public virtual Purchase? Purchase { get; set; }

    [ForeignKey("Seatid")]
    [InverseProperty("Tickets")]
    public virtual Seat Seat { get; set; } = null!;

    [ForeignKey("Ticketstatusid")]
    [InverseProperty("Tickets")]
    public virtual Ticketstatus Ticketstatus { get; set; } = null!;
}
