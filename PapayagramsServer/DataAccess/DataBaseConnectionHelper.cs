using LanguageExt;
using log4net;
using System;
using System.Configuration;

namespace DataAccess
{
    internal static class DataBaseConnectionHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DataBaseConnectionHelper));

        public static string GetConnectionString()
        {
            string serverName = Environment.GetEnvironmentVariable("Papayagrams_DataBaseServerName");
            string password = Environment.GetEnvironmentVariable("Papayagrams_DataBasePassword");

            if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(password))
            {
                _logger.ErrorFormat("Environment variables for connection string not found (serverName: {0}, password:{1})", string.IsNullOrEmpty(serverName), string.IsNullOrEmpty(password));
            }

            string connectionString = ConfigurationManager.ConnectionStrings["papayagramsEntities"].ConnectionString;
            return connectionString.Replace("{server_placeholder}",serverName).Replace("{password_placeholder}", password);
        }
    }
}
