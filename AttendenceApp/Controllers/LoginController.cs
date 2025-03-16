using Microsoft.AspNetCore.Mvc;
using AttendenceApp.DatabaseContext;
using Microsoft.EntityFrameworkCore;

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

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index","login"); 
        }


    }
}
