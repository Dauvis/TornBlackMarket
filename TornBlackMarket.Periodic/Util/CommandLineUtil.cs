using Microsoft.Extensions.Configuration;
using Serilog;
using TornBlackMarket.Periodic.Enums;

namespace TornBlackMarket.Periodic.Util
{
    internal static class CommandLineUtil
    {
        public static JobSettings GetSettings(IConfigurationRoot configuration, string[] args)
        {
            string apiKey = configuration["TBM_GENERAL_KEY"] ?? "";
            List<JobIdType> jobs = [];

            foreach (var argument in args)
            {
                int index = argument.IndexOf('=');

                if (index >= 0)
                {
                    string parameter = argument[..index];
                    string value = argument[(index + 1)..];

                    switch (parameter)
                    {
                        case "job":
                            jobs = ParseJobIdList(value);

                            if (jobs.Count == 0)
                            {
                                Log.Fatal("No jobs specified to run");
                                throw new ArgumentException("No jobs specified to run");
                            }
                            break;

                        case "key":
                            apiKey = value;
                            break;

                        default:
                            Log.Fatal("Unknown command line parameter {Parameter}. Please use 'job'", parameter);
                            throw new ArgumentException($"Unknown command line parameter {parameter}");
                    }
                }
                else
                {
                    Log.Fatal("Unknown command line argument {Parameter}. Please format as 'parameter=value'.", argument);
                    throw new ArgumentException($"Unknown command line argument");
                }
            }

            Log.Information("Running migration with following settings");
            Log.Information("  Job identifiers: {JobList}", string.Join(',', jobs));

            return new JobSettings(jobs, apiKey);
        }

        private static List<JobIdType> ParseJobIdList(string value)
        {
            List<JobIdType> jobList = [];
            var jobs = value.Split(',');

            foreach (var job in jobs)
            {
                bool wasParsed = Enum.TryParse(value, true, out JobIdType jobId);

                if (!wasParsed)
                {
                    Log.Fatal("Unknown job identifier {Value}. Supported values are ItemLoad", value);
                    throw new ArgumentException($"Unknown job identifier {value}");
                }

                jobList.Add(jobId);
            }

            return jobList;
        }
    }

}
