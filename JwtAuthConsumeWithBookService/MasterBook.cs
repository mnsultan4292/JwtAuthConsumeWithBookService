
using System;

namespace JwtAuthConsumeWithBookService
{
    public class MasterBook
    {
        public int BookId { get; set; }
        public string BookName { get; set; } 
        public string BookAuthor { get; set; }
        public string CourseName { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
