using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Services;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Interfaces;
using WebdevPeriod3.Models;

namespace WebdevPeriod3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, UserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            Request.Query.TryGetValue("userName", out var userNameValues);

            if (userNameValues.Count == 1)
            {
                var user = await _userRepository.FindByNormalizedUserName(userNameValues[0]);

                if (user != null)
                    return Ok(user);
                else
                    return NotFound();
            }

            Request.Query.TryGetValue("id", out var idValues);

            if (idValues.Count > 0) {
                await _userRepository.UpdateFieldById(idValues[0], user => user.PhoneNumber, "0511461556");

                return Ok(new
                {
                    value = await _userRepository.GetFieldByNormalizedUserName(idValues[0], user => user.AccessFailedCount)
                });
            }

            return Ok(await _userRepository.GetAll());
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
    }
}
