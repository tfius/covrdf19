using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var paper = Paper.FromJson(jsonString);

namespace covrd.Model
{
    public class PaperSearchResult
    {
        public PaperSearchResult()
        {
            searchTerms = new List<string>();
            paperRefs = new List<PaperRef>();
        }
        public List<string> searchTerms;
        public List<PaperRef> paperRefs;
    }
    public class PaperRef
    {
        public PaperRef()
        {
            inTitle = 0;
            inTokens = 0;
            inAbstract = new List<int>();
            inBody = new List<int>();
        }
        public int       paperIdx;
        public int       inTitle;
        public int       inTokens;
        public double     score;
        public List<int> inAbstract;
        public List<int> inBody;
    }

    public class TextData
    {
        public string Text { get; set; }
        public string Section { get; set; }
    }

    public class TransformedTextData : TextData
    {
        public float[] Features { get; set; }
    }
    public class TransformedTextFeaturesData : TextData
    {
        public float[] Features { get; set; }
        public string[] OutputTokens { get; set; }
    }

    public class TokenFeature : TextData
    {
        public string Token { get; set; }
        public float Feature { get; set; }
    }

    public class FeaturesData : TextData
    {
        public List<TokenFeature> Token { get; set; }
    }

    public class SemanticData
    {
        public TextData Data { get; set; }

        public string[] Tokens { get; set; }
        public List<TokenFeature> TokensFeatures { get; set; }
        //public TransformedTextFeaturesData LDAFeatures { get; set; }
        //public TransformedTextFeaturesData LDAFeatures { get; set; }
        //public TransformedTextFeaturesData TextFeatures { get; set; }

        //public List<SemanticData> BagOfWords;
    }
    public class SemanticRef
    {
        public SemanticRef() { 
            Samples = new List<SemanticData>();
            FeatureTokenNames = new List<string>();
        }
        public int PaperIndex;
        public int PaperCountId;
        public string PaperDbId;
        public string PaperTitle;
        public string Score;
        public List<SemanticData> Samples;
        public List<string> FeatureTokenNames; // from preprocesed step
        

    }
    public class SemanticResultsDisplay
    {
        public SemanticResultsDisplay()
        {
            Refs = new List<SemanticRef>();
        }
        public int AllOccurances;
        public int StartAt;
        public int MaxPages;
        public List<SemanticRef> Refs;
        public List<TokenFeature> SearchResultsTokenFeatures { get; set; }
    }

    public class PaperDisplayRef
    {
        public PaperDisplayRef() { Texts = new List<string>();  }
        public int PaperIndex;
        public string PaperDbId;
        public string PaperTitle;
        public List<string> Texts;
    }

    public class ResultsDisplay
    {
        public ResultsDisplay()
        {
            Refs = new List<PaperDisplayRef>();
        }
        public List<PaperDisplayRef> Refs;
    }
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[JsonIgnore]
        public long Id { get; set; }
        public string Notes { get; set; }
    }

    public class ExecutedSearch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[JsonIgnore]
        public long Id { get; set; }
        public string Text { get; set; }
        public string Section { get; set; }
    }

    public partial class Paper
    {
        [NotMapped]
        [JsonIgnore]
        public int Idx { get; set; }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[JsonIgnore]
        public long Id { get; set; }

        [JsonProperty("paper_id")]
        public string PaperId { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("abstract")]
        public List<Abstract> Abstract { get; set; }

        [JsonProperty("body_text")]
        public List<Abstract> Body { get; set; }

        //[JsonProperty("bib_entries")]
        //public BibEntries BibEntries { get; set; }

        //[JsonProperty("ref_entries")]
        //public RefEntries RefEntries { get; set; }

        //[JsonProperty("back_matter")]
        //public List<object> BackMatter { get; set; }

        //[JsonProperty("back_matter")]
        // public List<Comment> Comments { get; set; }

        public string Tokens { get; set; }
        public string Features { get; set; }
    }

    public partial class Metadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long MetadataId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public List<MetadataAuthor> Authors { get; set; }
    }

    public partial class Abstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long AbstractId { get; set; }

        [Column(TypeName = "ntext")]
        [JsonProperty("text")]
        public string Text { get; set; }


        [JsonProperty("cite_spans")]
        public List<Span> Cites { get; set; }

        //[JsonProperty("ref_spans")]
        //public List<Span> RefSpans { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        //[JsonProperty("eq_spans", NullValueHandling = NullValueHandling.Ignore)]
        //public List<object> EqSpans { get; set; }

        public string Tokens { get; set; }
    }

    public partial class Span
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long SpanId { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("ref_id")]
        public string RefId { get; set; }
    }

    public partial class MetadataAuthor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long MetadataAuthorId { get; set; }


        [JsonProperty("first")]
        public string First { get; set; }

        //[JsonProperty("middle")]
        //public List<object> Middle { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        //[JsonProperty("affiliation")]
        //public Affiliation Affiliation { get; set; }

        //[JsonProperty("email")]
        //public string Email { get; set; }
    }

    public partial class Affiliation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long AffiliationId { get; set; }

        [JsonProperty("laboratory")]
        public string Laboratory { get; set; }

        [JsonProperty("institution")]
        public string Institution { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public partial class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long LocationId { get; set; }

        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        [JsonProperty("settlement")]
        public string Settlement { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public partial class BibEntries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long BibEntriesId { get; set; }

        [JsonProperty("BIBREF0")]
        public Bibref0 Bibref0 { get; set; }
        [JsonProperty("BIBREF1")]
        public Bibref0 Bibref1 { get; set; }
        [JsonProperty("BIBREF2")]
        public Bibref0 Bibref2 { get; set; }
        [JsonProperty("BIBREF3")]
        public Bibref0 Bibref3 { get; set; }
        [JsonProperty("BIBREF4")]
        public Bibref0 Bibref4 { get; set; }

        [JsonProperty("BIBREF5")]
        public Bibref0 Bibref5 { get; set; }

        [JsonProperty("BIBREF6")]
        public Bibref0 Bibref6 { get; set; }

        [JsonProperty("BIBREF7")]
        public Bibref0 Bibref7 { get; set; }

        [JsonProperty("BIBREF8")]
        public Bibref0 Bibref8 { get; set; }

        [JsonProperty("BIBREF9")]
        public Bibref0 Bibref9 { get; set; }

        [JsonProperty("BIBREF10")]
        public Bibref0 Bibref10 { get; set; }

        [JsonProperty("BIBREF11")]
        public Bibref0 Bibref11 { get; set; }

        [JsonProperty("BIBREF12")]
        public Bibref0 Bibref12 { get; set; }

        [JsonProperty("BIBREF13")]
        public Bibref0 Bibref13 { get; set; }

        [JsonProperty("BIBREF14")]
        public Bibref0 Bibref14 { get; set; }

        [JsonProperty("BIBREF15")]
        public Bibref0 Bibref15 { get; set; }

        [JsonProperty("BIBREF16")]
        public Bibref0 Bibref16 { get; set; }

    }

    public partial class Bibref0
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long Bibref0Id { get; set; }

        [JsonProperty("ref_id")]
        public string RefId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public List<MetadataAuthor> Authors { get; set; }

        [JsonProperty("year")]
        public long? Year { get; set; }

        [JsonProperty("venue")]
        public string Venue { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("issn")]
        public string Issn { get; set; }

        [JsonProperty("pages")]
        public string Pages { get; set; }

        [NotMapped] 
        [JsonProperty("other_ids")]
        public OtherIds OtherIds { get; set; }
    }

    /*
    public partial class Bibref0Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public string Bibref0AuthorId { get; set; }

        [JsonProperty("first")]
        public string First { get; set; }

        //[JsonProperty("middle")]
        //public List<object> Middle { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    } */
    
    public partial class OtherIds
    {
        [JsonProperty("DOI")]
        public List<string> Doi { get; set; }
    }

    public partial class RefEntries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long RefEntriesId { get; set; }

        [JsonProperty("FIGREF0")]
        public Ref0 Figref0 { get; set; }

        [JsonProperty("FIGREF1")]
        public Ref0 Figref1 { get; set; }
        [JsonProperty("FIGREF2")]
        public Ref0 Figref2 { get; set; }
        [JsonProperty("FIGREF3")]
        public Ref0 Figref3 { get; set; }
        [JsonProperty("FIGREF4")]
        public Ref0 Figref4 { get; set; }
        [JsonProperty("FIGREF5")]
        public Ref0 Figref5 { get; set; }
        [JsonProperty("FIGREF6")]
        public Ref0 Figref6 { get; set; }
        [JsonProperty("FIGREF7")]
        public Ref0 Figref7 { get; set; }
        [JsonProperty("FIGREF8")]
        public Ref0 Figref8 { get; set; }
        [JsonProperty("FIGREF9")]
        public Ref0 Figref9 { get; set; }
        [JsonProperty("FIGREF10")]
        public Ref0 Figref10 { get; set; }


        [JsonProperty("TABREF0")]
        public Ref0 Tabref0 { get; set; }

        [JsonProperty("TABREF1")]
        public Ref0 Tabref1 { get; set; }

        [JsonProperty("TABREF2")]
        public Ref0 Tabref2 { get; set; }

        [JsonProperty("TABREF3")]
        public Ref0 Tabref3 { get; set; }

        [JsonProperty("TABREF4")]
        public Ref0 Tabref4 { get; set; }

        [JsonProperty("TABREF5")]
        public Ref0 Tabref5 { get; set; }

        [JsonProperty("TABREF6")]
        public Ref0 Tabref6 { get; set; }

        [JsonProperty("TABREF7")]
        public Ref0 Tabref7 { get; set; }

        [JsonProperty("TABREF8")]
        public Ref0 Tabref8 { get; set; }

        [JsonProperty("TABREF9")]
        public Ref0 Tabref9 { get; set; }

        [JsonProperty("TABREF10")]
        public Ref0 Tabref10 { get; set; }

        [JsonProperty("TABREF11")]
        public Ref0 Tabref11 { get; set; }

        [JsonProperty("TABREF12")]
        public Ref0 Tabref12 { get; set; }

    }

    public partial class Ref0
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore]
        public long Ref0Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [NotMapped]
        [JsonProperty("latex")]
        public object Latex { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Paper
    {
        public static Paper FromJson(string json) => JsonConvert.DeserializeObject<Paper>(json, PaperConverter.Settings);
        public string GetCombinedText()
        {
            StringBuilder sb = new StringBuilder();
            // sb.Append(Metadata.Title.Replace("\\n", " ") + "\n");
            foreach (var a in Abstract)
            {
                // sb.Append(a.Text.Replace("\\n", " ") + "\n");
            }
            foreach (var b in Body)
            {
                // sb.Append(b.Section.Replace("\\n", " ") + "\n");
                sb.Append(b.Text.Replace("\n", " ").Replace("\\\n", " ").Replace("\\\\n", " "));
            }
            return sb.ToString();
        }
        public string GetBodyText()
        {
            StringBuilder sb = new StringBuilder();
            //// sb.Append(Metadata.Title.Replace("\\n", " ") + "\n");
            //foreach (var a in Abstract)
            //{
            //    // sb.Append(a.Text.Replace("\\n", " ") + "\n");
            //}
            foreach (var b in Body)
            {
                sb.Append(b.Text.Replace("\n", " ").Replace("\\\n", " ").Replace("\\\\n", " "));
            }
            return sb.ToString();
        }
        public string GetAbstractText()
        {
            StringBuilder sb = new StringBuilder();
            //// sb.Append(Metadata.Title.Replace("\\n", " ") + "\n");
            foreach (var a in Abstract)
            {
               sb.Append(a.Text.Replace("\\n", " ") + "\n");
            }
            /*foreach (var b in BodyText)
            {
                sb.Append(b.Text.Replace("\n", " ").Replace("\\\n", " ").Replace("\\\\n", " "));
            }*/
            return sb.ToString();
        }
    }

    public static class Serialize
    {
        public static string ToJson(this Paper self) => JsonConvert.SerializeObject(self, PaperConverter.Settings);
    }

    public static class PaperConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            //NullValueHandling = NullValueHandling.Ignore,
            //DefaultValueHandling = DefaultValueHandling.
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
