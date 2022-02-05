using Microsoft.AspNetCore.Mvc;
using HR_Portal.Data;
using HR_Portal.Models;
using System.Text.Json;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace HR_Portal.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<User> usersList = _db.Users;
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;

            var result = (from u in _db.Users
                          join p in _db.Positions on u.PositionId equals p.PositionId into tab1
                          from p in tab1.DefaultIfEmpty()
                          join m in _db.Users on u.ManagerId equals m.UserId into tab3
                          from m in tab3.DefaultIfEmpty()
                          select new
                          {
                              u.UserId,
                              u.FirstName, u.LastName,
                              u.PositionId,
                              p.PositionName,
                              u.ManagerId,
                              u.CreationDate,
                              ManagerFirstName = m.FirstName,
                              ManagerLastName = m.LastName,
                              u.VacationDays
                          }).ToList();

            ViewBag.Users = result;

            return View(usersList);
        }

        [Authorize(Roles = "HR")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var resultId = _db.Users.Find(id);

            if (resultId == null)
            {
                return NotFound();
            }

            FillBagAsync();

            return View(resultId);
        }


        //POST
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(User obj)
        {
            FillBagAsync();

            if (ModelState.IsValid)
            {
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
            FillBagAsync();

            

            return View();
        }

        //POST
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(User obj, string permission)
        {

            if (obj.DateOfBirth.Date < new DateTime(1950, 1, 1).Date)
                ModelState.AddModelError("DateOfBirth", "Provided date " + obj.DateOfBirth + " is too early.");
            if (obj.DateOfBirth.Date > new DateTime(2003, 1, 1).Date)
                ModelState.AddModelError("DateOfBirth", "Provided date " + obj.DateOfBirth + " is too soon.");

            obj.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Users.Add(obj);
                _db.SaveChanges();

                String email = obj.FirstName.Substring(0, 1) + obj.LastName + "@example.com";
                String password = obj.DocumentId + email.Substring(0, 2) + "x!";
                email = email.ToLower();

                var user = new ApplicationUser() { UserName = email, Email = email, UserId = obj.UserId };

                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, permission);

                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? Myid = applicationUser.UserId;


            if (id == null || id == 0 || id == Myid)
                return NotFound();

            var obj = _db.Users.Find(id);

            if (obj == null)
                return NotFound();

            FillBagAsync();



            return View(obj);
        }

        //POST
        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Users.Find(id);
            if (obj == null)
                return NotFound();

            _db.Users.Remove(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "HR")]
        [NonAction]
        private async Task<ActionResult> FillBagAsync()
        {
            var managers = (from m in _db.Users
                            join p in _db.Positions on m.PositionId equals p.PositionId into tab
                            from p in tab.DefaultIfEmpty()
                            select new
                            {
                                p.PositionName,
                                m.FirstName,
                                m.LastName,
                                m.UserId
                            }).ToList();
            ViewBag.Managers = managers;

            var positions = (from p in _db.Positions
                             select new
                             {
                                 p.PositionId,
                                 p.PositionName
                             }).ToList();
            ViewBag.Positions = positions;

            ViewBag.Roles = _roleManager.Roles.ToList();

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;

            if(id == 0)
                ViewData["super-admin"] = "true";

            return View();

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBankAccount(int? id, string bankAccount)
        {

            if (id == null || id == 0)
                return NotFound();

            var obj = _db.Users.Find(id);

            if (obj == null)
                return NotFound();

            if (bankAccount == null)
                return NotFound();

            string strRegex = @"^[A-Z0-9]{15,50}$";
            Regex re = new Regex(strRegex);
           

            if (re.IsMatch(bankAccount))
            {
                obj.BankAccount = bankAccount;
                _db.SaveChanges();
                return RedirectToPage("/Account/Manage/PersonalData", new { area = "Identity" });
            }

            ModelState.AddModelError("DateOfBirth", "Provided date " + obj.DateOfBirth + " is too soon.");
            return RedirectToPage("/Account/Manage/PersonalData", new { area = "Identity" });
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAddress(int? id, string address)
    {

        if (id == null || id == 0)
            return NotFound();

        var obj = _db.Users.Find(id);

        if (obj == null)
            return NotFound();

        if (address == null)
            return NotFound();

        string strRegex = @"^[#.0-9a-zA-Z\s,-]+$";
        Regex re = new Regex(strRegex);


        if (re.IsMatch(address))
        {
            obj.Address = address;
            _db.SaveChanges();
            return RedirectToPage("/Account/Manage/PersonalData", new { area = "Identity" });
        }

        return RedirectToPage("/Account/Manage/PersonalData", new { area = "Identity" });
    }

        public IActionResult RedirectTOManageData()
        {
            return RedirectToPage("/Account/Manage/PersonalData", new { area = "Identity" });
        }

    }

 }
