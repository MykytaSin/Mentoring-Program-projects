using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("pricelevels")]
[Index("Levelname", Name = "pricelevels_levelname_key", IsUnique = true)]
public partial class Pricelevel
{
    [Key]
    [Column("pricelevelid")]
    public int Pricelevelid { get; set; }

    [Column("levelname")]
    [StringLength(50)]
    public string Levelname { get; set; } = null!;

    [InverseProperty("Pricelevel")]
    public virtual ICollection<Offerprice> Offerprices { get; set; } = new List<Offerprice>();
}
