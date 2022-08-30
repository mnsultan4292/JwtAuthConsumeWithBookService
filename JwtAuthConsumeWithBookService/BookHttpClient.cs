using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthConsumeWithBookService
{
    public class BookHttpClient<T>
    {        
        public async Task<List<T>> GetDataAsync(string url)
        {
            List<T> lst = default(List<T>);
            using (var data = new HttpClient())
            {
                using (var response = await data.GetAsync(url))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<T>>(apiResponse);
                }
            }
            return lst;
        }

        public async Task<object> PostDataAsync(string url, T t)
        {

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(url, content))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }
            }
        }
    }
}
