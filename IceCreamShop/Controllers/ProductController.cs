using IceCreamShop.Models;
using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IceCreamShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IceCreamShopContext _context;
        public ProductController(IceCreamShopContext context)
        {
            _context = context;
        }

        public IActionResult Index(decimal? minPrice, decimal? maxPrice, string manufacturer)
        {
            IQueryable<Product> products = _context.Products;

            // Применяем фильтр по минимальной цене, если задана
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice);
            }

            // Применяем фильтр по максимальной цене, если задана
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice);
            }

            // Применяем фильтр по производителю, если задан
            if (!string.IsNullOrEmpty(manufacturer))
            {
                products = products.Where(p => p.ProductManufacturer.ToLower() == manufacturer.ToLower());
            }

            // Получаем список отфильтрованных продуктов
            var filteredProducts = products.ToList();

            return View(filteredProducts);
        }

        //public IActionResult Index()
        //{
        //    var products = _context.Products.ToList();
        //    return View(products);
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var product = _context.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}

        public IActionResult DownloadProductData()
        {
            var products = _context.Products.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Product ID,Product Type,Product Price,Product Manufacturer,Product Description,Product Composition,Product Weight");
            foreach (var product in products)
            {
                sb.AppendLine($"{product.ProductId},{product.ProductType},{product.ProductPrice},{product.ProductManufacturer},{product.ProductDescription},{product.ProductComposition},{product.ProductWeight}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", "ProductsList.txt");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

