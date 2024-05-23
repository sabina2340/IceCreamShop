namespace IceCreamShop.Models
{
    public class SearchViewModel
    {
            public List<Employee> Employees { get; set; }
            public List<IcecreamModel> IcecreamModels { get; set; }
            public List<Product> Products { get; set; }
            public List<Sale> Sales { get; set; }
            public List<Store> Stores { get; set; }
    }
}
