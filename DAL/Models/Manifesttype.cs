using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("manifesttypes")]
[Index("Manifesttypename", Name = "manifesttypes_manifesttypename_key", IsUnique = true)]
public partial class Manifesttype
{
    [Key]
    [Column("manifesttypeid")]
    public int Manifesttypeid { get; set; }

    [Column("manifesttypename")]
    [StringLength(100)]
    public string Manifesttypename { get; set; } = null!;

    [InverseProperty("Manifesttype")]
    public virtual ICollection<Manifest> Manifests { get; set; } = new List<Manifest>();
}
