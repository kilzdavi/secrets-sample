using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStoreCore.Data;
using BookStoreCore.Models;

namespace BookStoreCore.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        //public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Book> Books { get; set; }


        //public IList<Book> Books { get;set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if(searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

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
                var pageSize = Configuration.GetValue("PageSize", 4);
                Books = await PaginatedList<Book>.CreateAsync(booksIQ.AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Publisher), pageIndex ?? 1, pageSize);
            }
        }
    }
}
