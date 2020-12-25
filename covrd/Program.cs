using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using covrd.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace covrd
{
    public class Program
    {
        public static List<Paper> Papers;
        static string hostUrl = "http://*:80;https://*:81";
        static string currentDirectory = Environment.CurrentDirectory;// + "/../../";
        static string path2 = currentDirectory + "/Resources/combined_papers.json";
        static string pathML = currentDirectory + "/ML/lda_predictionengine_all.zip";

        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            Console.Write("Loading " + path2);

            try
            {
                string papersJson = File.ReadAllText(path2);
                Papers = JsonConvert.DeserializeObject<List<Paper>>(papersJson, PaperConverter.Settings);
            } catch (Exception e)
            {
                Console.Write("Missing " + path2);
                Console.Write("1. Create data by running covrdf on source data from COVRD-19 dataset");
                Console.Write("2. Train LDA Estimator");
                Console.Write("3. Copy output data to " + path2);
                Console.Write("4. Copy model to " + pathML);
                return;
            }

            if(Papers!=null)
               Console.Write("Loaded " + Papers.Count);

            // need to reindex
            for (int i = 0; i < Papers.Count; i++)
                Papers[i].Idx = i;

            Console.Write("Starting up server");
            //Console.WriteLine($" completed in {stopWatch.Elapsed}");

            var logger = webHost.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("starting the seed");

            using (var scope = webHost.Services.CreateScope())
            {
                //Get the instance of DBContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<PapContext>();
                var ctx = await Seed.Initialize(services, logger);
            }

            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(hostUrl);
                });
    }
}
