using IceCreamShop.Models;
using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IceCreamShop.Controllers
{
    public class StoreController : Controller
    {
        private readonly IceCreamShopContext _context;

        public StoreController(IceCreamShopContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    var stores = _context.Stores.ToList();
        //    return View(stores);
        //}

        public IActionResult Index(string timetable)
        {
            // Получаем все магазины
            IQueryable<Store> stores = _context.Stores;

            // Если указано время работы для фильтрации
            if (!string.IsNullOrEmpty(timetable))
            {
                // Фильтруем магазины по времени работы
                stores = stores.Where(s => s.StoreTimetable.Contains(timetable));
            }

            // Преобразуем результат в список и передаем в представление
            return View(stores.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Stores.Add(store);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(store).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            _context.Stores.Remove(store);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var store = _context.Stores.Find(id);
            return View(store);
        }

        //[HttpGet("store/{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var store = _context.Stores.Find(id);
        //    if (store == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(store);
        //}

        public IActionResult DownloadStoreData()
        {
            var stores = _context.Stores.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Store ID,Store Address,Store Phone,Store Timetable");
            foreach (var store in stores)
            {
                sb.AppendLine($"{store.StoreId},{store.StoreAddress},{store.StorePhone},{store.StoreTimetable}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", "StoresList.txt");
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
