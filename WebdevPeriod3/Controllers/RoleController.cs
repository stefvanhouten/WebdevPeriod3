using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;

namespace WebdevPeriod3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public RoleController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("{userId}/add/{roleName}")]
        public async Task<IActionResult> AddToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            bool ResultContainsError(string name) => result.Errors.Any(error => error.Code == name);

            if (result.Succeeded || ResultContainsError(nameof(IdentityErrorDescriber.UserAlreadyInRole))) return Ok();

            if (ResultContainsError(nameof(IdentityErrorDescriber.InvalidRoleName))) return NotFound();

            throw new NotImplementedException($"We can't handle the identity error that has been raised:\n\r" +
                string.Join("\n\r", result.Errors.Select(error => error.Code)));
        }

        [HttpPost("{userId}/remove/{roleName}")]
        public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            bool ResultContainsError(string name) => result.Errors.Any(error => error.Code == name);

            if (result.Succeeded || ResultContainsError(nameof(IdentityErrorDescriber.UserNotInRole))) return Ok();

            if (ResultContainsError(nameof(IdentityErrorDescriber.InvalidRoleName))) return NotFound();

            throw new NotImplementedException($"We can't handle the identity error that has been raised:\n\r" +
                string.Join("\n\r", result.Errors.Select(error => error.Code)));
        }
    }
}
