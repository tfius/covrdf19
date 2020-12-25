using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace covrd.Model
{
    public class MetadataOverview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public string MetadataOverviewId { get; set; }

        [Display(Name = "sha", Order = 1)]
        public string sha { get; set; }

        [Display(Name = "source_x", Order = 2)]
        public string Source_x { get; set; }

        [Display(Name = "title", Order = 3)]
        public string Title { get; set; }

        [Display(Name = "doi", Order = 4)]
        public string Doi { get; set; }

        [Display(Name = "pmcid", Order = 5)]
        public string Pmcid { get; set; }

        [Display(Name = "pubmed_id", Order = 6)]
        public string Pubmed_id { get; set; }

        [Display(Name = "license", Order = 7)]
        public string License { get; set; }

        [Display(Name = "abstract", Order = 8)]
        public string Abstract { get; set; }

        [Display(Name = "publish_time", Order = 9)]
        public string Publish_time { get; set; }

        [Display(Name = "authors", Order = 10)]
        public string Authors { get; set; }

        [Display(Name = "journal", Order = 11)]
        public string Journal { get; set; }

        [Display(Name = "Microsoft Academic Paper ID", Order = 12)]
        public string Msapid { get; set; }

        [Display(Name = "WHO #Covidence", Order = 13)]
        public string Whocovid { get; set; }

        [Display(Name = "has_full_text", Order = 14)]
        public string HasFullText { get; set; }
        //sha,source_x,title,doi,pmcid,pubmed_id,license,abstract,publish_time,authors,journal,Microsoft Academic Paper ID, WHO #Covidence,has_full_text
    }
    public class CsvMap
    {
        [JsonIgnore]
        public long CsvMapId { get; set; }

        public string Name { get; set; }
        public string MappedTo { get; set; }
        public int Index { get; set; }
    }
}
