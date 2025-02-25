using System;
using System.Collections.Generic;

namespace Master_Detail.Models;

public partial class Ordertable
{
    public int Orderid { get; set; }

    public string? Username { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
