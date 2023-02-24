using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStoreCore.Data;
using BookStoreCore.Models;

namespace BookStoreCore.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly BookStoreCore.Data.ApplicationDbContext _context;

        public IndexModel(BookStoreCore.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IList<Book> Books { get;set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            IQueryable<Book> booksIQ = from b in _context.Books
                                      select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                booksIQ = booksIQ.Where(b => b.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    booksIQ = booksIQ.OrderByDescending(b => b.Title);
                    break;
                default:
                    booksIQ = booksIQ.OrderBy(b => b.Title);
                    break;
            }

            if (_context.Books != null)
            {
                Books = await booksIQ.AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Publisher).ToListAsync();
            }
        }
    }
}
