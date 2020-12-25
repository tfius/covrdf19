using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using covrd.Model;

namespace covrd
{
    public class IndexModel : PageModel
    {
        private readonly covrd.Model.PapContext _context;

        public IndexModel(covrd.Model.PapContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public int TotalRecords { get; set; }
        public int PageSize { get; set; } = 40;

        public PaginatedList<Paper> Papers { get; set; }

        //public IList<Paper> Paper { get;set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            NameSort = String.IsNullOrEmpty(sortOrder)? "Name" : "";

            if (pageIndex == null)
                pageIndex = 1;
            if (pageIndex< 1)
                pageIndex = 1;

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Paper> results = from s in _context.Papers select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                results = results.Where(s => s.Metadata.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                /*case "Title":
                    results = results.OrderBy(s => s.Version);
                    break; */
                default:
                    results = results.OrderBy(s => s.Metadata.Title);
                    break;
            }

            TotalRecords = results.Count();

            Papers = await PaginatedList<Paper>.CreateAsync(
                results.Include(x=>x.Metadata).AsNoTracking(), pageIndex ?? 1, PageSize);

        }
    }
}
