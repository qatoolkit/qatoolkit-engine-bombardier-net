using QAToolKit.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QAToolKit.Engine.Bombardier
{
    public class BombardierTestsRunner
    {
        private readonly IList<BombardierTest> _bombardierTests;

        public BombardierTestsRunner(IList<BombardierTest> bombardierTests)
        {
            _bombardierTests = bombardierTests;
        }

        /// <summary>
        /// Run Bombardier tests
        /// </summary>
        /// <returns></returns>
        public async Task<IList<BombardierResult>> Run()
        {
            var bombardierResult = new List<BombardierResult>();

            foreach (var test in _bombardierTests)
            {
                bombardierResult.Add(await Run(test.Command));
            }

            return bombardierResult;
        }

        private async Task<BombardierResult> Run(string testCommand)
        {
            var testStart = DateTime.Now;
            var indexOfDelimiter = testCommand.IndexOf("-m");
            var bombardierExecutable = testCommand
                .Substring(0, indexOfDelimiter - 1);
            var bombardierArguments = testCommand
                .Substring(indexOfDelimiter, testCommand.Length - indexOfDelimiter)
                .Replace(Environment.NewLine, "")
                .Trim();

            var bombardrierOutput = new StringBuilder();

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = bombardierExecutable,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = bombardierArguments
                };

                process.Start();

                while (!process.StandardOutput.EndOfStream)
                {
                    bombardrierOutput.AppendLine(await process.StandardOutput.ReadLineAsync());
                }

                process.WaitForExit();
            }

            //Delete temp files
            DeleteBodyFile(bombardierArguments);

            var testStop = DateTime.Now;
            var parsedBombardierOutput = ParseOutput(bombardrierOutput, bombardierArguments, testStart, testStop);

            return parsedBombardierOutput;
        }

        private void DeleteBodyFile(string arguments)
        {
            try
            {
                var file = StringHelper.Between(arguments, "-f \"", "\"");

                if (string.IsNullOrEmpty(file) && File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch { }
        }

        private BombardierResult ParseOutput(StringBuilder sb, string command, DateTime testStart, DateTime testStop)
        {
            try
            {
                var results = new BombardierResult
                {
                    TestStart = testStart,
                    TestStop = testStop,
                    Duration = testStop.Subtract(testStart).TotalSeconds
                };

                var str = sb.ToString();

                results.Command = command;
                results.Counter1xx = Convert.ToInt32(StringHelper.RemoveAllNonNumericChars(StringHelper.Between(str, "1xx - ", ",")));
                results.Counter2xx = Convert.ToInt32(StringHelper.RemoveAllNonNumericChars(StringHelper.Between(str, "2xx - ", ",")));
                results.Counter3xx = Convert.ToInt32(StringHelper.RemoveAllNonNumericChars(StringHelper.Between(str, "3xx - ", ",")));
                results.Counter4xx = Convert.ToInt32(StringHelper.RemoveAllNonNumericChars(StringHelper.Between(str, "4xx - ", ",")));
                results.Counter5xx = Convert.ToInt32(StringHelper.RemoveAllNonNumericChars(StringHelper.Between(str, "5xx - ", " ")));

                var reqsPerSec = StringHelper.ReplaceMultipleSpacesWithOne(StringHelper.Between(str, "Reqs/sec", Environment.NewLine).TrimStart().TrimEnd()).Split(' ');

                CultureInfo cultures = new CultureInfo("en-US");
                results.AverageRequestsPerSecond = Convert.ToDecimal(reqsPerSec[0], cultures);
                results.StdevRequestsPerSecond = Convert.ToDecimal(reqsPerSec[1], cultures);
                results.MaxRequestsPerSecond = Convert.ToDecimal(reqsPerSec[2], cultures);

                var latencies = StringHelper.ReplaceMultipleSpacesWithOne(StringHelper.Between(str, "Latency   ", Environment.NewLine).TrimStart().TrimEnd()).Split(' ');

                results.AverageLatency = GetLatencyMiliseconds(latencies[0]);
                results.StdevLatency = GetLatencyMiliseconds(latencies[1]);
                results.MaxLatency = GetLatencyMiliseconds(latencies[2]);

                return results;
            }
            catch
            {
                throw;
            }
        }

        private decimal GetLatencyMiliseconds(string latency)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var digitString = Regex.Match(latency, @"\d+.\d+");
            var unitString = Regex.Replace(latency, @"\d+.\d+", "");
            decimal digit;

            switch (unitString)
            {
                case "s":
                    digit = Convert.ToDecimal(digitString.Value, cultures) * 1000;
                    break;
                case "ms":
                    digit = Convert.ToDecimal(digitString.Value, cultures);
                    break;
                case "us":
                    digit = Convert.ToDecimal(digitString.Value, cultures) / 1000;
                    break;
                default:
                    digit = Convert.ToDecimal(digitString.Value, cultures);
                    break;
            }

            return digit;
        }
    }
}
