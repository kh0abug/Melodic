using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Application.ExtensionMethods;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Melodic.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRole.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public INotyfService _notyfService { get; }
        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, INotyfService notyfService)
        {
            _db = db;
            _userManager = userManager;
            _notyfService = notyfService;
        }
        //GET: UserController
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModel = new List<UserViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                thisViewModel.Roles = await GetUserRoles(user);
                userViewModel.Add(thisViewModel);
            }
            return View(userViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {

                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _notyfService.Success("Deleted!");
                    return RedirectToAction("index");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
            return View("Index", _userManager.Users);
        }
    }
}
