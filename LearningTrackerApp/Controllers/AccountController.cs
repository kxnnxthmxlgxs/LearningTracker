using LearningTrackerApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningTrackerApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserRepository _repo;

        public AccountController(UserRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            await _repo.CreateUser(username, password); // In a real app, we would hash this!
            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _repo.GetUserByUsername(username);
            if (user != null && user.PasswordHash == password) // Simple check
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Learning");
            }
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }
    }
}
