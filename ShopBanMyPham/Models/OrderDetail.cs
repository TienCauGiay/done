using System;
using System.Collections.Generic;

namespace ShopBanMyPham.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? SellNumber { get; set; }

    public int? ShippingCost { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
