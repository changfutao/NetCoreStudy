using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace WebAPP1.Common
{
    public class AppSettings
    {
        static IConfiguration Configuration;
        static AppSettings()
        {
            string Path = "appsettings.json";
            Configuration = new ConfigurationBuilder().AddJsonFile(Path);
        }
    }
}
