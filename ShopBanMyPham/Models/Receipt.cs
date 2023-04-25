using System;
using System.Collections.Generic;

namespace ShopBanMyPham.Models;

public partial class Receipt
{
    public int ReceiptId { get; set; }

    public int EmployeeId { get; set; }

    public int ProviderId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? TotalMoney { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
}
