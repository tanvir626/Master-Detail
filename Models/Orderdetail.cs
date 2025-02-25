using System;
using System.Collections.Generic;

namespace Master_Detail.Models;

public partial class Orderdetail
{
    public int orderdetailid { get; set; }

    public int? orderid { get; set; }

    public string? ProductName { get; set; }

    public int? Quantity { get; set; }

    public int? UnitPrice { get; set; }

    public int? Amount { get; set; }

    public virtual Ordertable? Order { get; set; }
}
