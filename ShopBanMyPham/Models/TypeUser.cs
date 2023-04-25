using System;
using System.Collections.Generic;

namespace ShopBanMyPham.Models;

public partial class TypeUser
{
    public int TypeUserId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
