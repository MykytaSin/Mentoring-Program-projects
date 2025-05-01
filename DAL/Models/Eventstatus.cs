using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Eventstatus
{
    public int Eventstatusid { get; set; }

    public string Eventstatusname { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
