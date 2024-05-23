using System;
using System.Collections.Generic;

namespace IceCreamShop.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? StoreAddress { get; set; }

    public string? StorePhone { get; set; }

    public string? StoreTimetable { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
