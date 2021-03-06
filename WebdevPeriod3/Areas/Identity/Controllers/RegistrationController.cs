﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Models;

namespace WebdevPeriod3.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RegistrationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegistrationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index([FromQuery] string returnUrl) => View(new RegistrationDto(returnUrl));

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
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(nameof(Index), dto);
            }

            await _signInManager.SignInAsync(user, dto.RemainSignedIn);

            if (dto.ReturnUrl != null)
                return Redirect(dto.ReturnUrl);
            else
                return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
