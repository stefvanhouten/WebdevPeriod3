using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Models;
using WebdevPeriod3.ViewModels;
using WebdevPeriod3.Services;
using WebdevPeriod3.Entities;
using Microsoft.AspNetCore.Http;

namespace WebdevPeriod3.Controllers
{
    public class DashboardController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ProductRepository _productRepository;
        private readonly DapperProductStore _dapperProductStore;
        private readonly DapperCommentStore _dapperCommentStore;


        public DashboardController(UserManager<User> userManager, ProductRepository productRepository, DapperProductStore dapperProductStore, DapperCommentStore dapperCommentStore)
        {
            _userManager = userManager;
            _productRepository = productRepository;
            _dapperProductStore = dapperProductStore;
            _dapperCommentStore = dapperCommentStore;
        }

        public async Task<IActionResult> Index()
        {
            var leProducts = await _productRepository.GetAllProductsInCatalog();

            List<Product> productPosts = leProducts.ToList();


            DashboardViewModel viewModel = new DashboardViewModel()
            {
                ProductPosts = productPosts
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ViewPost(string id)
        {
            var result = await _productRepository.FindProductById(id);

            if (result == null)
                return NotFound();

            var product = result?.product;
            var subProducts = result?.subProducts;

            var groupedCommentData = result?.comments.GroupBy(comment => comment.ParentId);
            var topLevelCommentData = groupedCommentData.SingleOrDefault(group => group.Key == null)
                ?? Enumerable.Empty<HydratedComment>();

            var commentDataDictionary = groupedCommentData
                .Where(group => group.Key != null)
                .ToDictionary(group => group.Key, group => group.AsEnumerable());

            CommentViewModel CreateCommentViewModel(HydratedComment commentData) =>
                new CommentViewModel(
                    commentData.Id,
                    commentData.PosterName,
                    commentData.Content,
                    commentDataDictionary
                    .GetValueOrDefault(commentData.Id, Enumerable.Empty<HydratedComment>())
                    .Select(CreateCommentViewModel));

            var comments = topLevelCommentData.Select(CreateCommentViewModel);

            return View(new PostViewModel(product, subProducts, comments));
        }

        public async Task<IActionResult> Image(string id)
        {
            var image = await _productRepository.FindImageByProductId(id);

            if (image == null)
                return NotFound();

            return File(image, "image/png");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct()
        {
            return View(new ProductDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            const string NOT_AGREED_TO_TERMS = "Must agree to terms";

            if (dto.TermsGDPR == false)
            {
                ModelState.AddModelError(string.Empty, NOT_AGREED_TO_TERMS);
                return View(nameof(CreateProduct), dto);
            }

            var product = new Product()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = dto.Name,
                Description = dto.Description,
                PosterId = _userManager.GetUserId(User),
                ShowInCatalog = dto.ShowInCatalog,
                CreatedAt = DateTime.Now,
                Image = await ConvertToByteArray(dto.Image)
            };

            await _dapperProductStore.AddProductAsync(
                product,
                dto.SubSystems.Select(subSystemId => new ProductRelation()
                {
                    ProductId = product.Id,
                    SubProductId = subSystemId
                }));

            return RedirectToAction("Index");

            static async Task<byte[]> ConvertToByteArray(IFormFile file)
            {
                using var fileStream = file.OpenReadStream();
                using var memoryStream = new System.IO.MemoryStream();

                await fileStream.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string id, [FromForm] CommentDto commentDto)
        {
            var posterId = _userManager.GetUserId(User);

            await _dapperCommentStore.AddComment(commentDto.Content, id, commentDto.ParentId, posterId);

            return RedirectToAction(nameof(ViewPost), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FlagComment([FromRoute]string id, [FromForm] FlagCommentDto flagCommentDto)
        {
            await _dapperCommentStore.FlagComment(flagCommentDto.Id);

            return RedirectToAction(nameof(ViewPost), new { id });
        }
    }
}
