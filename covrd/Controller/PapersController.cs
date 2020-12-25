using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using covrd.Model;
using Microsoft.Extensions.ML;

namespace covrd.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PapersController : ControllerBase
    {
        private readonly PapContext _context;
        private readonly PredictionEnginePool<TextData, TransformedTextFeaturesData> _predictionEnginePool;

        public PapersController(PapContext context, PredictionEnginePool<TextData, TransformedTextFeaturesData> predictionEnginePool)
        {
            _context = context;
            _predictionEnginePool = predictionEnginePool;
        }
        

        /// <summary>
        /// Get paper by paper index
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [Route("[action]/{pageIndex}")]
        [HttpGet]
        public async Task<ActionResult<Paper>> GetPaperIdx(int idx)
        {
            var paper = Program.Papers[idx]; //.Find(x => x.Idx == idx);

            //if (paper == null)
            //    paper = await _context.Papers.Where(x => x. == paperId).FirstOrDefaultAsync();

            if (paper == null)
            {
                return NotFound();
            }

            return paper;

            /*
            int PageSize = 1;
            if (pageIndex < 1) pageIndex = 1;

            IQueryable<Paper> results = from s in _context.Papers select s;
            var Papers = await PaginatedList<Paper>.CreateAsync(results
                .Include(x => x.Metadata)
                  .ThenInclude(y => y.Authors)
                .Include(x => x.Abstract)
                  .ThenInclude(y=>y.CiteSpans)
                .Include(x => x.Abstract)
                  .ThenInclude(y => y.RefSpans)
                .Include(x => x.BodyText)
                  .ThenInclude(y => y.CiteSpans)
                .Include(x => x.BodyText)
                  .ThenInclude(y => y.RefSpans)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref0.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref1.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref2.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref3.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref4.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref5.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref6.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref7.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref8.Authors)
                .Include(x => x.BibEntries)
                   .ThenInclude(y => y.Bibref9.Authors)
                .Include(x => x.BibEntries)
                    .ThenInclude(y => y.Bibref10.Authors)
                .Include(x => x.BibEntries)
                    .ThenInclude(y => y.Bibref11.Authors)
                .Include(x => x.BibEntries)
                    .ThenInclude(y => y.Bibref12.Authors)
                .Include(x => x.BibEntries)
                    .ThenInclude(y => y.Bibref13.Authors)
                .Include(x => x.BibEntries)
                    .ThenInclude(y => y.Bibref14.Authors)
                .Include(x => x.BibEntries)
                    .ThenInclude(y => y.Bibref15.Authors) 

                .Include(x => x.Comments)
                .Include(x => x.RefEntries)
                .AsNoTracking(), pageIndex, PageSize);
            return Papers;*/
        }
        
        /// <summary>
        /// Get num of papers in db
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<int>> GetPapersCount()
        {
            return Program.Papers.Count;
            //return _context.Papers.Count();
        }

        /// <summary>
        /// Get paper by paperId
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        [HttpGet("{PaperId}")]
        public async Task<ActionResult<Paper>> GetPaper(string paperId)
        {
            var paper =  Program.Papers.Find(x => x.PaperId == paperId);

            if (paper == null)
                paper = await _context.Papers.Where(x => x.PaperId == paperId).FirstOrDefaultAsync();

            if (paper == null)
            {
                return NotFound();
            }

            return paper;
        }

        /// <summary>
        /// Run Latent Diricleht Allocation Estimator on dataText, this will run prediciton engine trained on corpus of CORD-19 documents
        /// and return detected topics
        /// </summary>
        /// <param name="dataText"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TokenFeature>>> AnalyseText(TextData dataText)
        {
            var tfs = new List<TokenFeature>();
            try
            {
                var LDAFeatures = _predictionEnginePool.Predict("LDAEstimatorModel", dataText);
                for (int i = 0; i < LDAFeatures.Features.Length && i < LDAFeatures.OutputTokens.Length; i++)
                {
                     tfs.Add(new TokenFeature() { Feature = LDAFeatures.Features[i], Token = LDAFeatures.OutputTokens[i] });
                }
            }
            catch (Exception e) { }

            tfs = tfs.OrderByDescending(o => o.Feature).ToList();
            return tfs;
        }

        // PUT: api/Papers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /* [HttpPut("{id}")]
         public async Task<IActionResult> PutPaper(long id, Paper paper)
         {
             if (id != paper.Id)
             {
                 return BadRequest();
             }

             _context.Entry(paper).State = EntityState.Modified;

             try
             {
                 await _context.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!PaperExists(id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }

             return NoContent();
         }*/

        // POST: api/Papers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /* [HttpPost]
         public async Task<ActionResult<Paper>> PostPaper(Paper paper)
         {
             _context.Papers.Add(paper);
             await _context.SaveChangesAsync();

             return CreatedAtAction("GetPaper", new { id = paper.Id }, paper);
         }

         // DELETE: api/Papers/5
         [HttpDelete("{id}")]
         public async Task<ActionResult<Paper>> DeletePaper(long id)
         {
             var paper = await _context.Papers.FindAsync(id);
             if (paper == null)
             {
                 return NotFound();
             }

             _context.Papers.Remove(paper);
             await _context.SaveChangesAsync();

             return paper;
         }*/

        private bool PaperExists(long id)
        {
            return _context.Papers.Any(e => e.Id == id);
        }
    }
}
