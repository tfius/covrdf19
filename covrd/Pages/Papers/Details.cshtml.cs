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
    public class DetailsModel : PageModel
    {
        private readonly covrd.Model.PapContext _context;

        public DetailsModel(covrd.Model.PapContext context)
        {
            _context = context;
        }

        public Paper Paper { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Paper = await _context.Papers
                .Include(x=>x.Metadata)
                .ThenInclude(x => x.Authors)
                .Include(x => x.Abstract)
                 .ThenInclude(x => x.Cites)
                .Include(x => x.Abstract)
                //  .ThenInclude(x => x.RefSpans)
                .Include(x => x.Body)
                   .ThenInclude(x => x.Cites)
                .Include(x => x.Body)
                //   .ThenInclude(x => x.RefSpans)
                //.Include(x => x.BibEntries)
                //.Include(x => x.RefEntries)
                //.Include(x => x.Comments)
                .FirstOrDefaultAsync(m => m.PaperId == id);

            if (Paper == null)
            {
                Paper = Program.Papers.Find(x => x.PaperId == id);

                if (Paper == null)
                    return NotFound();
            }
            return Page();
        }
    }
}
