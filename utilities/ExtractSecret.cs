using Microsoft.Extensions.Configuration;

namespace playwrightCSharp.utilities
{
    public class ExtractSecret
    {
        private static IConfiguration _configuration;

        static ExtractSecret()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true) // Change to optional: true for CI
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddUserSecrets<ExtractSecret>(optional: true)
                .Build();
        }

        public static string GetSecret(string key)
        {
            return _configuration[key];
        }

        public static string BaseUrl =>
            Environment.GetEnvironmentVariable("BaseUrl")
            ?? _configuration["BaseUrl"]!;

        public static string BaseApiUrl =>
            Environment.GetEnvironmentVariable("BaseApiUrl")
            ?? _configuration["BaseApiUrl"]!;

        public static string TestEmail =>
            Environment.GetEnvironmentVariable("TestEmail")
            ?? _configuration["TestEmail"]!;

        public static string TestPassword =>
            Environment.GetEnvironmentVariable("TestPassword")
            ?? _configuration["TestPassword"]!;
    }
}