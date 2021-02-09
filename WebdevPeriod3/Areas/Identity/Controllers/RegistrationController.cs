using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Models;

namespace WebdevPeriod3.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RegistrationController : Controller
    {
        private const string GLOBAL_ERROR_KEY = "GLOBAL";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegistrationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), dto);

            var user = new User(dto.UserName)
            {
                Email = dto.Email
            };

            var creationResult = await _userManager.CreateAsync(user, dto.Password);

            if (!creationResult.Succeeded)
            {
                foreach (var error in creationResult.Errors)
                    ModelState.AddModelError(GLOBAL_ERROR_KEY, error.Description);

                return View(nameof(Index), dto);
            }

            await _signInManager.SignInAsync(user, dto.RemainSignedIn);

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
