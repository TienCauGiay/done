using System;
using System.Collections.Generic;

namespace ShopBanMyPham.Models;

public partial class Feedback
{
    public int FeedBack1 { get; set; }

    public int UserId { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}
