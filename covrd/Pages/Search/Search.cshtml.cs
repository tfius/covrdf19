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
    public class SearchModel : PageModel
    {
        private readonly covrd.Model.PapContext _context;

        public SearchModel(covrd.Model.PapContext context)
        {
            _context = context;
        }
        public string CurrentFilter { get; set; }
        //public Paper Paper { get; set; }
        public ResultsDisplay Results { get; set; }

        public int PageSize { get; set; } = 40;
        public async Task  OnGetAsync(string currentFilter)
        {
            IQueryable<Paper> results = from s in _context.Papers select s;
            if (String.IsNullOrEmpty(currentFilter)) return;
            CurrentFilter = currentFilter.Trim();

            /*
            var allPapers = await _context.Papers
                    .Include(x => x.Metadata)
                    //.ThenInclude(x => x.Authors)
                    .Include(x => x.Abstract) //.Where(y=>y.Text.Contains(currentFilter))
                     //.ThenInclude(x => x.CiteSpans)
                     //.Include(x => x.Abstract)
                     // .ThenInclude(x => x.RefSpans)
                    .Include(x => x.BodyText)// .Where(x => x.Text.Contains(currentFilter))
                     //  .ThenInclude(x => x.CiteSpans)
                     //.Include(x => x.BodyText)
                     //  .ThenInclude(x => x.RefSpans)
                    //.Include(x => x.BibEntries)
                    //.Include(x => x.RefEntries)
                    .Include(x => x.Comments).AsNoTracking().ToListAsync();
                    */

            var searchResults = Find(Program.Papers, currentFilter);
            if (Results == null) Results = new ResultsDisplay();

            Results.Refs.Clear();


            foreach(var p in searchResults.paperRefs)
            {
                var paper = Program.Papers[p.paperIdx];
                var item = new PaperDisplayRef();
                Results.Refs.Add(item);
                if(p.inTitle>0)
                    item.Texts.Add(paper.Metadata.Title);

                item.PaperDbId = paper.PaperId;
                item.PaperTitle = paper.Metadata.Title;

                foreach (var abi in p.inAbstract)
                {
                    item.Texts.Add(paper.Abstract[abi].Text);
                }
                foreach (var abi in p.inBody)
                {
                    item.Texts.Add(paper.Body[abi].Text);
                }


                //Papers.Add(allPapers[p.paper]);
            }
            /*
            Papers = await PaginatedList<Paper>.CreateAsync(
                results.Include(x => x.Metadata).AsNoTracking(), 1, PageSize);
                */

            return;
        }

        static private PaperSearchResult Find(List<Paper> papers, string text)
        {
            var results = new PaperSearchResult();
            results.searchTerms.Add(text);

            for (int p = 0; p < papers.Count; p++)
            {
                var found = new PaperRef();
                var paper = papers[p];
                found.paperIdx = paper.Idx;

                if (paper.Metadata.Title.Contains(text))
                    found.inTitle = 1;

                if (paper.Tokens.Contains(text))
                    found.inTokens = 1;

                for (int i = 0; i < paper.Abstract.Count; i++)
                {
                    if (paper.Abstract[i].Text.Contains(text))
                    {
                        found.inAbstract.Add((int)i);
                    }
                }
                for (int i = 0; i < paper.Body.Count; i++)
                {
                    if (paper.Body[i].Text.Contains(text))
                    {
                        found.inBody.Add((int)i);
                    }
                }

                if (found.inBody.Count > 0 || found.inAbstract.Count > 0 || found.inTitle > 0 || found.inTokens > 0)
                    results.paperRefs.Add(found);
            }

            return results; // $"Found in abstracts: {abstract_count} body: {body_count} records";
        }
    }
}
