using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Services;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Interfaces;
using WebdevPeriod3.ViewModels;
using WebdevPeriod3.Models;
using WebdevPeriod3.Utilities;

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
            return View();
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        //Should be moved to a auth controller
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        //[Authorize]
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
