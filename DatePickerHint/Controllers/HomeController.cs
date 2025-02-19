using DatePickerHint.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace DatePickerHint.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //Initializing logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //Action method for homepage
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //Errpr handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
