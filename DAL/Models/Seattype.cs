using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("seattypes")]
[Index("Seattypename", Name = "seattypes_seattypename_key", IsUnique = true)]
public partial class Seattype
{
    [Key]
    [Column("seattypeid")]
    public int Seattypeid { get; set; }

    [Column("seattypename")]
    [StringLength(100)]
    public string Seattypename { get; set; } = null!;

    [InverseProperty("Seattype")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
