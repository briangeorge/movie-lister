using System;

namespace Arcadia.Challenge.Repositories
{
    public class ConnectionStringRepository
    {
        public static string GetSqlAzureConnectionString(string name)
        {
            string? conStr = Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(conStr)) // Azure Functions App Service naming convention
                conStr = Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{name}", EnvironmentVariableTarget.Process);
            return conStr ?? string.Empty;
        }
    }
}