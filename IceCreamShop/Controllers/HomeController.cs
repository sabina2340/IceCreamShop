using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IceCreamShop.Models;
using System.Text;

namespace IceCreamShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IceCreamShopContext _context;

        public HomeController(IceCreamShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SalesReportsList()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        public async Task<IActionResult> SalesReport(int employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Sales)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public async Task<IActionResult> DownloadSalesReport(int employeeId)
        {
            // Находим информацию о сотруднике и его продажах
            var employee = await _context.Employees
                .Include(e => e.Sales)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            // Генерируем текстовый отчет о продажах для данного сотрудника
            var sb = new StringBuilder();
            sb.AppendLine($"Sales Report for Employee: {employee.EName}");
            sb.AppendLine("Sale ID, Sale Date, Total Amount");

            foreach (var sale in employee.Sales)
            {
                sb.AppendLine($"{sale.SaleId}, {sale.SaleDate}, {sale.TotalAmount}");
            }

            // Кодируем отчет в массив байтов
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            // Возвращаем файл для скачивания
            return File(bytes, "text/plain", $"SalesReport_Employee_{employeeId}.txt");
        }


        public IActionResult AveragePriceByProductType()
        {
            var averagePrices = _context.Products
                .GroupBy(p => p.ProductType)
                .Select(g => new
                {
                    ProductType = g.Key,
                    AveragePrice = g.Average(p => p.ProductPrice)
                })
                .ToList();

            return View(averagePrices);
        }

        public IActionResult DownloadAveragePriceByProductType()
        {
            var averagePrices = _context.Products
                .GroupBy(p => p.ProductType)
                .Select(g => new
                {
                    ProductType = g.Key,
                    AveragePrice = g.Average(p => p.ProductPrice)
                })
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Product Type,Average Price");

            foreach (var averagePrice in averagePrices)
            {
                sb.AppendLine($"{averagePrice.ProductType},{averagePrice.AveragePrice}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", "AveragePriceByProductType.txt");
        }



        public ActionResult Search(string query)
        {
            //employee
            var employeesByName = _context.Employees.Where(e => e.EName.Contains(query)).ToList();
            var employeesByTime = _context.Employees.Where(e => e.TimeEmployees.Contains(query)).ToList();
            var employeesByContact = _context.Employees.Where(e => e.ContactDetails.Contains(query)).ToList();

            var employees = employeesByName.Concat(employeesByTime).Concat(employeesByContact).Distinct().ToList();


            //icecream
            var ModelName = _context.IcecreamModels.Where(i => i.ModelName.Contains(query)).ToList();

            var icecreamModels = /*expIce.Concat(manufIce).Concat*/(ModelName)/*.Concat(barcodeModels)*/.Distinct().ToList();


            //product
            var ptype = _context.Products.Where(p => p.ProductType.Contains(query)).ToList();
            var dproduct = _context.Products.Where(p => p.ProductManufacturer.Contains(query)).ToList();
            var dproduct1 = _context.Products.Where(p => p.ProductDescription.Contains(query)).ToList();
            var dproduct2 = _context.Products.Where(p => p.ProductComposition.Contains(query)).ToList();

            var products = ptype./*Concat(pprice)*//*.*/Concat(dproduct).Concat(dproduct1).Concat(dproduct2)/*.Concat(dproduct3)*/.Distinct().ToList();

            //store
            var store1 = _context.Stores.Where(st => st.StoreAddress.Contains(query)).ToList();
            var store2 = _context.Stores.Where(st => st.StoreTimetable.Contains(query)).ToList();
            var store3 = _context.Stores.Where(st => st.StorePhone.Contains(query)).ToList();

            var stores = store1.Concat(store2).Concat(store3).Distinct().ToList();

            return View("SearchResults", new SearchViewModel
            {
                Employees = employees,
                IcecreamModels = icecreamModels,
                Products = products,
                Stores = stores
            });



        }
    }
}
