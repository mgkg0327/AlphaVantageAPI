using AlphaVantageAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace AlphaVantageAPI.Helpers
{
    public class DBLogic
    {
        SqlConnection con;

        public DBLogic()
        {
            var configuration = GetConfiguration();
            con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("DEV").Value);
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }


        public DataSet GetAllStocks()
        {
            string queryString = "SELECT Symbol FROM Symbols";
            SqlCommand cmd = new SqlCommand(queryString, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public List<Price> GetHistoricPricesDB(string symbol)
        {
            List<Price> somePrice = new List<Price>();

            string queryString = "SELECT * FROM Prices WHERE symbol = @Symbol";
            SqlCommand cmd = new SqlCommand(queryString, con);
            cmd.Parameters.AddWithValue("@Symbol", symbol);

            con.Open();
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    somePrice.Add(new Price()
                    {
                        Open = (decimal)oReader["Open"],
                        High = (decimal)oReader["High"]
                    });
                }
                con.Close();
            }

            return somePrice;
        }

        public string AddPrices(string symbol, string open, string high, string low, string close, string volume, string date)
        {
            SqlCommand cmd = new SqlCommand("sp_AddPrices", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@Symbol", symbol));
            cmd.Parameters.Add(new SqlParameter("@Open", open));
            cmd.Parameters.Add(new SqlParameter("@High", high));
            cmd.Parameters.Add(new SqlParameter("@Low", low));
            cmd.Parameters.Add(new SqlParameter("@Close", close));
            cmd.Parameters.Add(new SqlParameter("@Volume", volume));
            cmd.Parameters.Add(new SqlParameter("@Date", date));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return "finished";
        }
    }
}
