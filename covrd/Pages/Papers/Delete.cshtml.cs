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
    public class DeleteModel : PageModel
    {
        private readonly covrd.Model.PapContext _context;

        public DeleteModel(covrd.Model.PapContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Paper Paper { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Paper = await _context.Papers.FirstOrDefaultAsync(m => m.PaperId == id);

            if (Paper == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Paper = _context.Papers
                .Include(x => x.Abstract)
                //  .ThenInclude(y => y.CiteSpans)
                .Include(x => x.Abstract)
                //  .ThenInclude(y => y.RefSpans)
                //.Include(x => x.BibEntries)

                .Include(x => x.Body)
                //  .ThenInclude(y => y.CiteSpans)
                .Include(x => x.Body)
                //  .ThenInclude(y => y.RefSpans)

                //.Include(x => x.Comments)
                .Include(x => x.Metadata)
                //.Include(x => x.RefEntries)
                .Where(x => x.Id == id).FirstOrDefault();
                //.FindAsync(id);

            if (Paper != null)
            {
                _context.Papers.Remove(Paper);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
