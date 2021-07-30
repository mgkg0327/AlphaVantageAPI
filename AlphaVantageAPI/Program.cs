using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using AlphaVantageAPI.Models;
using AlphaVantageAPI.Helpers;

namespace AlphaVantageAPI
{
    public class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static object JsonConvert { get; private set; }
        DBLogic db = new DBLogic();


        static async Task Main(string[] args)
        {
            Program p = new Program();
            await p.GetHistoricPrices("TSLA");
            await p.ProcessAllStocks("TIME_SERIES_DAILY");
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }


        public async Task ProcessAllStocks(string reportType)
        {
            // Get a list of all stock symbols in the db
            DataSet ds = db.GetAllStocks();

            var configuration = GetConfiguration();
            var apiKey = configuration.GetSection("ApiInfo").GetSection("Key").Value;
            var apiHost = configuration.GetSection("ApiInfo").GetSection("Host").Value;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var symbol = row[0].ToString();

                var url = "https://alpha-vantage.p.rapidapi.com/query?function=" + reportType + "&symbol=" + symbol;

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Headers =
                    {
                        { "x-rapidapi-key", apiKey },
                        { "x-rapidapi-host", apiHost },
                    },
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    Root obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(body);

                    var dates = obj.MonthlyTimeSeries.Fields;
                    string date, open, high, low, close, volume;
                    date = open = high = low = close = volume = string.Empty;

                    foreach (var ob in obj.MonthlyTimeSeries.Fields)
                    {
                        date = ob.Key;
                        open = ob.Value["1. open"].ToString();
                        high = ob.Value["2. high"].ToString();
                        low = ob.Value["3. low"].ToString();
                        close = ob.Value["4. close"].ToString();
                        volume = ob.Value["5. volume"].ToString();

                        var result = db.AddPrices(symbol, open, high, low, close, volume, date);
                    }
                    Console.WriteLine(body);
                }
            }
        }

        public async Task GetHistoricPrices(string symbol)
        {
            List<Price> prices = db.GetHistoricPricesDB(symbol);

            foreach (var price in prices)
            {
                var symbol2 = price.Symbol;
                var open = price.Open;
                var high = price.High;
            }
        }
    }
}
