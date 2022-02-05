using Microsoft.AspNetCore.Mvc;
using HR_Portal.Data;
using HR_Portal.Models;
using Microsoft.AspNetCore.Authorization;

namespace HR_Portal.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "HR,IT")]
        public IActionResult Index()
        {
            var result = (from d in _db.Departments
                          join u in _db.Users on d.DirectorId equals u.UserId into tab
                          from u in tab.DefaultIfEmpty()
                          select new
                          {
                              u.UserId,
                              u.FirstName,
                              u.LastName,
                              d.DepartmentName,
                              d.DepartmentId
                          })
                         .ToList();
            ViewBag.Directors = result;

            return View();
        }

        [Authorize(Roles = "HR,IT")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var resultId = _db.Departments.Find(id);

            if (resultId == null)
            {
                return NotFound();
            }

            var result = (from u in _db.Users
                          join p in _db.Positions on u.PositionId equals p.PositionId into tab
                          from p in tab.DefaultIfEmpty()
                          select new
                          {
                              u.UserId,
                              u.FirstName,
                              u.LastName,
                              p.PositionName
                          })
                         .ToList();
            ViewBag.Directors = result;


            return View(resultId);
        }

        //POST
        [Authorize(Roles = "HR,IT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department obj)
        {

            if (ModelState.IsValid)
            {
                _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        //GET
        [Authorize(Roles = "HR,IT")]
        public IActionResult Create()
        {
            var result = (from u in _db.Users
                          join p in _db.Positions on u.PositionId equals p.PositionId into tab
                          from p in tab.DefaultIfEmpty()
                          select new
                          {
                              u.UserId,
                              u.FirstName,
                              u.LastName,
                              p.PositionName
                          })
                         .ToList();
            ViewBag.Directors = result;

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR,IT")]
        public IActionResult Create(Department obj)
        {

            if (ModelState.IsValid)
            {
                _db.Departments.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        [Authorize(Roles = "HR,IT")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Departments.Find(id);

            if (obj == null)
                return NotFound();

            var result = (from u in _db.Users
                          join p in _db.Positions on u.PositionId equals p.PositionId into tab
                          from p in tab.DefaultIfEmpty()
                          select new
                          {
                              u.UserId,
                              u.FirstName,
                              u.LastName,
                              p.PositionName
                          })
                         .ToList();
            ViewBag.Directors = result;

            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR,IT")]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Departments.Find(id);
            if (obj == null)
                return NotFound();

            _db.Departments.Remove(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
