using System;
using System.Collections.Generic;

namespace ShopBanMyPham.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? OrderStatus { get; set; }

    public int? TotalMoney { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
