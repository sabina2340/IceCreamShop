using IceCreamShop.Models;
using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IceCreamShop.Controllers
{
    public class IcecreamModelController : Controller
    {
        private readonly IceCreamShopContext _context;

        public IcecreamModelController(IceCreamShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var icecreamModels = _context.IcecreamModels.ToList();
            return View(icecreamModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IcecreamModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.IcecreamModels.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var icecreamModel = _context.IcecreamModels.Find(id);
            if (icecreamModel == null)
            {
                return NotFound();
            }
            return View(icecreamModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IcecreamModel icecreamModel)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(icecreamModel).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(icecreamModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var icecreamModel = _context.IcecreamModels.Find(id);
            if (icecreamModel == null)
            {
                return NotFound();
            }
            return View(icecreamModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var icecreamModel = _context.IcecreamModels.Find(id);
            if (icecreamModel == null)
            {
                return NotFound();
            }
            _context.IcecreamModels.Remove(icecreamModel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var icecreamModel = _context.IcecreamModels.Find(id);
            return View(icecreamModel);
        }

        public IActionResult DownloadIcecreamModelData()
        {
            var icecreamModels = _context.IcecreamModels.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Model ID\tModel Name\tBarcode\tDate of Manufacture\tExpiration Date\tProduct ID\tSale ID");

            foreach (var model in icecreamModels)
            {
                sb.AppendLine($"{model.ModelId}\t{model.ModelName}\t{model.Barcode}\t{model.DateOfManufacture}\t{model.ExpirationDate}\t{model.ProductId}\t{model.SaleId}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = "IcecreamModelData.txt";

            return File(bytes, "text/plain", fileName);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var icecreamModel = _context.IcecreamModels.Find(id);
        //    if (icecreamModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(icecreamModel);
        //}

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


