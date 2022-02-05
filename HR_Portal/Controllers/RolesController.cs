using Microsoft.AspNetCore.Mvc;
using HR_Portal.Data;
using HR_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HR_Portal.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var querry = (from d in _db.UserRoles
                          join a in _db.ApplicationUser on d.UserId equals a.Id into tab
                          from a in tab.DefaultIfEmpty()
                          join u in _db.Users on a.UserId equals u.UserId into tab2
                          from u in tab2.DefaultIfEmpty()
                          join r in _db.Roles on d.RoleId equals r.Id into tab3
                          from r in tab3.DefaultIfEmpty()
                          select new
                          {
                              d.UserId,
                              d.RoleId,
                              u.FirstName,
                              u.LastName,
                              r.Name
                          }).ToList();
            if (querry.Count != 0)
                ViewBag.Users = querry;

            return View();
        }

        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            return View();
        }

        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(string role)
        {
            await _roleManager.CreateAsync(new IdentityRole(role));

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Assign(string RoleID)
        {
            var querry = (from a in _db.ApplicationUser
                          join u in _db.Users on a.UserId equals u.UserId into tab2
                          from u in tab2.DefaultIfEmpty()
                          select new
                          {
                              a.Id,
                              u.FirstName,
                              u.LastName,
                          }).ToList();
            if(querry.Count != 0)
                ViewBag.Users = querry;

            var querry2 = (from r in _db.Roles
                          select new
                          {
                              r.Id,
                              r.Name
                          }).ToList();
            if (querry.Count != 0)
                ViewBag.Roles = querry2;


            return View();
        }

        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignAsync(string user, string role)
        {
            var querry = (from r in _db.UserRoles
                          where r.RoleId.Equals(role) && r.UserId.Equals(user)
                          select new
                          {
                              r.RoleId
                          }).ToList();
            if (querry.Count == 0) {
                var appUser = await _userManager.FindByIdAsync(user);
                await _userManager.AddToRoleAsync(appUser, role);
             }


            return RedirectToAction("Index");

        }

        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnAssign(string user, string role)
        {
            var appUser = await _userManager.FindByIdAsync(user);
            await _userManager.RemoveFromRoleAsync(appUser, role);

            return RedirectToAction("Index");
        }

    }
}
