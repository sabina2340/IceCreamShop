using IceCreamShop.Models;
using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;

namespace IceCreamShop.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly IceCreamShopContext _context;

        public EmployeeController(IceCreamShopContext context)
        {
            _context = context;
        }
        public ActionResult Index(string timeOfWork)
        {
            IQueryable<Employee> employees = _context.Employees;

            // Фильтрация по времени работы
            if (!string.IsNullOrEmpty(timeOfWork))
            {
                employees = employees.Where(e => e.TimeEmployees == timeOfWork);
            }

            // Сортировка по алфавиту
            employees = employees.OrderBy(e => e.EName);

            var employeesList = employees.ToList();
            return View(employeesList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(employee).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index"); // Перенаправление на главную страницу или другую страницу
            }
            return View(employee);
        }
        [HttpPost]
        public IActionResult ExportEmployeesToTextFile()
        {
            var employees = _context.Employees.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Employee ID\tName\tTime Employees\tContact Details");
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.EmployeeId}\t{employee.EName}\t{employee.TimeEmployees ?? "-"}\t{employee.ContactDetails ?? "-"}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", "EmployeesList.txt");
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index", "Employee");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _context.Employees.Find(id);
            return View(employee);
        }

    }
}

