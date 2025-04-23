using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("venues")]
public partial class Venue
{
    [Key]
    [Column("venueid")]
    public int Venueid { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("capacity")]
    public int? Capacity { get; set; }

    [InverseProperty("Venue")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    [InverseProperty("Venue")]
    public virtual ICollection<Manifest> Manifests { get; set; } = new List<Manifest>();
}
