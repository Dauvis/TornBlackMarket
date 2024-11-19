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
                int index = argument.IndexOf('=');

                if (index >= 0)
                {
                    string parameter = argument[..index];
                    string value = argument[(index + 1)..];

                    switch (parameter)
                    {
                        case "connect":
                            connectionString = value;
                            break;

                        case "database":
                            connectionString = ConnectionStringFromDatabase(configuration, value);
                            break;

                        case "downgrade":
                            bool success = long.TryParse(value, out downgradeVersion);

                            if (!success)
                            {
                                Log.Fatal("Invalid downgrade version specified: {DowngradeVersion} could not be converted to an long integer.", value);
                                throw new ArgumentException("Invalid downgrade version");
                            }
                            break;

                        default:
                            Log.Fatal("Unknown command line parameter {Parameter}. Please use 'connect', 'database', or 'downgrade'", parameter);
                            throw new ArgumentException($"Unknown command line parameter");
                    }
                }
                else
                {
                    Log.Fatal("Unknown command line argument {Parameter}. Please format as 'parameter=value'.", argument);
                    throw new ArgumentException($"Unknown command line argument");
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
