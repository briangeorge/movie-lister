using System;

public class ConnectionStringRepository
{
    public static string GetSqlAzureConnectionString(string name)
    {
        string conStr = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
        if (string.IsNullOrEmpty(conStr)) // Azure Functions App Service naming convention
            conStr = System.Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{name}", EnvironmentVariableTarget.Process);
        return conStr;
    }
}