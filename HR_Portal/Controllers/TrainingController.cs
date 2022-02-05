using Microsoft.AspNetCore.Mvc;
using HR_Portal.Data;
using HR_Portal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HR_Portal.Controllers
{
    [Authorize]
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = "HR,IT")]
        public IActionResult Index()
        {
            IEnumerable<Training> TrainingList = _db.Trainings.ToList();
            return View(TrainingList);
        }

        [Authorize(Roles = "HR,IT")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var resultId = _db.Trainings.Find(id);

            if (resultId == null)
            {
                return NotFound();
            }


            return View(resultId);
        }

        //POST
        [Authorize(Roles = "HR,IT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Training obj)
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
            return View();
        }

        //POST
        [Authorize(Roles = "HR,IT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Training obj)
        {

            if (ModelState.IsValid)
            {
                _db.Trainings.Add(obj);
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

            var obj = _db.Trainings.Find(id);

            if (obj == null)
                return NotFound();


            return View(obj);
        }

        //POST
        [Authorize(Roles = "HR,IT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Trainings.Find(id);
            if (obj == null)
                return NotFound();

            _db.Trainings.Remove(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserTrainingsAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;

            var querry = (from d in _db.TrainingDestinations
                          join t in _db.Trainings on d.TrainingId equals t.TrainingId into tab
                          from t in tab.DefaultIfEmpty()
                          where d.UserId == id
                          select new
                          {
                              d.UserId,
                              d.DestinationId,
                              t.TrainingName,
                              t.TrainingType,
                              t.Cost,
                              d.Progress
                          })
                         .ToList();

            if (querry.Count != 0) ViewBag.Trainings = querry;

            var querry2 = (from d in _db.TrainingDestinations
                          join t in _db.Trainings on d.TrainingId equals t.TrainingId into tab
                          from t in tab.DefaultIfEmpty()
                          where d.UserId == id && d.Progress == 0
                          select new
                          {
                              d.UserId
                          })
                         .ToList();
            if (querry2.Count != 0) ViewData["assigned"] = "true";

            return View();
        }

        //GET
        public async Task<IActionResult> AssignAsync(int? id, int? error)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? UserId = applicationUser.UserId;

            if(error is not null)
            {
                ViewData["assigned"] = "true";
            }

            if (id is null)
                id = 1;

            var querry = (from t in _db.Trainings
                          where t.TrainingId == id
                          select new
                          {
                              t.TrainingId,
                              t.TrainingName,
                              t.TrainingType,
                              t.Cost,
                              t.Description
                          })
                         .ToList();

            if (querry.Count == 1) ViewBag.Result = querry;

            var querry2 = (from t in _db.Trainings
                           select new
                           {
                               t.TrainingId,
                               t.TrainingName,
                               t.Cost
                           })
                         .ToList();
            ViewBag.Trainings = querry2;

            ViewData["User"] = UserId;

            return View("Assign");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(TrainingDestination obj)
        {

            if (obj == null)
                return NotFound();

            var querry = (from d in _db.TrainingDestinations
                          where d.UserId == obj.UserId && d.TrainingId == obj.TrainingId && obj.Progress != 2
                          select new
                          {
                              d.UserId
                          })
                        .ToList();

            if (querry.Count != 0)
            {

                return RedirectToAction("Assign", new { error = 1 });
            }

            if (obj.Progress > 0)
                obj.Progress = -1;


            if (ModelState.IsValid)
            {
                _db.TrainingDestinations.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("UserTrainings");
            }

            return RedirectToAction("UserTrainings");
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Select(int? id)
        {

            return RedirectToAction("Assign", new { id = id });
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnAssignAsync(int? id)
        {
            var obj = _db.TrainingDestinations.Find(id);
            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            if (Myid == obj.UserId)
            {

                if (obj.Progress == 0 || obj.Progress == -1)
                {
                    _db.TrainingDestinations.Remove(obj);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("UserTrainings");
        }
    }
}
