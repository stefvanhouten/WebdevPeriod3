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
using WebdevPeriod3.Services;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.Controllers
{
    public class DashboardController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ProductRepository _productRepository;
        private readonly DapperProductStore _dapperProductStore;


        public DashboardController(UserManager<User> userManager, ProductRepository productRepository, DapperProductStore dapperProductStore)
        {
            _userManager = userManager;
            _productRepository = productRepository;
            _dapperProductStore = dapperProductStore;
        }

        public async Task<IActionResult> Index()
        {
            var leProducts = await _productRepository.GetAllProducts();

            List<Product> productPosts = leProducts.ToList();


            DashboardViewModel viewModel = new DashboardViewModel()
            {
                ProductPosts = productPosts
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
            var user = await _userManager.GetUserAsync(User);

            ProfileViewModel viewModel = new ProfileViewModel()
            {
                UserInformation = user,
                OwnProducts = await _productRepository.FindProductsByPosterId(user.Id)

            };
            return View(viewModel);
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            const string NOT_SIGNED_IN_ERROR = "Not logged in";
            const string NOT_AGREED_TO_TERMS = "Must agree to terms";
            //const string NO_SUBSYSTEMS = "Must have at least 2 subsystems";

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, NOT_SIGNED_IN_ERROR);
                return View(nameof(CreatePost), dto);
            }
            /* if (dto.SubSystems.Count() < 2)
            {
                ModelState.AddModelError(string.Empty, NO_SUBSYSTEMS);
                return View(nameof(CreatePost), dto);
            } */
            if (dto.TermsGDPR == false)
            {
                ModelState.AddModelError(string.Empty, NOT_AGREED_TO_TERMS);
                return View(nameof(CreatePost), dto);
            }

            var product = new Product()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = dto.Name,
                Description = dto.Description,
                PosterId = user.Id,
                ShowInCatalog = true,
                CreatedAt = DateTime.Now
            };

            await _dapperProductStore.AddProductAsync(product);

            foreach (string Subsystem in dto.SubSystems)
            {
                var productRelation = new ProductRelation()
                {
                    ProductId = product.Id,
                    SubProductId = Subsystem
                };

                await _dapperProductStore.AddSubProductAsync(productRelation);
            }

            return RedirectToAction("Index");
        }
    }
}
