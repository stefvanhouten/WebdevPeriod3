using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Models;
using WebdevPeriod3.Utilities;
using WebdevPeriod3.ViewModels;

namespace WebdevPeriod3.Controllers
{
    public class DashboardController : Controller
    {

        private readonly UserManager<User> _userManager;

        public DashboardController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<RobotPost> robotPosts = new List<RobotPost>();

            for (int i = 0; i < 15; i++)
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
            //TODO: Split into seperate controller?
            List<BlogImage> images = new List<BlogImage>();
            for (int i = 0; i < 4; i++)
            {
                BlogImage image = new BlogImage()
                {
                    ID = i,
                    Title = "Image name",
                    WebshopUrl = new List<string>() { "www.kaasislekker.nl" },
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
                    ImagePath = "~/images/another-robot.png"
                };
                images.Add(image);
            }

            PostViewModel viewModel = new PostViewModel()
            {
                Images = images
            };
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            
            // RETRIEVE USER INFORMATION
            User user = await _userManager.GetUserAsync(User);
            //user.UserName

            //SELECT ALL THE POSTS USER CREATED
            string SELECT = SqlHelper.CreateSelectWhereQuery("Products", "PosterId", $"{user.Id}");

            ProfileViewModel viewModel = new ProfileViewModel()
            {
                UserInformation = user,

            };
            return View(viewModel);
        }

        public IActionResult CreatePost()
        {
            return View();
        }
    }
}
