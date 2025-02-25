using System.Diagnostics;
using AttendenceApp.DatabaseContext;
using AttendenceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace AttendenceApp.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ILogger<HomeController> _logger;
        private readonly MyAppContext _appContext;

        public HomeController(ILogger<HomeController> logger,MyAppContext _appContext)
        {
            _logger = logger;
            this._appContext = _appContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Events()
        {
            var events = _appContext.Events.OrderBy(e => e.StartDate).ToList();
            return View(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
