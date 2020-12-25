using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using covrd.Model;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using Microsoft.ML.Data;
using Microsoft.Extensions.ML;
using covrd.Extensions;
using System.Text.RegularExpressions;
using System.Text;

namespace covrd
{
    public class SemanticModel : PageModel
    {
        private readonly covrd.Model.PapContext _context;
        private readonly PredictionEnginePool<TextData, TransformedTextFeaturesData> _predictionEnginePool;

        public SemanticModel(covrd.Model.PapContext context, PredictionEnginePool<TextData, TransformedTextFeaturesData> predictionEnginePool)
        {
            _context = context;
            _predictionEnginePool = predictionEnginePool;
        }
        public string CurrentFilter { get; set; }

        public string WordQuery { get; set; } // need clean without + - and stuff otherwise regex in HTMLExtensions HighlightKeyWords will fail
        public int PageIndex { get; set; }
        //public Paper Paper { get; set; }
        public SemanticResultsDisplay Results { get; set; }

        public int PageSize { get; set; } = 10;
        public async Task OnGetAsync(string currentFilter, int? pageIndex, bool doLDA)
        {
            //IQueryable<Paper> results = from s in _context.Papers select s;
            if (String.IsNullOrEmpty(currentFilter)) return;
            CurrentFilter = currentFilter.Trim();
            if (pageIndex == null || !pageIndex.HasValue)
                pageIndex = 1;
            if (pageIndex.Value < 1)
                pageIndex = 1;

            PageIndex = pageIndex.Value;

            var searchResults = Find(Program.Papers, currentFilter);
            if (Results == null) Results = new SemanticResultsDisplay();

            _context.ExecutedSearchs.Add(new ExecutedSearch() { Text = currentFilter });
            _context.SaveChanges();

            Results.Refs.Clear();
            // https://github.com/dotnet/machinelearning/blob/1288d1dacf4588d29b56d8b05374504f15fc895c/docs/samples/Microsoft.ML.Samples/Dynamic/Transforms/Text/LatentDirichletAllocation.cs

            var samples = new List<TextData>();
            int startIdx = ((pageIndex ?? 1) - 1) * PageSize;
            int count = 0;
            Results.MaxPages = (searchResults.paperRefs.Count / PageSize) + 1;
            Results.StartAt = (startIdx + 1);


            //foreach (var p in searchResults.paperRefs)
            int countOccurances = 0;
            for (int pidx = startIdx; pidx < searchResults.paperRefs.Count; pidx++)
            {
                var p = searchResults.paperRefs[pidx];
                var paper = Program.Papers[p.paperIdx];
                var item = new SemanticRef();
                Results.Refs.Add(item);
                if (p.inTitle > 0 || p.inTokens > 0)
                {
                    item.Samples.Add(new SemanticData() { Data = new TextData() { Text = paper.Metadata.Title },
                        Tokens = null
                    });
                    countOccurances++;
                }

                item.PaperCountId = pidx + 1;
                item.PaperDbId = paper.PaperId;
                item.PaperTitle = paper.Metadata.Title;
                item.Score = p.score.ToString("0.00");

                string[] toks = "".Split(".");
                foreach (var abi in p.inAbstract)
                {
                    if (paper.Abstract[abi].Tokens != null && paper.Abstract[abi].Tokens.Length > 1)
                        toks = paper.Abstract[abi].Tokens.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    item.Samples.Add(new SemanticData()
                    {
                        Data = new TextData()
                        {
                            Text = paper.Abstract[abi].Text,
                            Section = paper.Abstract[abi].Section,
                        },
                        Tokens = toks
                    }); ;
                    countOccurances++;
                }
                foreach (var abi in p.inBody)
                {
                    if (paper.Body[abi].Tokens != null && paper.Body[abi].Tokens.Length > 1)
                        toks = paper.Body[abi].Tokens.Split(";", StringSplitOptions.RemoveEmptyEntries);

                    item.Samples.Add(new SemanticData() { Data = new TextData()
                    {   
                        Text = paper.Body[abi].Text,
                        Section = paper.Body[abi].Section
                    },
                        // Tokens = 
                        Tokens = toks
                    });
                    countOccurances++;
                }

                item.FeatureTokenNames = paper.Tokens
                    /*.Replace("(", "")
                    .Replace(")", "")
                    .Replace(",", "")
                    .Replace(".", "")
                    .Replace("[", "")
                    .Replace("]", "")*/
                    .Split(';').ToList();

                foreach (var d in item.Samples) // add all text to latentDirichletAlocation
                    samples.Add(d.Data);

                if (p.inTitle > 0 || p.inTokens > 0)
                    item.Samples.RemoveAt(0);

                count++;

                if (count >= PageSize)
                    break;
            }
            Results.AllOccurances = countOccurances++;

            var doResultsLDA = false;
            if (doResultsLDA)
            { 
                if (doLDA)
                {
                    foreach (var srd in Results.Refs)
                    {
                        foreach (var td in srd.Samples)// predict how much is this 
                        {
                            td.TokensFeatures = new List<TokenFeature>();
                            try
                            {
                                var LDAFeatures = _predictionEnginePool.Predict("LDAEstimatorModel", td.Data);
                                for (int i = 0; i < LDAFeatures.Features.Length && i < LDAFeatures.OutputTokens.Length; i++)
                                {
                                    if (LDAFeatures.Features[i] > 0 || i < 20)
                                        td.TokensFeatures.Add(new TokenFeature() { Feature = LDAFeatures.Features[i], Token = LDAFeatures.OutputTokens[i] });
                                }
                            }
                            catch (Exception e) { }
                            td.TokensFeatures = td.TokensFeatures.OrderByDescending(o => o.Feature).Take(30).ToList();
                        }
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var srd in Results.Refs)
                    {
                        foreach (var td in srd.Samples)// predict how much is this 
                        {
                            //sb.Append(td.Tokens.ConvertStringArrayToString() + "\n");
                            sb.Append(td.Data.Text + "\n");
                        }
                    }
                    TextData data = new TextData() { Text = sb.ToString() };

                    Results.SearchResultsTokenFeatures = new List<TokenFeature>();
                    try
                    {
                        var LDAFeatures = _predictionEnginePool.Predict("LDAEstimatorModel", data);
                        for (int i = 0; i < LDAFeatures.Features.Length && i < LDAFeatures.OutputTokens.Length; i++)
                        {
                            //if (LDAFeatures.Features[i] > 0 || i < 20)
                            Results.SearchResultsTokenFeatures.Add(new TokenFeature() {
                                Feature = LDAFeatures.Features[i],
                                Token = LDAFeatures.OutputTokens[i].Replace(",", "").Replace(";", "")
                            });
                        }

                        Results.SearchResultsTokenFeatures = Results.SearchResultsTokenFeatures.OrderByDescending(o => o.Feature).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Predicting Error " + e.Message);
                    }
                }
            }

            return;
        }

        public List<SemanticData> GetWordBags(TextData textData, PredictionEngine<TextData, TransformedTextData> bagOfWordsPredictionEngine, IDataView transformedDataView)
        {
            List<SemanticData> lsd = new List<SemanticData>();
            var prediction = bagOfWordsPredictionEngine.Predict(textData);

            VBuffer<ReadOnlyMemory<char>> slotNames = default;
            transformedDataView.Schema["BagOfWordFeatures"].GetSlotNames(ref slotNames);
            var BagOfWordFeaturesColumn = transformedDataView.GetColumn<VBuffer<float>>(transformedDataView.Schema["BagOfWordFeatures"]);
            var slots = slotNames.GetValues();
            foreach (var featureRow in BagOfWordFeaturesColumn)
            {
                foreach (var item in featureRow.Items())
                {
                   // lsd.Add(new SemanticData() { Data = new TextData() { Text = (slots[item.Key]).ToString() }, LDAFeatures = prediction });;
                }
            }
            /*for (int i = 0; i < 10; i++)
            {
                lsd[i].Topic = prediction.BagOfWordFeatures[i];
            }*/
            return lsd;
        }


        private PaperSearchResult Find(List<Paper> papers, string query)
        {
            var results = new PaperSearchResult();
            if (query == String.Empty) return results;

            WordQuery = "";

            var words = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            results.searchTerms = words.ToList();

            List<Paper> workingSet = new List<Paper>(papers);

            // remove those we don't want
            for(int i=words.Count-1;i>=0;i--) // go through all words in querry 
            {
                var q = words[i];
                var remove_c = (q.Length > 1 && (q[0] == '+' || q[0] == '-' || q[0] == '!' || q[0] == '='));
                var isAnd = (q.Length > 1 && q[0] == '+');
                var isNot = (q.Length > 1 && q[0] == '-');
                var isFeat = (q.Length > 1 && q[0] == '=');
                var isNotFeat = (q.Length > 1 && q[0] == '!');
                var isOr = (q.Length > 1 && q[0] != '-' && q[0] != '+');

                if (remove_c) 
                     q = words[i].Substring(1);

                {
                    if (!isNot) WordQuery += q + " ";

                    for (int w = workingSet.Count - 1; w >= 0; w--)
                    {
                        var pap = workingSet[w];
                        PaperRef PapRef = null;
                        if (isFeat || isNotFeat)
                        {
                            PapRef = FindFeatureText(pap, q);
                            if ((isFeat && PapRef.inTokens > 0) || (isNotFeat && PapRef.inTokens == 0))
                            {
                                var exist = results.paperRefs.FirstOrDefault(q => q.paperIdx == pap.Idx);
                                if (exist == null) // does not exits
                                {
                                    results.paperRefs.Add(PapRef);
                                }
                                else
                                { 
                                    exist.inAbstract = exist.inAbstract.Union(PapRef.inAbstract).ToList();
                                    exist.inBody = exist.inBody.Union(PapRef.inBody).ToList();
                                    exist.score += PapRef.score;
                                }
                            }
                            else
                            {
                                workingSet.RemoveAt(w);
                                RemovePaperRef(pap.Idx, ref results);
                                continue;
                            }
                        }

                        PapRef = FindHasText(pap, q);

                        if (PapRef == null) // has no text
                        {
                            if (!isOr)
                            {
                                workingSet.RemoveAt(w);
                                RemovePaperRef(pap.Idx, ref results);
                                continue;
                            }
                        }
                        else
                        {
                            var exist = results.paperRefs.FirstOrDefault(q => q.paperIdx == pap.Idx);
                            if (exist == null) // does not exits
                            {
                                if (!isNot) // add only if its add or or
                                    results.paperRefs.Add(PapRef);
                            }
                            else
                            {
                                if (isOr)// union of data
                                {
                                    exist.inAbstract = exist.inAbstract.Union(PapRef.inAbstract).ToList();
                                    exist.inBody = exist.inBody.Union(PapRef.inBody).ToList();
                                    exist.score += PapRef.score;
                                }
                                if(isAnd)
                                {
                                    if (exist.inAbstract.Count > 0)
                                        exist.inAbstract = exist.inAbstract.Intersect(PapRef.inAbstract).ToList();
                                    else
                                        exist.inAbstract = PapRef.inAbstract;
                                    if (exist.inBody.Count > 0)
                                       exist.inBody = exist.inBody.Intersect(PapRef.inBody).ToList();
                                    else
                                        exist.inBody = PapRef.inBody;

                                    exist.score += PapRef.score;
                                }
                                if (isNot)
                                {
                                    exist.inAbstract = exist.inAbstract.Except(PapRef.inAbstract).ToList();
                                    exist.inBody = exist.inBody.Except(PapRef.inBody).ToList();

                                    exist.score -= PapRef.score;
                                }
                            }
                        }
                    }
                }
            }

            results.paperRefs = results.paperRefs.OrderByDescending(o => o.score).ToList();
            return results;
        }
        private void RemovePaperRef(int idx, ref PaperSearchResult results)
        {
            //workingSet.RemoveAt(w);
            var exist = results.paperRefs.FirstOrDefault(q => q.paperIdx == idx);
            if (exist != null) // remove eixtsing 
                results.paperRefs.Remove(exist);
        }

        static private PaperRef FindFeatureText(Paper paper, string word) //, ref PaperSearchResult psr)
        {
            if (word == String.Empty)
                return null;

            var found = new PaperRef();
            found.paperIdx = paper.Idx;

            if (paper.Tokens.ContainsInsensitive(word))
            {
                found.inTokens = 1;
                found.score += 0.1;
            }

            if (found.inTokens > 0)
            {
                for (int i = 0; i < paper.Body.Count; i++)
                {
                    //if (paper.BodyText[i].Text.ContainsInsensitive(word))
                    if (paper.Body[i].Tokens.ContainsInsensitive(word))
                    {
                        found.inBody.Add((int)i);
                        found.score += 0.1;
                    }
                    //if (paper.BodyText[i].Text.ContainsInsensitive(word))
                    //{
                    //    found.inBody.Add((int)i);
                    //    found.score += 0.1;
                    //}
                }
            }
            return found;
        }


        static private PaperRef FindHasText(Paper paper, string word) //, ref PaperSearchResult psr)
        {
            if (word == String.Empty)
                return null;

            var found = new PaperRef();
            found.paperIdx = paper.Idx;

            if (paper.Metadata.Title.ContainsInsensitive(word))
            { 
                found.inTitle = 1;
                found.score += 0.1;
            }

            if (paper.Tokens.ContainsInsensitive(word))
            {
                found.inTokens = 1;
                found.score += 0.07;
            }

            for (int i = 0; i < paper.Abstract.Count; i++)
            {
                if (paper.Abstract[i].Text.ContainsInsensitive(word))
                {
                    found.inAbstract.Add((int)i);
                    found.score += 0.18;
                }
            }
            for (int i = 0; i < paper.Body.Count; i++)
            {
                if (paper.Body[i].Text.ContainsInsensitive(word))
                {
                    found.inBody.Add((int)i);
                    found.score += 0.33;
                }
            }

            // return if anything was found 
            if (found.inBody.Count > 0 || found.inAbstract.Count > 0 || found.inTitle > 0 || found.inTokens > 0)
                return found;

            return null;
        }
        static private PaperRef FindNoText(Paper paper, string word) //, ref PaperSearchResult psr)
        {
            if (word == String.Empty)
                return null;

            var found = new PaperRef();
            found.paperIdx = paper.Idx;

            if (paper.Metadata.Title.ContainsInsensitive(word))
                return null;

            if (paper.Tokens.ContainsInsensitive(word))
                return null;

            for (int i = 0; i < paper.Abstract.Count; i++)
            {
                if (paper.Abstract[i].Text.ContainsInsensitive(word))
                {
                    return null;
                }
            }
            for (int i = 0; i < paper.Body.Count; i++)
            {
                if (paper.Body[i].Text.ContainsInsensitive(word))
                {
                    return null;
                }
            }

            return found;
        }

        static private PaperSearchResult FindOldAll(List<Paper> papers, string[] words)
        {
            var results = new PaperSearchResult();
            if (words.Length < 1) return results;

            //results.searchTerms.Add(words.ConvertStringArrayToString());

            for (int p = 0; p < papers.Count; p++)
            {
                var found = new PaperRef();
                found.paperIdx = p;
                var paper = papers[p];
                /*
                if (FindX(paper.Metadata.Title, words))
                    found.inTitle = 1;

                //if (paper.Tokens.ContainsInsensitive(text))
                if (FindX(paper.Tokens, words))
                    found.inTokens = 1;

                for (int i = 0; i < paper.Abstract.Count; i++)
                {
                    if (FindX(paper.Abstract[i].Text, words))
                    {
                        found.inAbstract.Add((int)i);
                    }
                }
                for (int i = 0; i < paper.BodyText.Count; i++)
                {
                    if (FindX(paper.BodyText[i].Text, words))
                    {
                        found.inBody.Add((int)i);
                    }
                }
                
                if (paper.Metadata.Title.ContainsInsensitive(text))
                    found.inTitle = 1;

                if (paper.Tokens.ContainsInsensitive(text))
                    found.inTokens = 1;

                for (int i = 0; i < paper.Abstract.Count; i++)
                {
                    if (paper.Abstract[i].Text.ContainsInsensitive(text))
                    {
                        found.inAbstract.Add((int)i);
                    }
                }
                for (int i = 0; i < paper.BodyText.Count; i++)
                {
                    if (paper.BodyText[i].Text.ContainsInsensitive(text))
                    {
                        found.inBody.Add((int)i);
                    }
                }
                
                if (found.inBody.Count > 0 || found.inAbstract.Count > 0 || found.inTitle > 0 || found.inTokens > 0)
                    results.paperRefs.Add(found);

    */
            }

            return results; // $"Found in abstracts: {abstract_count} body: {body_count} records";
        }
    }
}
/*
                if (isOr) // might contain it
                {
                    for (int w = workingSet.Count - 1; w >= 0; w--)
                    {
                        var pap = workingSet[w];
                        var PapRef = FindHasText(pap, q);
                        if (PapRef == null) // has no text, remove from working set 
                        {
                            //workingSet.RemoveAt(w);
                        } else
                        {
                            var exist = results.paperRefs.FirstOrDefault(q => q.paperIdx == pap.Idx);
                            if (exist == null)
                            {
                                results.paperRefs.Add(PapRef);
                            } else
                            {
                                exist.inAbstract = exist.inAbstract.Union(PapRef.inAbstract).ToList();
                                exist.inBody = exist.inBody.Union(PapRef.inBody).ToList();
                            }
                        }
                    }
                }*/
