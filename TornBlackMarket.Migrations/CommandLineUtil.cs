using Microsoft.Extensions.Configuration;
using Serilog;

namespace TornBlackMarket.Migrations
{
    internal static class CommandLineUtil
    {
        public static MigrationSettings GetSettings(IConfigurationRoot configuration, string[] args)
        {
            string connectionString = "";
            long downgradeVersion = 0;

            foreach (var argument in args)
            {
                var parse = argument.ToLower().Split('=');

                if (parse.Length > 1)
                {
                    switch (parse[0])
                    {
                        case "connect":
                            connectionString = parse[1];
                            break;

                        case "database":
                            connectionString = ConnectionStringFromDatabase(configuration, parse[1]);
                            break;

                        case "downgrade":
                            bool success = long.TryParse(parse[1], out downgradeVersion);

                            if (!success)
                            {
                                Log.Fatal("Invalid downgrade version specified: {DowngradeVersion} could not be converted to an long integer.", parse[1]);
                                throw new ArgumentException("Invalid downgrade version");
                            }
                            break;

                        default:
                            Log.Fatal("Unknown command line parameter {Parameter}. Please use 'connect', 'database', or 'downgrade'", parse[0]);
                            throw new ArgumentException($"Unknown command line parameter");
                    }
                }
            }

            Log.Information("Running migration with following settings");

#if DEBUG
            Log.Information("Connection string: {ConnectionString}", connectionString);
#else
            Log.Information("Connection string: {ConnectionString}", "REDACTED");
#endif

            if (downgradeVersion == 0)
            {
                Log.Information("Upgrading to latest version");
            }
            else
            {
                Log.Information("Downgrading to version {Version}", downgradeVersion);
            }

            return new MigrationSettings(connectionString, downgradeVersion);
        }

        private static string ConnectionStringFromDatabase(IConfigurationRoot configuration, string database)
        {
            string connectionStringBase = (configuration["TBM_CONNECT_INFO"] ?? "");

            if (string.IsNullOrEmpty(connectionStringBase))
            {
                Log.Fatal("When specifying a database name, TBM_CONNECT_INFO environment variable need to hold base connection string");
                throw new ArgumentException("Environment variable not properly set");
            }

            return $"{connectionStringBase}Initial Catalog={database};";
        }

    }
}
