using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Models;

namespace WebdevPeriod3.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            const string INVALID_CREDENTIALS_MESSAGE = "Invalid credentials";

            if (!ModelState.IsValid)
                return View(nameof(Index), dto);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, INVALID_CREDENTIALS_MESSAGE);
                return View(nameof(Index), dto);
            }

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RemainSignedIn, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "Sorry, you've been locked out.");
                if (result.IsNotAllowed)
                    ModelState.AddModelError(string.Empty, "You're not allowed to log in.");
                if (result.RequiresTwoFactor)
                    ModelState.AddModelError(string.Empty, "You need to use two-factor authentication.");
                if (!(result.IsLockedOut || result.IsNotAllowed || result.RequiresTwoFactor))
                    ModelState.AddModelError(string.Empty, INVALID_CREDENTIALS_MESSAGE);

                return View(nameof(Index), dto);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}
