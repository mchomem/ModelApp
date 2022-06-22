using Microsoft.Extensions.Configuration;

namespace ModelApp.Domain.Shareds
{
    public static class AppSettings
    {
        #region Properties

        public static string AllowedHosts { get => GetValeuFromKey("AllowedHosts"); }

        public static string SqlServerConnection { get => GetConnectionString("SqlServerConnection"); }

        public static string[] UrlsClientApp
        {
            get => GetValeuFromKey("UrlsClientApp")
                .Split(";")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
        }

        #endregion

        #region Methods

        private static IConfigurationRoot GetAppSettings()
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
                IConfigurationRoot root = builder.Build();
                return root;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetConnectionString(string key) => GetAppSettings().GetConnectionString(key);

        private static string GetValeuFromKey(string key) => GetAppSettings().GetSection(key).Value;

        #endregion

        #region Nested class

        public static class SmtpEmail
        {
            #region Properties

            public static string Host { get => GetValeuFromKey("SmtpEmail:Host"); }
            public static string Port { get => GetValeuFromKey("SmtpEmail:Port"); }
            public static string User { get => GetValeuFromKey("SmtpEmail:User"); }
            public static string Password { get => GetValeuFromKey("SmtpEmail:Password"); }
            public static string From { get => GetValeuFromKey("SmtpEmail:From"); }
            public static bool EnableSSL { get => Convert.ToBoolean(GetValeuFromKey("SmtpEmail:EnableSSL")); }
            public static bool UseRelay { get => Convert.ToBoolean(GetValeuFromKey("SmtpEmail:UseRelay")); }
            public static bool Active { get => Convert.ToBoolean(GetValeuFromKey("SmtpEmail:Active")); }

            #endregion            
        }

        #endregion
    }
}
