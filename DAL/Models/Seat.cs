using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("seats")]
public partial class Seat
{
    [Key]
    [Column("seatid")]
    public int Seatid { get; set; }

    [Column("rownumber")]
    public int Rownumber { get; set; }

    [Column("seatnumber")]
    public int Seatnumber { get; set; }

    [Column("seattypeid")]
    public int Seattypeid { get; set; }

    [Column("sectionid")]
    public int Sectionid { get; set; }

    [ForeignKey("Seattypeid")]
    [InverseProperty("Seats")]
    public virtual Seattype Seattype { get; set; } = null!;

    [ForeignKey("Sectionid")]
    [InverseProperty("Seats")]
    public virtual Section Section { get; set; } = null!;

    [InverseProperty("Seat")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
