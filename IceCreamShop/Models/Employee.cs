using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IceCreamShop.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EName { get; set; } = "123";

    public string? TimeEmployees { get; set; }

    public string? ContactDetails { get; set; }

    public int? StoreId { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual Store? Store { get; set; }
}
