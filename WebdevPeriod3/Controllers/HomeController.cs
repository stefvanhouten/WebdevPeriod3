using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Interfaces;
using WebdevPeriod3.Models;

namespace WebdevPeriod3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRespoitory _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRespoitory productRepository)
        {
            _logger = logger;
            _userRepository = productRepository;
            this.GetAll();
        }

        public async Task<ActionResult<User>> GetAll()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }


        public IActionResult Index()
        {
            return View();
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
