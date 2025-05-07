using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Usersrole
{
    public int Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
