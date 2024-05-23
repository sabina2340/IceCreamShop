using IceCreamShop.Models;
using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IceCreamShop.Controllers
{
    public class SaleController : Controller
    {
        private readonly IceCreamShopContext _context;
        public SaleController(IceCreamShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sales = _context.Sales.ToList();
            return View(sales);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Sales.Add(sale);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var sale = _context.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(sale).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sale = _context.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var sale = _context.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }
            _context.Sales.Remove(sale);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var sale = _context.Sales.Find(id);
            return View(sale);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var sale = _context.Sales.Find(id);
        //    if (sale == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(sale);
        //}
        public IActionResult DownloadSaleData()
        {
            var sales = _context.Sales.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Sale ID,Sale Date,Total Amount,Employee ID");
            foreach (var sale in sales)
            {
                sb.AppendLine($"{sale.SaleId},{sale.SaleDate},{sale.TotalAmount},{sale.EmployeeId}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", "SalesList.txt");
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


