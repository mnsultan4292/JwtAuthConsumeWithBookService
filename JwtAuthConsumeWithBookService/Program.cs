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

        public async Task<int> DeleteBook(int bookId)
        {
            url =String.Format(ConfigurationManager.AppSettings["DeleteBook"].ToString(),bookId);
            BookHttpClient<MasterBook> obj = new BookHttpClient<MasterBook>();
            return await obj.DeleteAsync(url, bookId);
        }

        static void Main(string[] args)
        {
            Program obj = new Program();

            var tokentask = obj.Login();
            tokentask.Wait();
            _token = tokentask.Result;  // use session to assign the token and use through out the application
            Console.WriteLine("token=" + _token + "\n");

            GetBookDetails();

            var bookDataPostTask = obj.PostBookData();
            bookDataPostTask.Wait();
            Console.WriteLine("\n\n Book data post status= " + bookDataPostTask.Result);
            
            GetBookDetails();

            var deleteTask = obj.DeleteBook(14);
            deleteTask.Wait();
            Console.WriteLine("\n\n Delete Book records status= "+deleteTask.Result);

            GetBookDetails();

            Console.ReadKey();
        }

        private static void GetBookDetails()
        {
            Program obj = new Program();
            var bookDataTask = obj.GetBook();
            bookDataTask.Wait();
            Console.WriteLine("\n\n Total book records are= " + bookDataTask.Result.Count);
        }
    }
}
