using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using covrdf.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace covrd.Controller
{
    public class VisualEvent
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int OccurancesInBody { get; set; }
        public int OccurancesInAbstract { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetTopics : ControllerBase
    {
        public GetTopics()
        {
        }
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            Dictionary<string, int> mapping = new Dictionary<string, int>();
            List<VisualEvent> ves = new List<VisualEvent>();

            for (int pidx = 0; pidx < Program.Papers.Count; pidx++)
            {
                var pap = Program.Papers[pidx];
                var t = pap.Tokens.Split(";", StringSplitOptions.RemoveEmptyEntries);
                //var text = pap.GetAbstractText();

                for (int i=0;i<t.Length;i++)
                {
                    if (t[i].Length < 2) continue; 

                    if(mapping.ContainsKey(t[i]))
                    {
                        ves[mapping[t[i]]].Count++;
                    } 
                    else
                    {
                        mapping.Add(t[i], ves.Count);
                        ves.Add(new VisualEvent() { Name = t[i], Count=1 });
                    }

                    ves[mapping[t[i]]].OccurancesInAbstract += pap.GetAbstractText().Contains(t[i]) ? 1 : 0;
                    ves[mapping[t[i]]].OccurancesInBody += pap.GetBodyText().Contains(t[i]) ? 1 : 0;
                }
            }

            ves = ves.OrderByDescending(o => o.Count).ToList();

            CsvWriter csvWriter = new CsvWriter();
            return csvWriter.Write(ves, true);
            //return ev.ListToCVS<ExtinguisherEvent>();
        }

        public static int CheckOccurrences(string str1, string pattern)
        {
            int count = 0;
            int a = 0;
            while ((a = str1.IndexOf(pattern, a)) != -1)
            {
                a += pattern.Length;
                count++;
            }
            return count;
        }
    }

    
}