using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("eventstatuses")]
[Index("Eventstatusname", Name = "eventstatuses_eventstatusname_key", IsUnique = true)]
public partial class Eventstatus
{
    [Key]
    [Column("eventstatusid")]
    public int Eventstatusid { get; set; }

    [Column("eventstatusname")]
    [StringLength(100)]
    public string Eventstatusname { get; set; } = null!;

    [InverseProperty("Eventstatus")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
