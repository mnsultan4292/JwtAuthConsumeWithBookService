using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthConsumeWithBookService
{
    public class BookHttpClient<T>
    {
        public async Task<List<T>> GetDataAsync(string url, string token)
        {
            List<T> lst = default(List<T>);
            using (var httpClient = new HttpClient())
            {
                //to add the token in header
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (var response = await httpClient.GetAsync(url))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<T>>(apiResponse);
                }
            }
            return lst;
        }

        public async Task<object> PostDataAsync(string url, T t, string token=null)
        {
            using (var httpClient = new HttpClient())
            {
                if(!string.IsNullOrEmpty(token))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(url, content))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }
            }
        }
        public async Task<int> DeleteAsync(string url, int bookId,string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.DeleteAsync(url))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return Convert.ToInt32(apiResponse);
                }
            }
        }
    }
}
