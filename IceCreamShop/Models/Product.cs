using System;
using System.Collections.Generic;

namespace IceCreamShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductType { get; set; } = null!;

    public decimal? ProductPrice { get; set; }

    public string ProductManufacturer { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public string? ProductComposition { get; set; }

    public int? ProductWeight { get; set; }

    public virtual ICollection<IcecreamModel> IcecreamModels { get; set; } = new List<IcecreamModel>();
}
