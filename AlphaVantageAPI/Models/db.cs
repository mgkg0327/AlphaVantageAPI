using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace AlphaVantageAPI.Models
{
    public class db
    {
        SqlConnection con;
        public db()
        {
            var configuration = GetConfiguration();
            con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("DEV").Value);
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }

}
