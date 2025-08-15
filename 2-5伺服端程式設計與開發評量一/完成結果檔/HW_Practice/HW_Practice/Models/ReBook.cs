using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuestBooks.Models
{
    public partial class ReBook
    {
        public string ReBookID { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;


        public string BookID { get; set; } = null!;  //外鍵，指向主文資料表的BookID

        public Book? Book { get; set; } 


    }

}
