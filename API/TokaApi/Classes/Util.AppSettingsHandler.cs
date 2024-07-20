using System;

namespace Util
{
    /// <summary>AppSettingsHandler</summary>
    public static class AppSettingsHandler
    {
        /// <summary>Find and get the connection string in the appsettings.json file.</summary>
        /// <param name="name">The connection string key.</param>
        /// <returns>System.String</returns>
        public static string GetConnectionString(string name)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var config = builder.Build();

            return config.GetConnectionString(name);
        }
    }
}
