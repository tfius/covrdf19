using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace covrd.Model
{
    public class Seed
    {
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public static async Task<PapContext> Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var db = new PapContext(serviceProvider.GetRequiredService<DbContextOptions<PapContext>>());

            if (db.Papers.Any())
            {
                // load from file and add all 
                return db;   // Data was already seeded
            }

            string path1 = @"C:\Users\tex\Downloads\covid-19 cord-19\metadata.json";
            string json = File.ReadAllText(path1);
            var metadata = JsonConvert.DeserializeObject<List<MetadataOverview>>(json, PaperConverter.Settings);

            string path2 = @"C:\Users\tex\Downloads\covid-19 cord-19\combined_papers.json";
            string papersJson = File.ReadAllText(path2);
            var papers = JsonConvert.DeserializeObject<List<Paper>>(papersJson, PaperConverter.Settings);
            //Console.WriteLine($" completed in {stopWatch.Elapsed}");
            logger.LogInformation($" seed data read completed");


            logger.LogInformation($"mapping");
            Dictionary<string, string> metadataMap = new Dictionary<string, string>();
            foreach (var r in metadata) // include only those with proper sha
            {
                if(!metadataMap.ContainsKey(r.sha))
                    metadataMap.Add(r.sha, r.Title);
                /*if (!metadataMap.ContainsKey(r.Title))
                     metadataMap.Add(r.Title, r.sha);*/
            }
            /*
            foreach (KeyValuePair<string, string> entry in metadataMap)
            {
                var m = entry.Value
                db.MetadataOverviews.Add(metadata[i]);
                logger.LogInformation($" adding {metadata[i].Title}");
            }*/
            /*
            int count = 0;
            for (int i=0;i<metadata.Count;i++)
            {
                if (metadataMap.ContainsKey(metadata[i].sha))
                {
                    count++;
                    db.MetadataOverviews.Add(metadata[i]);
                    //logger.LogInformation($" adding {metadata[i].Title}");
                }
            }
            
            logger.LogInformation($" added {count} metadatas");
            count = await db.SaveChangesAsync();
            logger.LogInformation($" saved {count} metadatas");
            */

            var count2 = 0;
            for (int i = 0; i < papers.Count; i++)
            {
                count2++;
                db.Papers.Add(papers[i]);
                if (i!=0 && i % 50 == 0)
                {
                    var cnt = db.SaveChanges();
                    logger.LogInformation($" saved {cnt} papers");
                    break;
                }
                //logger.LogInformation($" adding {papers[i].Metadata.Title}");
            }

            logger.LogInformation($" added {count2} papers");
            //count2 = await db.SaveChangesAsync();
            logger.LogInformation($" saved {count2} metadatas");

            return db;   // Data was already seeded
        }
    }
}
