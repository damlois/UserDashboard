using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UserDashboard.Models;
using UserDashboard.Repository;

namespace UserDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataRepository _dataRepo;

        public HomeController(IDataRepository dataRepo)
        {
            _dataRepo = dataRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string gender, int pageNumber)
        {
            if (string.IsNullOrEmpty(gender) || gender == "all") gender = "";
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            var randomUsers = await _dataRepo.GetMultipleUsers(4, gender, pageNumber);
            var user_gender = string.IsNullOrEmpty(gender) ? "All" : gender;
            ViewData["totalUsers"] = 4;
            if (TempData.Peek("randomUser") == null)
            {
                var index = new Random((int)DateTime.Now.Ticks).Next(0, randomUsers.Count - 1);
                TempData["randomUser"] = randomUsers[index].Name.First;
            }
            ViewData["gender"]  = user_gender;
            return View(randomUsers);
        }

        [HttpGet("/user/{email}")]
        public IActionResult UserDetails(string email)
        {
            var users = JsonSerializer.Deserialize<List<User>>((string)TempData["users"]);
            return View(users.First(x => x.Email == email));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
