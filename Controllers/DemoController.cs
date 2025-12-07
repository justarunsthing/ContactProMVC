using ContactProMVC.Data;
using ContactProMVC.Models;
using ContactProMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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

        [HttpGet("demo-login")]
        public async Task<IActionResult> DemoLogin()
        {
            if (!Request.Cookies.ContainsKey("DemoUserId"))
            {
                var demoId = Guid.NewGuid().ToString("N")[..12];

                Response.Cookies.Append("DemoUserId", demoId, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });
            }

            var demoIdFromCookie = Request.Cookies["DemoUserId"];
            var demoUser = await DataHelper.GetOrCreateDemoUserAsync(_userManager, _context, demoIdFromCookie!);

            await _signInManager.SignInAsync(demoUser, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
    }
}
