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
    public class Program
    {
        public static string _token;
        public string url = string.Empty;
        public async Task<List<MasterBook>> GetBook()
        {
            BookHttpClient<MasterBook> obj = new BookHttpClient<MasterBook>();
            url = ConfigurationManager.AppSettings["GetBookUrl"].ToString();
            return await obj.GetDataAsync(url);
        }

        public async Task<string> Login()
        {
            url = ConfigurationManager.AppSettings["LoginUrl"].ToString();
            BookHttpClient<UserLogin> obj = new BookHttpClient<UserLogin>();
            UserLogin userLogin = new UserLogin()
            {
                Username = "naeem",
                Password = "naeem_admin"
            };
            var response = await obj.PostDataAsync(url, userLogin);
            return response.ToString();
        }
        public async Task<int> PostBookData()
        {
            url = ConfigurationManager.AppSettings["PostBook"].ToString();
            BookHttpClient<MasterBook> obj = new BookHttpClient<MasterBook>();

            MasterBook masterBook = new MasterBook()
            {
                BookName = "New_Book",
                BookAuthor = "New Author",
                CourseName = "MCA",
                PurchaseDate = DateTime.Now
            };

            var response = await obj.PostDataAsync(url, masterBook);
            if (response != null)
                return Convert.ToInt32(response);

            return 0;
        }
        static void Main(string[] args)
        {
            Program obj = new Program();
            var tokenTask = obj.Login();
            tokenTask.Wait();
            _token = tokenTask.Result;  // Use session to assign the token and use through out the application
            Console.WriteLine("Token="+_token+"\n");

            var bookDataTask = obj.GetBook();
            bookDataTask.Wait();
            Console.WriteLine("\n Total book records are= " + bookDataTask.Result.Count);

            var bookDataPostTask = obj.PostBookData();
            bookDataPostTask.Wait();
            Console.WriteLine("\n\n Book data post status= " + bookDataPostTask.Result);

            var bookData1Task = obj.GetBook();
            bookData1Task.Wait();
            Console.WriteLine("\n\n Total book records are= " + bookData1Task.Result.Count);

            Console.ReadKey();
        }
    }
}
