using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace WebAPP1.Common
{
    public class AppSettings
    {
        static IConfiguration Configuration { get; set; }
        static AppSettings()
        {
            string Path = "appsettings.json";
            Configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
                            .Build();
        }

        public static string app(params string[]sections)
        {
            try
            {
                string val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }
                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {

                return "";
            }
        }

    }
}
