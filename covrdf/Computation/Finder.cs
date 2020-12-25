using covrdf.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace covrdf.Computation
{
    public class Finder
    {
        static public PaperSearchResult FindParallel(List<Paper> papers, string text)
        {
            var results = new PaperSearchResult();
            results.searchTerms.Add(text);

            //foreach (var paper in papers)
            Parallel.ForEach(papers, (paper, pls, index) =>
            {
                var found = new PaperRef();
                found.paper = (int)index;

                if (paper.Metadata.Title.Contains(text))
                    found.inTitle = 1;

                Parallel.ForEach(paper.Abstract, (prop, pls, index) =>
                {
                    if (prop.Text.Contains(text))
                    {
                        //lock (results)
                        {
                            found.inAbstract.Add((int)index);
                        }
                    }
                });
                Parallel.ForEach(paper.BodyText, (prop, pls, index) =>
                {
                    if (prop.Text.Contains(text))
                    {
                        //lock (results)
                        {
                            found.inBody.Add((int)index);
                        }
                    }
                });
                if (found.inBody.Count > 0 || found.inAbstract.Count > 0 || found.inTitle > 0)
                    results.paperRefs.Add(found);
            });

            return results;
        }
        static public PaperSearchResult Find(List<Paper> papers, string text)
        {
            var results = new PaperSearchResult();
            results.searchTerms.Add(text);

            for (int p = 0; p < papers.Count; p++)
            {
                var found = new PaperRef();
                found.paper = p;
                var paper = papers[p];

                if (paper.Metadata.Title.Contains(text))
                    found.inTitle = 1;

                for (int i=0;i< paper.Abstract.Count;i++)
                {
                    if (paper.Abstract[i].Text.Contains(text))
                    {
                        found.inAbstract.Add((int)i);
                    }
                }
                for (int i = 0; i < paper.BodyText.Count; i++)
                {
                    if (paper.BodyText[i].Text.Contains(text))
                    {
                        found.inBody.Add((int)i);
                    }
                }
                if (found.inBody.Count > 0 || found.inAbstract.Count > 0 || found.inTitle>0)
                    results.paperRefs.Add(found);
            }

            return results; // $"Found in abstracts: {abstract_count} body: {body_count} records";
        }
    }
}
