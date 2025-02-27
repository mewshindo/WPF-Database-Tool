using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Database_Tool
{
    public class Book
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Author { get; set; }
        public int PublishYear { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int Genre { get; set; }


        public Author AuthorClass { get; set; } = null!;
        public Genre GenreClass { get; set; } = null!;
    }
}
