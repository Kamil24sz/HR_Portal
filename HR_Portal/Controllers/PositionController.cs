using Microsoft.AspNetCore.Mvc;
using HR_Portal.Data;
using HR_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace HR_Portal.Controllers
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PositionController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "HR")]
        public IActionResult Index()
        {
            var result = (from p in _db.Positions
                          join d in _db.Departments on p.DepartmentId equals d.DepartmentId into tab
                          from d in tab.DefaultIfEmpty()
                          select new
                          {
                              p.PositionId,
                              p.PositionName,
                              p.BaseSalary,
                              d.DepartmentName
                          })
                         .ToList();
            ViewBag.Result = result;

            return View();
        }

        [Authorize(Roles = "HR")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var resultId = _db.Positions.Find(id);

            if (resultId == null)
            {
                return NotFound();
            }

            var result = (from d in _db.Departments
                             select new
                             {
                                 d.DepartmentId,
                                 d.DepartmentName
                             }).ToList();
            ViewBag.Departments = result;

            ViewBag.Result = result;


            return View(resultId);
        }

        //POST
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Position obj)
        {

            if (ModelState.IsValid)
            {
               // _db.Positions.Add(obj);
                _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        //GET
        [Authorize(Roles = "HR")]
        public IActionResult Create()
        {
            var result = (from d in _db.Departments
                          select new
                          {
                              d.DepartmentId,
                              d.DepartmentName
                          })
                         .ToList();
            ViewBag.Result = result;

            return View();
        }

        //POST
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Position obj)
        {

            if (ModelState.IsValid)
            {
                _db.Positions.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        [Authorize(Roles = "HR")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Positions.Find(id);

            if (obj == null)
                return NotFound();

            var result = (from d in _db.Departments
                          select new
                          {
                              d.DepartmentId,
                              d.DepartmentName
                          }).ToList();
            ViewBag.Departments = result;

            ViewBag.Result = result;

            return View(obj);
        }

        //POST
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Positions.Find(id);
            if (obj == null)
                return NotFound();

            _db.Positions.Remove(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
