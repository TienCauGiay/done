using System;
using System.Collections.Generic;

namespace ShopBanMyPham.Models;

public partial class User
{
    public int UserId { get; set; }

    public int TypeUserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual TypeUser TypeUser { get; set; } = null!;
}
