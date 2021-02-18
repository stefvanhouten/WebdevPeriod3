using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Areas.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using WebdevPeriod3.Services;
using WebdevPeriod3.Areas.Identity.Models;

namespace WebdevPeriod3.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class ProfileController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ProductManager _productManager;

        public ProfileController(UserManager<User> userManager, ProductManager productManager)
        {
            _userManager = userManager;
            _productManager = productManager;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            // RETRIEVE USER INFORMATION
            User user = await _userManager.GetUserAsync(User);

            ProfileDto profileDto = new ProfileDto()
            {
                UserInformation = user,
                OwnProducts = await _productManager.GetProductsByPosterId(user.Id)
            };
            return View(profileDto);
        }
    }
}
