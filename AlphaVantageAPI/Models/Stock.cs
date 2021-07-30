using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlphaVantageAPI.Models
{
    public class Stock
    {
        public string Symbol { get; set; }
    }

    public class Price
    {
        public int ID { get; set; }
        public string Symbol { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public int? Volume { get; set; }
        public DateTime? Date { get; set; }
    }

    public class MetaData
    {
        [JsonProperty("2. Symbol")]
        public string Symbol { get; set; }
    }

    public class MonthlyTimeSeries
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> Fields { get; set; }
    }

    public class Root
    {
        [JsonProperty("Meta Data")]
        public MetaData MetaData { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public MonthlyTimeSeries MonthlyTimeSeries { get; set; }
    }
}
