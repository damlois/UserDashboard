using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UserDashboard.Models;

namespace UserDashboard.Repository
{
    public class DataRepository : IDataRepository
    {
        private readonly string randomUserUrl;

        public DataRepository(IConfiguration configuration)
        {
            randomUserUrl = configuration["RandomUserUrl"];
        }

        public async Task<List<User>> GetMultipleUsers(int amount, string gender, int pageNumber)
        {
            const int limit = 3;
            var users = new List<User>();
            string url = $"{randomUserUrl}?results={amount}&gender={gender}";

            var data = await FetchJson(url);

            if (data != null)
            {
                data = data.Skip((pageNumber - 1) * limit).Take(limit).ToList();
                foreach (var item in data)
                {
                    item.CurrentPage = pageNumber;
                    users.Add(item);
                }
            }

            return users;
        }

        public async Task<User> GetSingleUser()
        {
            var user = new User();

            string url = randomUserUrl;

            var data = await FetchJson(url);

            if (data != null)
                user = data.FirstOrDefault();

            return user;
        }

        public User FindUser(string email)
        {
            var randomUsers = new List<User> { new User { Email = "testtest00" } };
            return randomUsers.FirstOrDefault(x => x.Email == email);
        }

        private async static Task<List<User>> FetchJson(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var results = JObject.Parse(json)["results"].ToString();

                    var data = JsonConvert.DeserializeObject<List<User>>(results);
                    return data;
                }
            }

            return null;
        }

    }
}
