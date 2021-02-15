using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Models;
using WebdevPeriod3.ViewModels;

namespace WebdevPeriod3.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            List<User> users = new List<User>();
            for(int i = 0; i < 30; i++)
            {
                users.Add(new User() { Email = $"JohnDoe{i}@gmail.com", Role = "Admin" });
            }

            AdminViewModel viewModel = new AdminViewModel()
            {
                Users = users
            };
            return View(viewModel);
        }

        public IActionResult FlaggedComments()
        {
            List<Comment> comments = new List<Comment>();
            for (int i = 0; i < 30; i++)
            {
                comments.Add(new Comment() { Post= "Robot0", PostedBy = $"JohnDoe{i}@gmail.com", Summary = "Some racist comment" });
            }

            AdminViewModel viewModel = new AdminViewModel()
            {
                Comments = comments
            };
            return View(viewModel);
        }
    }
}
