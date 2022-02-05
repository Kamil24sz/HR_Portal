// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using HR_Portal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HR_Portal.Models;
using HR_Portal.Data;

namespace HR_Portal.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly ApplicationDbContext _db;

        public PersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<PersonalDataModel> logger, ApplicationDbContext db)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            int? id = user.UserId;

            var querry = (from m in _db.Users
                            join p in _db.Positions on m.PositionId equals p.PositionId into tab
                            from p in tab.DefaultIfEmpty()
                            where m.UserId == id
                            select new
                            {
                                p.PositionName,
                                p.BaseSalary,
                                m.SalaryBonus
                            }).ToList();
            if (querry.Count == 1)
            {
                ViewData["PositionName"] = querry[0].PositionName;
                ViewData["Salary"] = querry[0].BaseSalary;
                ViewData["SalaryBonus"] = querry[0].SalaryBonus;
            }

            User modelUser = _db.Users.Find(id);
            ViewData["userID"] = modelUser.UserId;
            ViewData["firstName"] = modelUser.FirstName;
            ViewData["lastName"] = modelUser.LastName;
            ViewData["documentID"] = modelUser.DocumentId;
            ViewData["address"] = modelUser.Address;
            ViewData["bankAccount"] = modelUser.BankAccount;
            return Page();
        }
    }
}
