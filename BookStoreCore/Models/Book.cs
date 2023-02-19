using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace BookStoreCore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public Author Author { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
