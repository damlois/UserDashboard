using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

            var randomUsers = await _dataRepo.GetMultipleUsers(11, gender);
            if (randomUsers is null) { return View(Enumerable.Empty<User>().ToList()); }

            var user_gender = string.IsNullOrEmpty(gender) ? "All" : gender;
            ViewData["totalUsers"] = 11;
            ViewData["pageNumber"] = pageNumber;

            if (TempData.Peek("randomUser") == null && randomUsers.Any())
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
            var user = users.First(x => x.Email == email);
            return View(user);
        }

        [HttpGet("/find")]
        public IActionResult FindUser(string user, string country)
        {
            List<User> viewUsers = new List<User>(),
                users = new List<User>(),
                usersFound = new List<User>();

            if (TempData["users"] != null) { users = JsonSerializer.Deserialize<List<User>>((string)TempData["users"]); }
            ViewData["totalUsers"] = 11;

            if (string.IsNullOrEmpty(country))
            {
                usersFound = users.Where(
                x => x.FullName.Contains(user, StringComparison.InvariantCultureIgnoreCase) || x.Email.Contains(user, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
            }

            else if (string.IsNullOrEmpty(user))
            {
                usersFound = users.Where(x => x.UserLocation.Contains(country, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }    

            if (usersFound.Count != 0) viewUsers = usersFound;

            return View(viewUsers);
        }

        [Route("/users/downloadcsv")]
        public async Task<FileContentResult> DownloadCSV()
        {
            var users = JsonSerializer.Deserialize<List<User>>((string)TempData["users"]);
            var sb = new StringBuilder();
            sb.AppendLine("FullName, Email Address, Street, City, Country, Phone Number");
            foreach (var data in users)
            {
                sb.AppendLine(data.FullName + "," + data.Email + "," + data.UserLocation + "," + data.Phone);
            }
            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "users.csv");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
