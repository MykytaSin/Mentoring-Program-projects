using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("Section")]
public partial class Section
{
    [Key]
    [Column("sectionid")]
    public int Sectionid { get; set; }

    [Column("manifestid")]
    public int Manifestid { get; set; }

    [Column("sectionname")]
    [StringLength(50)]
    public string Sectionname { get; set; } = null!;

    [ForeignKey("Manifestid")]
    [InverseProperty("Sections")]
    public virtual Manifest Manifest { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
