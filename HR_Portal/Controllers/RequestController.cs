using Microsoft.AspNetCore.Mvc;
using HR_Portal.Data;
using HR_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace HR_Portal.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            int? id = applicationUser.UserId;

            ViewBag.UserID = id;

            var result = (from r in _db.Requests
                          join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab
                          from s in tab.DefaultIfEmpty()
                          join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab2
                          from t in tab2.DefaultIfEmpty()
                          where r.RequestorId == id
                          select new
                          {
                              r.RequestId,
                              t.RequestName,
                              s.StatusId,
                              s.StatusName,
                              r.Description,
                              r.CreationDate
                          })
                         .ToList();

            if(result.Count != 0)  ViewBag.Result = result;

            User manager;
            if (_db.Users.Find(id).ManagerId is not null) 
            {
                manager = _db.Users.Find(_db.Users.Find(id).ManagerId);
                ViewBag.Manager = manager.FirstName +" "+ manager.LastName;

                var ManagerEmail = (from u in _db.Users
                          join a in _db.ApplicationUser on u.UserId equals a.UserId into tab
                          from a in tab.DefaultIfEmpty()
                          where a.UserId == manager.UserId
                          select new
                          {
                              a.Email
                          })
                          .ToList();
                if (ManagerEmail.Count != 0) ViewBag.ManagerEmail = ManagerEmail;
            }

            var HrPerosns = (from u in _db.Users
                             join a in _db.ApplicationUser on u.UserId equals a.UserId into tab
                             from a in tab.DefaultIfEmpty()
                             where u.PositionId == 1
                             select new
                             {
                                 u.FirstName,
                                 u.LastName,
                                 a.Email
                             })
                          .ToList();
            if(HrPerosns.Count != 0) ViewBag.HR = HrPerosns;

            var result2 = (from r in _db.Requests
                           where r.RequestorId == id && r.StatusId == 2
                           select new
                          {
                              r.RequestId
                          })
                         .ToList();

            if (result2.Count != 0) ViewData["PendingHR"] = "true";


            return View();
        }

        //GET
        public async Task<IActionResult> InfoAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _db.Requests.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? managerid = applicationUser.UserId;

            var querry = (from r in _db.Requests
                          join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab
                          from s in tab.DefaultIfEmpty()
                          join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab2
                          from t in tab2.DefaultIfEmpty()
                          join u in _db.Users on r.RequestorId equals u.UserId into tab3
                          from u in tab3.DefaultIfEmpty()
                          where r.RequestId == id && (r.RequestorId == managerid || u.ManagerId == managerid)
                          select new
                          {
                              r.RequestId,
                              t.RequestName,
                              r.StatusId,
                              s.StatusName,
                              r.Description,
                              r.CreationDate
                          })
                         .ToList();

            if (querry.Count != 0) ViewBag.Result = querry;


            var querry2 = (from r in _db.Requests
                          join u in _db.Users on r.RequestorId equals u.UserId into tab
                          from u in tab.DefaultIfEmpty()
                          where u.ManagerId == managerid
                           select new
                          {
                                r.RequestId
                          })
                         .ToList();

            if (querry2.Count != 0) ViewData["Manager"] = "true";
            else ViewData["Manager"] = "false";


            return View(result);
        }

        //GET
        public async Task<IActionResult> CreateAsync()
        {
            var types = (from t in _db.RequestTypes
                          select new
                          {
                              t.RequestTypeId,
                              t.RequestName
                          })
                         .ToList();
            ViewBag.RequestTypes = types;

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;

            var vacationDays = (from u in _db.Users
                                where u.UserId == id
                                select new
                                {
                                    u.VacationDays
                                })
                         .ToList();
            ViewBag.VacationDays = vacationDays;

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(Request obj)
        {

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;

            obj.CreationDate = DateTime.Now;

            obj.RequestorId = id ?? default;

            if (obj.RequestTypeId == 1 || obj.RequestTypeId == 3 || obj.RequestTypeId == 4)
                obj.StatusId = 1;
            else if (obj.RequestTypeId == 2 || obj.RequestTypeId == 5)
                obj.StatusId = 2;

            if (obj.RequestTypeId == 1)
            {
                User user = _db.Users.Find(id);
                user.VacationDays = Int32.Parse(getBetween(obj.Description, "after operation: ", "\r\n"));
                _db.SaveChanges();
            }

            if (ModelState.IsValid)
            {
                _db.Requests.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Requests.Find(id);

            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;
            if(obj.RequestorId == Myid)
            {

                var querry = (from r in _db.Requests
                              join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab
                              from s in tab.DefaultIfEmpty()
                              join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab2
                              from t in tab2.DefaultIfEmpty()
                              where r.RequestId == id
                              select new
                              {
                                  r.RequestId,
                                  t.RequestTypeId,
                                  t.RequestName,
                                  s.StatusId,
                                  s.StatusName,
                                  r.Description,
                                  r.CreationDate
                              })
                             .ToList();

                if (querry.Count != 0) ViewBag.Result = querry;

                return View(obj);
            }
            else
                return NotFound();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOSTAsync(int? id)
        {
            var obj = _db.Requests.Find(id);
            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            if (obj.RequestorId == Myid)
            {
                _db.Requests.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return NotFound();
            
        }

        public async Task<IActionResult> ManagerAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;


            var querry = (from u in _db.Users
                              join p in _db.Positions on u.PositionId equals p.PositionId into tab
                              from p in tab.DefaultIfEmpty()
                              where u.ManagerId == id && u.ManagerId != u.UserId
                          select new
                          {
                              u.UserId,
                              u.FirstName,
                              u.LastName,
                              p.PositionName,
                              u.VacationDays,
                              p.BaseSalary,
                              u.SalaryBonus

                          })
                         .ToList();

            if (querry.Count != 0) ViewBag.Users = querry;

            var querry2 = (from r in _db.Requests
                            join u in _db.Users on r.RequestorId equals u.UserId into tab
                            from u in tab.DefaultIfEmpty()
                            join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab2
                            from t in tab2.DefaultIfEmpty()
                          where u.ManagerId == id && r.StatusId == 1  && u.ManagerId != u.UserId
                      select new
                      {
                          r.RequestId,
                          t.RequestName,
                          u.FirstName,
                          u.LastName,
                          r.CreationDate
                      })
                         .ToList();

            if (querry2.Count != 0) ViewBag.Requests = querry2;

            var querry3 = (from d in _db.TrainingDestinations
                           join u in _db.Users on d.UserId equals u.UserId into tab
                           from u in tab.DefaultIfEmpty()
                           join t in _db.Trainings on d.TrainingId equals t.TrainingId into tab2
                           from t in tab2.DefaultIfEmpty()
                           where u.ManagerId == id && d.Progress == -1
                           select new
                           {
                               d.DestinationId,
                               u.FirstName,
                               u.LastName,
                               t.TrainingName,
                               t.Cost,
                               t.TrainingType

                           })
                         .ToList();
            if (querry3.Count != 0) ViewBag.Trainings = querry3;

            return View();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusAsync(int? id, int status)
        {

            var obj = _db.Requests.Find(id);
            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? myId = applicationUser.UserId;

            var querry = (from r in _db.Requests
                          join u in _db.Users on r.RequestorId equals u.UserId into tab
                          from u in tab.DefaultIfEmpty()
                          where (r.RequestId == id && u.ManagerId == myId) || u.PositionId == 1
                          select new
                          {
                              r.RequestId
                          })
                         .ToList();
            if (querry.Count != 0)
            {

                if (obj.RequestTypeId == 4 && status == 4)
                    obj.StatusId = 2;
                else
                    obj.StatusId = status;

                if (obj.RequestTypeId == 1 && obj.StatusId == 3)
                {
                    User user = _db.Users.Find(obj.RequestorId);
                    user.VacationDays = Int32.Parse(getBetween(obj.Description, "before operation: ", "\r\n"));
                    _db.SaveChanges();
                }


                _db.SaveChanges();

                return RedirectToAction("Manager");
            }
            else
                return NotFound();
        }

        public async Task<IActionResult> ChangeBonus(int? id, int newBonus)
        {

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            var querry = (from u in _db.Users
                          where u.ManagerId == Myid
                          select new
                          {
                              u.UserId
                          })
                         .ToList();
            if (querry.Count != 0)
            {

                var obj = _db.Users.Find(id);
                if (obj == null)
                    return NotFound();

                obj.SalaryBonus = newBonus;
                _db.SaveChanges();

                return RedirectToAction("Manager");
            }
            else
                return NotFound();
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptTrainingAsync(int? id)
        {

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            var querry = (from t in _db.TrainingDestinations
                          join u in _db.Users on t.UserId equals u.UserId into tab
                          from u in tab.DefaultIfEmpty()
                          where u.ManagerId == Myid
                          select new
                          {
                              t.UserId
                          })
                         .ToList();
            if (querry.Count != 0)
            {

                var obj = _db.TrainingDestinations.Find(id);
                if (obj == null)
                    return NotFound();

                obj.Progress = 0;

                _db.SaveChanges();

                return RedirectToAction("Manager");
            }
            else
                return NotFound();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectTrainingAsync(int? id)
        {

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            var querry = (from t in _db.TrainingDestinations
                          join u in _db.Users on t.UserId equals u.UserId into tab
                          from u in tab.DefaultIfEmpty()
                          where u.ManagerId == Myid
                          select new
                          {
                              t.UserId
                          })
                         .ToList();
            if (querry.Count != 0)
            {
                var obj = _db.TrainingDestinations.Find(id);
                if (obj == null)
                    return NotFound();

                obj.Progress = 2;

                _db.SaveChanges();

                return RedirectToAction("Manager");
            }
            else
                return NotFound();
        }


        public IActionResult ViewAllAsync()
        {


            var querry = (from r in _db.Requests
                          join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab
                          from t in tab.DefaultIfEmpty()
                          join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab2
                          from s in tab2.DefaultIfEmpty()
                          join u in _db.Users on r.RequestorId equals u.UserId into tab3
                          from u in tab3.DefaultIfEmpty()
                          select new
                          {
                              r.RequestId,
                              t.RequestName,
                              s.StatusName,
                              u.FirstName,
                              u.LastName,
                              r.Description,
                              r.CreationDate
                          })
                         .ToList();
            if (querry.Count != 0)
            {
                ViewBag.Requests = querry;
                return View();
            }
            else
                return NotFound();
        }

        [Authorize(Roles = "HR")]
        public IActionResult ViewHR()
        {

            var querry = (from r in _db.Requests
                          join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab
                          from t in tab.DefaultIfEmpty()
                          join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab2
                          from s in tab2.DefaultIfEmpty()
                          join u in _db.Users on r.RequestorId equals u.UserId into tab3
                          from u in tab3.DefaultIfEmpty()
                          where r.StatusId == 2
                          select new
                          {
                              r.RequestId,
                              t.RequestName,
                              s.StatusName,
                              u.FirstName,
                              u.LastName,
                              r.Description,
                              r.CreationDate
                          })
                         .ToList();
            if (querry.Count != 0)
                ViewBag.Requests = querry;

                return View();

        }


        [Authorize(Roles = "HR")]
        public IActionResult InfoHRAsync(int? id, bool stat)
        {
            if (stat)
                ViewBag.Stat = "true";


            var status = (from s in _db.RequestStatuses
                                       select new
                                       {
                                           s.StatusId,
                                           s.StatusName
                                       })
                         .ToList();
            if (status.Count != 0)
                ViewBag.Status = status;


            var querry = (from r in _db.Requests
                          join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab
                          from t in tab.DefaultIfEmpty()
                          join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab2
                          from s in tab2.DefaultIfEmpty()
                          join u in _db.Users on r.RequestorId equals u.UserId into tab3
                          from u in tab3.DefaultIfEmpty()
                          where r.RequestId == id
                          select new
                          {
                              r.RequestId,
                              t.RequestName,
                              s.StatusName,
                              s.StatusId,
                              u.FirstName,
                              u.LastName,
                              r.Description,
                              r.CreationDate
                          })
                         .ToList();
            if (querry.Count != 0)
            {
                ViewBag.Result = querry;
                return View();
            }
            else
                return NotFound();
        }

        [Authorize(Roles = "HR")]
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int? id, int status, string desc)
        {

            if (id is null || id == 0)
                return NotFound();

            Request? req = _db.Requests.Find(id);

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            if (req is not null && req.RequestorId!=Myid)
            {
                req.StatusId = status;
                req.Description = desc;
                _db.SaveChanges();
                return RedirectToAction("ViewAll");
            }

            return NotFound();

        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeletePOSTHRAsync(int? id)
        {

            var obj = _db.Requests.Find(id);
            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            if (obj.RequestorId != Myid)
            {
                _db.Requests.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("ViewAll");
            }
            return NotFound();

        }

        [Authorize(Roles = "HR")]
        public IActionResult DeleteHR(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Requests.Find(id);

            if (obj == null)
                return NotFound();


                var querry = (from r in _db.Requests
                              join s in _db.RequestStatuses on r.StatusId equals s.StatusId into tab
                              from s in tab.DefaultIfEmpty()
                              join t in _db.RequestTypes on r.RequestTypeId equals t.RequestTypeId into tab2
                              from t in tab2.DefaultIfEmpty()
                              join u in _db.Users on r.RequestorId equals u.UserId into tab3
                              from u in tab3.DefaultIfEmpty()
                              where r.RequestId == id
                              select new
                              {
                                  r.RequestId,
                                  t.RequestTypeId,
                                  t.RequestName,
                                  u.FirstName,
                                  u.LastName,
                                  s.StatusId,
                                  s.StatusName,
                                  r.Description,
                                  r.CreationDate
                              })
                             .ToList();

                if (querry.Count != 0) ViewBag.Result = querry;
                    return View(obj);
                
                return NotFound();


        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> AcceptHRAsync(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Requests.Find(id);

            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            if(obj.RequestorId != Myid)
            {
                obj.StatusId = 4;
                _db.SaveChanges();

                return View("ViewHR");
            }

            return NotFound();

        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> RejectHRAsync(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Requests.Find(id);

            if (obj == null)
                return NotFound();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;

            if (obj.RequestorId != Myid)
            {
                obj.StatusId = 3;
                _db.SaveChanges();

                return View("ViewHR");
            }

            return NotFound();

        }


        public IActionResult ChooseStatus(int? id)
        {
            return RedirectToAction("InfoHR", new { id, stat = true});
        }

    }

}
