using Market_Store___First_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Market_Store___First_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.isLogin = false;
           if (HttpContext.Session.GetString("UserName") !=  null)
            {

                ViewBag.isLogin = true;
                ViewBag.name = HttpContext.Session.GetString("UserName");
            }
         
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact_us()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
