using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HR_Portal.Models;
using HR_Portal.Data;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HR_Portal.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var result = (from u in _db.ApplicationUser
                          select new
                          {
                              u.UserId,
                          }).ToList();

            if (result.Count == 0)
            { 
                return RedirectToAction("Initialize");
            }

            if (User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToPage("/Account/Login", new { area = "Identity" });

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "HR, IT, Admin")]
        public async Task<IActionResult> AdminAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            int? id = applicationUser.UserId;

            ViewBag.Roles = await _userManager.GetRolesAsync(applicationUser);

            string allRoles="";

            foreach (string role in ViewBag.Roles)
                allRoles += role;

            ViewData["userRoles"] = allRoles;

            return View();
        }

        public async Task<IActionResult> InitializeAsync()
        {

            string[] roleNames = { "Admin", "User", "HR", "IT" };

            foreach(string roleName in roleNames)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = roleName
                };

                await _roleManager.CreateAsync(identityRole);

            }
            

            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\user.txt");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            foreach (User obj in users)
            {
                if (obj.UserId == 0)
                {
                    //SUPER ADMIN
                    var admin = new ApplicationUser() { UserName = "admin@example.com", Email = "admin@example.com", UserId = obj.UserId };
                    await _userManager.CreateAsync(admin, "Wsei2022@$");
                    await _userManager.AddToRoleAsync(admin, "User");
                    await _userManager.AddToRoleAsync(admin, "HR");
                    await _userManager.AddToRoleAsync(admin, "IT");
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {

                    String email = obj.FirstName.Substring(0, 1) + obj.LastName + "@example.com";
                    String password = obj.DocumentId + email.Substring(0, 2) + "x!";
                    email = email.ToLower();

                    var user = new ApplicationUser() { UserName = email, Email = email, UserId = obj.UserId };

                    await _userManager.CreateAsync(user, password);

                    await _userManager.AddToRoleAsync(user, "User");

                    if (obj.PositionId == 1)
                        await _userManager.AddToRoleAsync(user, "HR");

                    if (obj.PositionId == 4)
                        await _userManager.AddToRoleAsync(user, "IT");
                }

            }

            json = System.IO.File.ReadAllText(@"wwwroot\lib\files\department.txt");
            List<Department> departments = JsonConvert.DeserializeObject<List<Department>>(json);
            foreach(Department department in departments)
            {
                var obj = _db.Departments.Find(department.DepartmentId);
                obj.DirectorId = department.DirectorId;
                _db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        public IActionResult News()
        {
            return View();
        }
    }
}
