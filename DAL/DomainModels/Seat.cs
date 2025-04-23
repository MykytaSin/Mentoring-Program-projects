using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("seats")]
[Index("Manifestid", "Sectionname", "Rownumber", "Seatnumber", Name = "uq_seat_location", IsUnique = true)]
public partial class Seat
{
    [Key]
    [Column("seatid")]
    public int Seatid { get; set; }

    [Column("manifestid")]
    public int Manifestid { get; set; }

    [Column("sectionname")]
    [StringLength(50)]
    public string? Sectionname { get; set; }

    [Column("rownumber")]
    public int Rownumber { get; set; }

    [Column("seatnumber")]
    public int Seatnumber { get; set; }

    [Column("seattypeid")]
    public int Seattypeid { get; set; }

    [ForeignKey("Manifestid")]
    [InverseProperty("Seats")]
    public virtual Manifest Manifest { get; set; } = null!;

    [ForeignKey("Seattypeid")]
    [InverseProperty("Seats")]
    public virtual Seattype Seattype { get; set; } = null!;

    [InverseProperty("Seat")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
