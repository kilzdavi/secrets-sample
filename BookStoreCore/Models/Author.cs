using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreCore.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255, ErrorMessage = $"First name must be less than 255 characters. ")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(255, ErrorMessage = $"Last name must be less than 255 characters. ")]
        public string LastName { get; set; }
        [NotMapped]
        public string AuthorInitials
        {
            get
            {
                return this.FirstName.Substring(0, 1) + this.LastName.Substring(0, 1);
            }
        }

        public ICollection<Book> Books { get; set; }
    }
}
