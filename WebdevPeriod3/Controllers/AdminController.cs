using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Services;
using WebdevPeriod3.Models;
using WebdevPeriod3.ViewModels;

namespace WebdevPeriod3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserRepository _userRepository;

        public AdminController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Users()
        {
            AdminViewModel viewModel = new AdminViewModel()
            {
                Users = (await _userRepository.GetAll()).ToList()
            };

            return View(viewModel);
        }

        public IActionResult FlaggedComments()
        {
            List<Comment> comments = new List<Comment>();
            for (int i = 0; i < 30; i++)
            {
                comments.Add(new Comment() { Post = "Robot0", PostedBy = $"JohnDoe{i}@gmail.com", Summary = "Some racist comment" });
            }

            AdminViewModel viewModel = new AdminViewModel()
            {
                Comments = comments
            };
            return View(viewModel);
        }
    }
}
