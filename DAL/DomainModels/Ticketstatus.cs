using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("ticketstatuses")]
[Index("Ticketstatusname", Name = "ticketstatuses_ticketstatusname_key", IsUnique = true)]
public partial class Ticketstatus
{
    [Key]
    [Column("ticketstatusid")]
    public int Ticketstatusid { get; set; }

    [Column("ticketstatusname")]
    [StringLength(50)]
    public string Ticketstatusname { get; set; } = null!;

    [InverseProperty("Ticketstatus")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
