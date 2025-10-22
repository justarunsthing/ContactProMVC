namespace ContactProMVC.Helpers
{
    public static class ConnectionHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("pgSettings")["pgConnection"];

            return connectionString;
        }
    }
}