using System;
using System.Collections.Generic;
using System.Text;

namespace covrdf.NERModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public partial class EntityInfo
    {
        public string TypeName { get; set; }

        public HashSet<string> UniqueText { get; set; }
        public List<string> Texts { get; set; }
        public HashSet<long> DocIds { get; set; }
    }


    public partial class Annotation
    {
        [JsonProperty("doc_id")]
        public long DocId { get; set; }

        [JsonProperty("sents")]
        public List<Sent> Sents { get; set; }
    }

    public partial class Sent
    {
        [JsonProperty("sent_id")]
        public long SentId { get; set; }

        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }
    }

    public partial class Entity
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, covrdf.NERModel.NERConverter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, covrdf.NERModel.NERConverter.Settings);
    }

    public static class NERConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Error = delegate (object sender, ErrorEventArgs args)
            {
                //errors.Add(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
                Console.WriteLine(args.ErrorContext.Error.Message);
            },
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
