using System;
using System.Collections.Generic;

namespace IceCreamShop.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public DateOnly? SaleDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<IcecreamModel> IcecreamModels { get; set; } = new List<IcecreamModel>();
}
