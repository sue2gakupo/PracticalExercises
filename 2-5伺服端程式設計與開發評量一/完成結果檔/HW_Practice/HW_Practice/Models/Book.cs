using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuestBooks.Models
{
    public partial class Book 
    {

        public string BookID { get; set; } = null!; 

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Photo { get; set; }   

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public virtual List<ReBook>? ReBooks { get; set; } 

    }
}
