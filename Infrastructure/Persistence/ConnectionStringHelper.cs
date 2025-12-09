using Microsoft.Extensions.Configuration;
namespace Infrastructure.Persistence;

public static class ConnectionStringHelper
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        // 1. Check for POSTGRESQL_ADDON_URI (Clever Cloud)
        var uriString = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_URI");
        if (!string.IsNullOrEmpty(uriString))
        {
            return ConvertUriToConnectionString(uriString);
        }

        // 2. Check for individual env vars (DB_HOST, etc.)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString != null && connectionString.Contains("${DB_HOST}"))
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
            var ssl = Environment.GetEnvironmentVariable("DB_SSLMODE") ?? "Prefer";

            return connectionString
                .Replace("${DB_HOST}", host)
                .Replace("${DB_PORT}", port)
                .Replace("${DB_NAME}", db)
                .Replace("${DB_USER}", user)
                .Replace("${DB_PASSWORD}", pass)
                .Replace("${DB_SSLMODE}", ssl);
        }

        return connectionString ?? string.Empty;
    }

    private static string ConvertUriToConnectionString(string input)
    {
        if (input.StartsWith("postgres://") || input.StartsWith("postgresql://"))
        {
            try
            {
                var uri = new Uri(input);
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";
                
                return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password};SslMode=Require;Trust Server Certificate=true";
            }
            catch
            {
                // Log warning or fallback
                return input;
            }
        }
        return input;
    }
}
