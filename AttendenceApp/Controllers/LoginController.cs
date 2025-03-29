using Microsoft.AspNetCore.Mvc;
using AttendenceApp.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AttendenceApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyAppContext _myAppContext;
        public LoginController(MyAppContext myAppContext)
        {
            _myAppContext = myAppContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = Request.Cookies["UserId"];
            var userName = Request.Cookies["UserName"];

            if (userId != null && userName != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Auth(string email, string password)
        {

            var user = await _myAppContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "Invalid email or password.";
                return View("index");
            }

            // Store user data in session
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Name);

            // Store user data in cookies
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddHours(1), // Set expiration time for 1 hour
                HttpOnly = true, // Prevent JavaScript access (security best practice)
                Secure = true, // Use secure cookies (recommended for HTTPS)
                SameSite = SameSiteMode.Strict // Prevent CSRF attacks
            };

            // Set cookies
           

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            return RedirectToAction("index", "login");
        }
        public IActionResult NotFound()
        {
            return View("NotFound"); // Render the custom 404 page
        }

    }
}
