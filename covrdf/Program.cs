using covrdf.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using covrdf.Computation;
using System.Threading.Tasks;
//using OpenNLP.Tools.SentenceDetect;
//using OpenNLP.Tools.Tokenize;
//using OpenNLP.Tools.PosTagger;
//using OpenNLP.Tools.Lang.English;
//using OpenNLP.Tools.Coreference;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using covrdf.Model;
using Microsoft.ML.Transforms.Text;
using System.Text;
using covrdf.NERModel;

namespace covrdf
{
    class Program
    {
        public static Dictionary<string, string> metadataMap = new Dictionary<string, string>();
        public static Dictionary<string, Paper> papersMap = new Dictionary<string, Paper>();
        public static List<Paper> papers = null;
        public static List<MetadataOverview> metadata = null;

        public static List<Annotation> nerModel = null;

        static string currentDirectory = Environment.CurrentDirectory;// + "/../../";
        static string sdModelFilePath = currentDirectory + "/Resources/Models/EnglishSD.nbin"; // sentence dectecor
        static string tokModelFilePath = currentDirectory + "/Resources/Models/EnglishTok.nbin"; // tokenizer
        static string posModelFilePath = currentDirectory + "/Resources/Models/EnglishPOS.nbin"; // pos tagger
        static string tagdictDir = currentDirectory + "/Resources/Models/Parser/tagdict"; // pos tagger
        static string tblModelPath = currentDirectory + "/Resources/Models/Coref/"; // TreebankLinker

        static readonly string _clusteringModelPath = Path.Combine(Environment.CurrentDirectory, "Data", "custClusteringModel.zip");
        static Stopwatch stopWatch = new Stopwatch();
        static IConfiguration config;
        static string path = "";
        static string[] folders;

        static public void WritePapersFeatures(List<PaperFeatures> paperTokenFeatures)
        {
            #region write papers features combined
            stopWatch.Restart();
            path = config["root"] + config["features"];
            Console.Write($"Writing {path}");
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, paperTokenFeatures);
            }
            Console.WriteLine($" completed in {stopWatch.Elapsed}");
            #endregion
        }
        static public void WritePapers(List<Paper> papersToWrite)
        {
            #region write papers combined
            stopWatch.Restart();
            path = config["root"] + config["combined"];
            Console.Write($"Writing {path}");
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, papers);
            }
            
            Console.WriteLine($" completed in {stopWatch.Elapsed}");
            Console.WriteLine($" Copying {config["combined"]} to covrd project");
            var covrdPath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\covrd\\Resources\\" + config["combined"];
            Console.WriteLine($" {path} copy to {covrdPath}");

            File.Copy(path, covrdPath, true);

            #endregion
        }
        static public List<Paper> ReadPapersFromFiles()
        {
            var papersList = new List<Paper>();
            stopWatch.Restart();
            int count = 0;
            foreach (var foldername in folders)
            {
                path = config["root"] + foldername;
                Console.SetCursorPosition(0, 7);
                Console.WriteLine($"reading papers {path}");
                try
                {
                    string[] dirs = Directory.GetFiles(path, "*.json");
                    foreach (var file in dirs)
                    {
                        var filename = Path.GetFileNameWithoutExtension(file);
                        //if (cvsMap.ContainsKey(filename))
                        {
                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine($"serializing {count++} {file}   ");

                            string jsonString = File.ReadAllText(file);
                            var paper = Paper.FromJson(jsonString);
                            paper.Clean();
                            papersList.Add(paper);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"* {foldername} : {e.Message}");
                }
            }
            Console.WriteLine($"serialized {papersList.Count} papers in {stopWatch.Elapsed}");
            return papersList;
        }
        static public void MapPapers()
        {
            #region mapping papers
            stopWatch.Restart();
            Console.Write("mapping papers");
            foreach (var paper in papers)
                papersMap.Add(paper.PaperId, paper);

            Console.WriteLine($" {papersMap.Count} in {stopWatch.Elapsed}");
            #endregion
        }
        static public List<Paper> ReadCombinedPapers()
        {
            #region read combined
            try
            {
                var papersList = new List<Paper>();
                stopWatch.Restart();
                path = config["root"] + config["combined"];
                Console.Write($"reading combined {path}");
                string combinedJson = File.ReadAllText(path);
                papersList = JsonConvert.DeserializeObject<List<Paper>>(combinedJson, PaperConverter.Settings);
                Console.WriteLine($" completed in {stopWatch.Elapsed}");
                return papersList;
            }
            catch (Exception e)
            {
                Console.WriteLine($" rebuild required");
            }
            return null;
            #endregion
        }

        static public void MetadataMapping()
        {
            #region map
            stopWatch.Restart();
            Console.Write($"mapping");
            foreach (var r in metadata) // include only those with proper sha
            {
                //if(!cvsMap.ContainsKey(r.sha))
                //    cvsMap.Add(r.sha, r.Title );
                if (!metadataMap.ContainsKey(r.Title))
                     metadataMap.Add(r.Title, r.sha);
            }
            Console.WriteLine($" mapped {metadataMap.Count} in {stopWatch.Elapsed}");
            #endregion
        }
        static public void MetadataParse()
        {
            #region metadata parse
            if (metadata == null)
            {
                stopWatch.Restart();
                path = config["root"] + config["metadata"];
                Console.WriteLine($"reading {path}");
                Console.WriteLine($"parsing {path}");
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                metadata = fs.CsvToList<MetadataOverview>(metadataMap);
                Console.WriteLine($" {metadata.Count} in {stopWatch.Elapsed}");

                #region write metadata 
                stopWatch.Restart();
                path = config["root"] + config["metastore"];
                Console.WriteLine($"Writing {path}");
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, metadata);
                }
                Console.WriteLine($" completed in {stopWatch.Elapsed}");
                #endregion
            }
            #endregion
        }

        static public void MetadataReadCombined()
        {
            #region metadata read combined
            try
            {
                stopWatch.Restart();
                path = config["root"] + config["metastore"];
                Console.Write($"reading metadata {path}");
                string json = File.ReadAllText(path);
                metadata = JsonConvert.DeserializeObject<List<MetadataOverview>>(json, PaperConverter.Settings);
                Console.WriteLine($" completed in {stopWatch.Elapsed}");
            }
            catch (Exception e)
            {
                Console.WriteLine($" parse required");
            }
            #endregion
        }
        static public void ReadNER()
        {
            #region read NER
            try
            {
                stopWatch.Restart();
                nerModel = new List<Annotation>();
                path = config["root"] + config["ner"];
                Console.WriteLine($"reading ner {path}");

                int counter = 0;
                string line;

                System.IO.StreamReader file =new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    var ann = JsonConvert.DeserializeObject<Annotation>(line, NERConverter.Settings);
                    nerModel.Add(ann);
                    counter++;
                }

                file.Close();
                Console.WriteLine($"completed in {stopWatch.Elapsed}");
                Console.WriteLine($"NER doc count {nerModel.Count}");
                StructureNER();
            }
            catch (Exception e)
            {
                Console.WriteLine($"NER parse failed " + path);
                Console.WriteLine($"skipping NER");
            }
            #endregion

            

            
        }

        static public void StructureNER()
        {
            Console.WriteLine($" StructureNER {nerModel.Count}");
            var dict = new Dictionary<string, EntityInfo>();
            foreach(var a in nerModel)
            {
                foreach(var s in a.Sents) // get 
                {
                    foreach (var e in s.Entities) // get 
                    {
                        if (dict.ContainsKey(e.Type))
                        {
                            var dt = dict[e.Type];
                            dt.UniqueText.Add(e.Text);
                            dt.Texts.Add(e.Text);
                            dt.DocIds.Add(a.DocId);
                        }
                        else
                        {
                            var ei = new EntityInfo() { TypeName = e.Type, DocIds = new HashSet<long>(), Texts = new List<string>(), UniqueText= new HashSet<string>() };
                            ei.UniqueText.Add(e.Text);
                            ei.Texts.Add(e.Text);
                            ei.DocIds.Add(a.DocId);
                            dict.Add(e.Type, ei); 
                        }
                    }
                }
            }

            foreach (var item in dict)
            {
                Console.Write(item.Key);
                Console.WriteLine($" :{item.Value.Texts.Count}  {item.Value.UniqueText.Count} in {item.Value.DocIds.Count} docs");
            }
        }

        static async Task Main(string[] args)
        {
            #region configuration
            
            stopWatch.Start();
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();
            
            folders = config.GetSection("folders")
                                .GetChildren()
                                .Select(x => x.Value)
                                .ToArray();

            #endregion

            #region STEP 1, load metadata, map, read combined or combine papers into fused form
            #region metadata

            #region metadata read combined
            MetadataReadCombined();
            #endregion

            #region metadata parse
            MetadataParse();
            #endregion

            #endregion

            #region metadata mapping
            MetadataMapping();
            #endregion

            ReadNER();

            #region read papers, first try from combined file, else from folders
            papers = ReadCombinedPapers();
            if (papers == null)
            {
                papers = ReadPapersFromFiles();
                WritePapers(papers);
            }
            // else  Console.WriteLine("Skipping reading files and writing combined");
            #endregion

            Console.WriteLine("Step 1: Read files process completed");
            #endregion

            MapPapers();

            // so now we have papers, papersMap, cvsMap
            int ystart = 12;
            var g = new Graph();

            /*
            var posTagger = new EnglishMaximumEntropyPosTagger(posModelFilePath, tagdictDir);
            var coreferenceFinder = new TreebankLinker(tblModelPath, LinkerMode.Test);
            var sentenceDetector = new EnglishMaximumEntropySentenceDetector(sdModelFilePath);
            var tokenizer = new EnglishMaximumEntropyTokenizer(tokModelFilePath);
            var nlpProcessor = new NLPProcess(sentenceDetector, tokenizer, posTagger, coreferenceFinder);
            */

            #region sample search
            //stopWatch.Restart();
            //var res1 = Finder.Find(papers, "Antibody");
            //Console.WriteLine($" with for: found {res1.paperRefs.Count} papers in {stopWatch.Elapsed}");

            //stopWatch.Restart();
            //var res2 = Finder.FindParallel(papers, "interleukin-6");
            //Console.WriteLine($" parallel: found {res2.paperRefs.Count} papers in {stopWatch.Elapsed}");
            #endregion



            //string pathToModel = currentDirectory + "/lda_predictionengine_abstract.zip";
            string modelName = "lda_predictionengine_all.zip";
            string pathToModel = currentDirectory + "/" + modelName;
            bool   trainLDA    = true; // re-build model
            bool   buildEstimatorResults = true; // add estimator results to combined data 

            #region Train LatentDirichletAllocation Estimator
            if (trainLDA)
            {
                #region Prepare data for training
                Console.WriteLine("Prepare data samples for training");
                var featuresOptions = new TextFeaturizingEstimator.Options()
                {
                    // Also output tokenized words
                    OutputTokensColumnName = "OutputTokens",
                    //KeepDiacritics = false,
                    //KeepNumbers = false,
                    //KeepPunctuations = false,
                    CaseMode = TextNormalizingEstimator.CaseMode.Lower,
                    // Use ML.NET's built-in stop word remover
                    StopWordsRemoverOptions = new StopWordsRemovingEstimator.Options()
                    {
                        Language = TextFeaturizingEstimator.Language.English
                    },

                    WordFeatureExtractor = new WordBagEstimator.Options()
                    {
                        NgramLength = 3,
                        //SkipLength = 3,
                        UseAllLengths = true
                    },

                    CharFeatureExtractor = new WordBagEstimator.Options()
                    {
                        NgramLength = 4,
                        UseAllLengths = false
                    }
                };
                Console.WriteLine("preprocessing samples");
                var samples = new List<TextData>();
                foreach (var paper in papers)
                {
                    samples.Add(new TextData() { Text = /*paper.GetCombinedText()*/ paper.GetBodyText() });
                    /*
                    samples.Add(new TextData() { Text = paper.Metadata.Title } );

                    foreach (var abi in paper.Abstract)
                        samples.Add(new TextData() { Text = abi.Text});

                    foreach (var abi in paper.BodyText)
                        samples.Add(new TextData() { Text = abi.Text }); */
                }
                #endregion
                Console.WriteLine("Done preprocessing data samples");
                //papers.Clear(); // clear to reduce mem consumption
                var mlContext = new MLContext();
                // https://github.com/dotnet/machinelearning/blob/1288d1dacf4588d29b56d8b05374504f15fc895c/docs/samples/Microsoft.ML.Samples/Dynamic/Transforms/Text/LatentDirichletAllocation.cs

                Console.WriteLine("Loading data");
                var dataview = mlContext.Data.LoadFromEnumerable(samples);

                Console.WriteLine("Transforming");
                var pipeline = mlContext.Transforms.Text.NormalizeText("NormalizedText", "Text", caseMode:TextNormalizingEstimator.CaseMode.Lower/*, keepNumbers:false, keepPunctuations:false, keepDiacritics:false*/)
                                        .Append(mlContext.Transforms.Text.TokenizeIntoWords("Tokens", "NormalizedText"))
                                        .Append(mlContext.Transforms.Text.RemoveDefaultStopWords("Tokens", language: StopWordsRemovingEstimator.Language.English))
                                        .Append(mlContext.Transforms.Conversion.MapValueToKey("Tokens"))
                                        .Append(mlContext.Transforms.Text.ProduceNgrams("Tokens"))
                                        .Append(mlContext.Transforms.Text.FeaturizeText("Features", featuresOptions, "Text"))
                                        .Append(mlContext.Transforms.Text.LatentDirichletAllocation(
                                            "Features", "Tokens", 
                                            numberOfTopics: 100, //100, 
                                            maximumNumberOfIterations: 200, //200,
                                            likelihoodInterval: 5,
                                            maximumTokenCountPerDocument : 384 //384
                                            ));

                Console.WriteLine("Training (this might take some time}");
                var trainedModel = pipeline.Fit(dataview);
                var LDApredictionEngine = mlContext.Model.CreatePredictionEngine<TextData, TransformedTextFeaturesData>(trainedModel);
                // save 
                Console.WriteLine($"Saving {pathToModel}");
                mlContext.Model.Save(trainedModel, dataview.Schema, pathToModel);
                Console.WriteLine("Trained LatentDirichletAllocation");

                var covrdMLPath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\covrd\\ML\\" + modelName;
                Console.WriteLine($" {pathToModel} copy to {covrdMLPath}");

                File.Copy(pathToModel, covrdMLPath);
            }
            #endregion

            Console.Write($"Path To Model: {pathToModel}");


            #region Run Estimator 
            if (buildEstimatorResults)
            {
                var mlContext = new MLContext();
                DataViewSchema modelSchema;
                ITransformer trainedModel = mlContext.Model.Load(pathToModel, out modelSchema);
                var LDApredictionEngine   = mlContext.Model.CreatePredictionEngine<TextData, TransformedTextFeaturesData>(trainedModel);

                var paperTokenFeatures = new List<PaperFeatures>();
                for(int idx = 0;idx<papers.Count;idx++)
                {
                    var paper = papers[idx];
                    var combinedText = new TextData() { Text = paper.GetCombinedText() };
                    try
                    {
                        papers[idx].Tokens = "";
                        papers[idx].Features = "";

                        try
                        {
                            var LDAFeatures = LDApredictionEngine.Predict(combinedText);

                            var TokensFeatures = new List<TokenFeature>();
                            if (LDAFeatures.Features != null && LDAFeatures.OutputTokens != null)
                            {
                                for (int i = 0; i < LDAFeatures.Features.Length && i < LDAFeatures.OutputTokens.Length; i++)
                                {
                                    if (LDAFeatures.OutputTokens[i].Length < 2) continue;

                                    //if (LDAFeatures.Features[i] > 0)
                                    TokensFeatures.Add(new TokenFeature() { F = LDAFeatures.Features[i], T = LDAFeatures.OutputTokens[i] });
                                }
                            }
                            TokensFeatures = TokensFeatures.OrderByDescending(o => o.F)/*.Take(20)*/.ToList();

                            //var paperFeatures = new PaperFeatures() { idx = idx, TFs = TokensFeatures };
                            StringBuilder tsb = new StringBuilder();
                            StringBuilder fsb = new StringBuilder();
                            for (int itf = 0; itf < TokensFeatures.Count; itf++)
                            {
                                var tf = TokensFeatures[itf];
                                tsb.Append(tf.T.Replace("(", "").Replace(")", "").Replace(",", "").Replace(".", "").Replace("[", "").Replace("]", ""));
                                fsb.Append(tf.F.ToString("0.000"));
                                if (itf != TokensFeatures.Count - 1)
                                {
                                    tsb.Append(";");
                                    fsb.Append(";");
                                }
                            }
                            var paperFeatures = new PaperFeatures() { idx = idx, T = tsb.ToString(), F = fsb.ToString() };

                            papers[idx].Tokens = tsb.ToString();
                            papers[idx].Features = fsb.ToString();
                            //Console.WriteLine($"{idx} {paper.PaperId} features {TokensFeatures.Count}");

                            paperTokenFeatures.Add(paperFeatures);
                        } catch(Exception ep)
                        {
                            Console.WriteLine($"{idx} {paper.PaperId} fail features");
                        }

                        try
                        {
                            for (int i = 0; i < paper.Abstract.Count; i++)
                            {
                                //Console.WriteLine($"{i} abstract {paper.Abstract[i].Section}");
                                var text = new TextData() { Text = paper.Abstract[i].Text };
                                paper.Abstract[i].Tokens = "";

                                var TF = LDApredictionEngine.Predict(text);
                                var TFL = new List<TokenFeature>();
                                if (TF.Features != null && TF.OutputTokens != null)
                                {
                                    for (int tf = 0; tf < TF.Features.Length && tf < TF.OutputTokens.Length; tf++)
                                    {
                                        if (TF.OutputTokens[tf].Length <= 2) continue;

                                        //if (TF.Features[tf] > 0)
                                        TFL.Add(new TokenFeature() { F = TF.Features[tf], T = TF.OutputTokens[tf] });
                                    }
                                }
                                TFL = TFL.OrderByDescending(o => o.F).Take(20).ToList();
                                StringBuilder ttsb = new StringBuilder();
                                for (int itf = 0; itf < TFL.Count; itf++)
                                {
                                    var tf = TFL[itf];
                                    ttsb.Append(tf.T.Replace("(", "").Replace(")", "").Replace(",", "").Replace(".", "").Replace("[", "").Replace("]", ""));
                                    if (itf != TFL.Count - 1)
                                        ttsb.Append(";");
                                }
                                paper.Abstract[i].Tokens = ttsb.ToString();
                            }
                        } catch(Exception ea)
                        {
                            Console.WriteLine($"{idx} abstract FAIL {ea.Message}");
                        }

                        try
                        {
                            for (int i = 0; i < paper.BodyText.Count; i++)
                            {
                                //Console.WriteLine($"{i} text {paper.BodyText[i].Section}"); 

                                var text = new TextData() { Text = paper.BodyText[i].Text };
                                paper.BodyText[i].Tokens = "";
                                var TF = LDApredictionEngine.Predict(text);

                                var TFL = new List<TokenFeature>();
                                if (TF.Features != null && TF.OutputTokens != null)
                                {
                                    for (int tf = 0; tf < TF.Features.Length && tf < TF.OutputTokens.Length; tf++)
                                    {
                                        if (TF.OutputTokens[tf].Length <= 2) continue;
                                        TFL.Add(new TokenFeature() { F = TF.Features[tf], T = TF.OutputTokens[tf] });
                                    }
                                } 
                                TFL = TFL.OrderByDescending(o => o.F).Take(20).ToList();

                                StringBuilder ttsb = new StringBuilder();
                                for (int itf = 0; itf < TFL.Count; itf++)
                                {
                                    var tf = TFL[itf];
                                    ttsb.Append(tf.T.Replace("(", "").Replace(")", "").Replace(",", "").Replace(".", "").Replace("[", "").Replace("]", ""));
                                    if (itf != TFL.Count - 1)
                                        ttsb.Append(";");
                                }
                                paper.BodyText[i].Tokens = ttsb.ToString();
                            }

                            Console.WriteLine($"{idx} {paper.PaperId} done ");
                        }
                        catch (Exception ea)
                        {
                            Console.WriteLine($"{idx} body FAIL {ea.Message}");
                        }

                } catch(Exception e)
                    {
                        Console.WriteLine($"{idx} FAIL {e.Message}");
                    }


                }

                WritePapers(papers); // overwrite with features result
                WritePapersFeatures(paperTokenFeatures);

            }
            #endregion


            /*
            favipiravir
            avigan
            chloroquine phosphate

            Lopinavir / ritonavir
            Kaletra Aluvia

            Sarilumab Kevzara

            tocilizumab

            hydroxychloroquine and azithromycin
            */

            // https://www.nytimes.com/2020/03/17/science/coronavirus-treatment.html?smid=tw-nytimesscience&smtyp

            // https://www.kaggle.com/allen-institute-for-ai/CORD-19-research-challenge/tasks?taskId=568
            //var mlContext = new MLContext(seed: 19);
            //IDataView dataView = mlContext.Data.LoadFromTextFile<IrisData>(_dataPath, hasHeader: false, separatorChar: ','); 

            if (false)
                foreach (var paper1 in papers)
                {
                    Console.SetCursorPosition(0, ystart);
                    Console.WriteLine($" paper: {paper1.PaperId}");

                    foreach (var abstract1 in paper1.Abstract)
                    {
                        Console.SetCursorPosition(0, ystart + 1);
                        Console.WriteLine($" abstract: {abstract1.Text.TruncateTo(70)}");

                        #region paragraphAnalysis
                        /*
                        var pa = nlpProcessor.AnalyseParagraph(abstract1.Text);
                        Console.WriteLine($" sentences: {pa.sentences.Length}");
                        Console.WriteLine($" tokens: {pa.tokens.Length}");
                        Console.WriteLine($" parts of speech: {pa.partsOfSpeech.Length}");
                        Console.WriteLine($" coreference: {pa.coreference}"); 
                        */
                        #endregion


                        foreach (var paper2 in papers)
                        {
                            if (paper1 == paper2) continue;
                            //foreach (var abstract2 in paper2.Abstract)
                            //{
                            //    var similarity = Similarity.Calculate(abstract1.Text, abstract2.Text);
                            //    Console.SetCursorPosition(0, ystart + 2);
                            //    Console.WriteLine($"                    : {abstract2.Text.TruncateTo(40)}");
                            //    Console.SetCursorPosition(0, ystart + 3);
                            //    Console.WriteLine($" {similarity}");
                            //}


                            //Parallel.ForEach(paper2.Abstract, abstract2 =>
                            //{
                            //    var similarity = Similarity.Calculate(abstract1.Text, abstract2.Text);
                            //    Console.SetCursorPosition(0, ystart + 2);
                            //    Console.WriteLine($"                    : {abstract2.Text.TruncateTo(70)}");
                            //    Console.SetCursorPosition(0, ystart + 3);
                            //    Console.WriteLine($" {similarity}");
                            //});

                            //Parallel.ForEach(paper2.BodyText, bodyText2 =>
                            //{
                            //     var similarity = Similarity.Calculate(abstract1.Text, bodyText2.Text);
                            //     Console.SetCursorPosition(0, ystart + 4);
                            //     Console.WriteLine($"                    : {bodyText2.Text.TruncateTo(70)}");
                            //     Console.SetCursorPosition(0, ystart + 5);
                            //     Console.WriteLine($" {similarity}");
                            //});
                        }
                    }
                    // link abstracts with 
                    // abstracts idx.text
                    // body_text idx.text
                    // bib_entries
                    // ref_entries
                    // g.AddNode(p, );
                }


            Console.WriteLine("Done");


            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddCommandLine(args);
                }).ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<HostOptions>(option =>
                    {
                        services.AddSingleton<AppHost, AppHost>();
                    });
                });//.Build();

            await host.RunConsoleAsync(); // RunAsync();

            Console.ReadKey();
        }

        public class AppHost
        {
            //private readonly IMailService _mailService;
            public AppHost(/*IMailService mailService*/)
            {
                //_mailService = mailService;
            }
            public void Run()
            {
                // Removed for brevity
                Console.WriteLine("AppHost running");
            }
        }
    }
}
