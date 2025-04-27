using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("manifests")]
public partial class Manifest
{
    [Key]
    [Column("manifestid")]
    public int Manifestid { get; set; }

    [Column("venueid")]
    public int Venueid { get; set; }

    [Column("manifesttypeid")]
    public int Manifesttypeid { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("Manifest")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    [ForeignKey("Manifesttypeid")]
    [InverseProperty("Manifests")]
    public virtual Manifesttype Manifesttype { get; set; } = null!;

    [InverseProperty("Manifest")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    [ForeignKey("Venueid")]
    [InverseProperty("Manifests")]
    public virtual Venue Venue { get; set; } = null!;
}
