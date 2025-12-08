using ContactProMVC.Data;
using ContactProMVC.Models;
using ContactProMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ContactProMVC.Controllers
{
    public class DemoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public DemoController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet("demo-login")]
        public async Task<IActionResult> DemoLogin()
        {
            var demoUserId = Guid.NewGuid().ToString("N")[..12];

            if (Request.Cookies.TryGetValue("DemoUserId", out var oldDemoId))
            {
                await DataHelper.DeleteDemoUserAsync(_userManager, _context, oldDemoId);
            }

            var demoUser = await DataHelper.CreateDemoUserAsync(_userManager, _context, demoUserId);

            Response.Cookies.Append("DemoUserId", demoUserId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddHours(1),
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax
            });

            await _signInManager.SignInAsync(demoUser, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
    }
}
