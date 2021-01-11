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
            randomUserUrl = configuration["RadomUserUrl"];
        }

        public async Task<List<User>> GetMultipleUsers(int amount)
        {
            var users = new List<User>();
            string url = $"{randomUserUrl}?results={amount}";

            var data = await FetchJson(url);

            if (data != null)
            {
                foreach (var item in data)
                {
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
