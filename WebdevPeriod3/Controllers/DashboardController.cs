using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Models;
using WebdevPeriod3.ViewModels;

namespace WebdevPeriod3.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            List<RobotPost> robotPosts = new List<RobotPost>();

            for (int i = 0; i < 10; i++)
            {
                RobotPost post = new RobotPost()
                {
                    ID = i,
                    Name = $"Robot{i}",
                    Description = "Some description",
                    Replies = i * 5,
                };
                robotPosts.Add(post);
            }

            DashboardViewModel viewModel = new DashboardViewModel()
            {
                RobotPosts = robotPosts
            };

            return View(viewModel);
        }

        public IActionResult ViewPost(int id)
        {
            return View();
        }
    }
}
