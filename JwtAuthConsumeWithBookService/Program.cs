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
    public class Program
    {
        public static string _token;
        public string url = string.Empty;
        
        public async Task<List<MasterBook>> GetBook(string token)
        {
            BookHttpClient<MasterBook> obj = new BookHttpClient<MasterBook>();
            url = ConfigurationManager.AppSettings["GetBookUrl"].ToString();
            return await obj.GetDataAsync(url,token);
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
        public async Task<int> PostBookData(string token)
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

            var response = await obj.PostDataAsync(url, masterBook,token);
            if (response != null)
                return Convert.ToInt32(response);

            return 0;
        }

        public async Task<int> DeleteBook(int bookId,string token)
        {
            url =String.Format(ConfigurationManager.AppSettings["DeleteBook"].ToString(),bookId);
            BookHttpClient<MasterBook> obj = new BookHttpClient<MasterBook>();
            return await obj.DeleteAsync(url, bookId,token);
        }

        static void Main(string[] args)
        {
            Program obj = new Program();

            var tokentask = obj.Login();
            tokentask.Wait();
            _token = tokentask.Result;  // use session to assign the token and use through out the application
            Console.WriteLine("token=" + _token + "\n");           

            GetBookDetails(_token);

            var bookDataPostTask = obj.PostBookData(_token);
            bookDataPostTask.Wait();
            Console.WriteLine("\n\n Book data post status= " + bookDataPostTask.Result);

            GetBookDetails(_token);

            //var deleteTask = obj.DeleteBook(14,_token);
            //deleteTask.Wait();
            //Console.WriteLine("\n\n Delete Book records status= " + deleteTask.Result);

            //GetBookDetails(_token);

            Console.ReadKey();
        }
        private static void GetBookDetails(string token)
        {
            Program obj = new Program();
            var bookDataTask = obj.GetBook(token);
            bookDataTask.Wait();
            Console.WriteLine("\n\n Total book records are= " + bookDataTask.Result.Count);
        }
    }
}
