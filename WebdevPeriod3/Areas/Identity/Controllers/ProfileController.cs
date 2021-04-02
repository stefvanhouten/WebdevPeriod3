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
        private readonly ProductRepository _productRepository;

        public ProfileController(UserManager<User> userManager, ProductRepository productManager)
        {
            _userManager = userManager;
            _productRepository = productManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // RETRIEVE USER INFORMATION
            var user = await _userManager.GetUserAsync(User);

            var profileDto = new ProfileDto()
            {
                UserInformation = user,
                OwnProducts = await _productRepository.FindProductsByPosterId(user.Id)
            };

            return View(profileDto);
        }
    }
}
