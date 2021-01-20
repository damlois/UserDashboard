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

        public async Task<List<User>> GetMultipleUsers(int amount, string gender)
        {
            string url = $"{randomUserUrl}?results={amount}&gender={gender}";

            var data = await FetchJson(url);

            return data;
        }

        private async static Task<List<User>> FetchJson(string url)
        {
            List<User> data = new List<User>();
            try
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

                        data = JsonConvert.DeserializeObject<List<User>>(results);
                    }
                }
            }

            catch(Exception)
            {
                data = null;
            }

            return data;

        }

    }
}
