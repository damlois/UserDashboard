using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UserDashboard.Models;
using UserDashboard.Repository;

namespace UserDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataRepository _dataRepo;

        public HomeController(ILogger<HomeController> logger, IDataRepository dataRepo)
        {
            _logger = logger;
            _dataRepo = dataRepo;
        }

        [Route("/")]
        [Route("/{pageNumber}")]
        [Route("/users/{gender}")]
        [Route("/users/{gender}/{pageNumber}")]
        public async Task<IActionResult> Index(string gender, int pageNumber)
        {
            if (string.IsNullOrEmpty(gender) || gender == "all") gender = "";
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            var randomUsers = await _dataRepo.GetMultipleUsers(4, gender, pageNumber);
            var randomUser = await _dataRepo.GetSingleUser();
            var user_gender = String.IsNullOrEmpty(gender) ? "All" : gender;
            ViewData["totalUsers"] = 4;
            ViewData["user"] = randomUser.Name.First;
            ViewData["gender"]  = user_gender;
            return View(randomUsers);
        }

        [Route("/home/user/{id}")]
        public IActionResult UserDetails(string email)
        {
            var user = _dataRepo.FindUser(email);  
            ViewData["user"] = user;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
