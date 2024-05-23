using System;
using System.Collections.Generic;

namespace IceCreamShop.Models;

public partial class IcecreamModel
{
    public int ModelId { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public DateOnly DateOfManufacture { get; set; }

    public string? ModelName { get; set; }

    public int Barcode { get; set; }

    public int? ProductId { get; set; }

    public int? SaleId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Sale? Sale { get; set; }
}
