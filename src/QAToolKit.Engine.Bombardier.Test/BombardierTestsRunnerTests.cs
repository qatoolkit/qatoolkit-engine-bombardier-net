using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierTestsRunnerTests
    {
        private readonly ILogger<BombardierTestsRunnerTests> _logger;

        public BombardierTestsRunnerTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<BombardierTestsRunnerTests>();
        }

        //[IgnoreOnGithubFact]
        public async Task BombardierGetTestWithOptionsTest_Successfull()
        {
            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTestsGenerator = new BombardierTestsGenerator(httpRequest, options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
            });

            var bombardierTests = await bombardierTestsGenerator.Generate();

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

            _logger.LogInformation(JsonConvert.SerializeObject(bombardierResults, Formatting.Indented));

            Assert.NotNull(bombardierResults);
            Assert.Single(bombardierResults);
            Assert.True(bombardierResults.FirstOrDefault().Counter1xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter2xx > 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter3xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter4xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter5xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().TestStart.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.Subtract(bombardierResults.FirstOrDefault().TestStart).TotalSeconds == bombardierResults.FirstOrDefault().Duration);
        }

        //[IgnoreOnGithubFact]
        public async Task BombardierPostTestWithOptionsTest_Successfull()
        {
            var content = File.ReadAllText("Assets/addPet.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTestsGenerator = new BombardierTestsGenerator(httpRequest, options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierNumberOfTotalRequests = 1;
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"id",1241451},
                        { "name","MJ"}
                });
            });

            var bombardierTests = await bombardierTestsGenerator.Generate();

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

            _logger.LogInformation(JsonConvert.SerializeObject(bombardierResults, Formatting.Indented));

            Assert.NotNull(bombardierResults);
            Assert.Single(bombardierResults);
            Assert.True(bombardierResults.FirstOrDefault().Counter1xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter2xx > 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter3xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter4xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter5xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().TestStart.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.Subtract(bombardierResults.FirstOrDefault().TestStart).TotalSeconds == bombardierResults.FirstOrDefault().Duration);
        }

        //[IgnoreOnGithubFact]
        public async Task BombardierPostTestWithBodyAndOptionsTest_Successfull()
        {
            var content = File.ReadAllText("Assets/AddBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTestsGenerator = new BombardierTestsGenerator(httpRequest, options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.AddReplacementValues(new Dictionary<string, object> {
                    {"Bicycle",@"{""id"":66,""name"":""my bike"",""brand"":""cannondale"",""BicycleType"":1}"}
                });
            });

            var bombardierTests = await bombardierTestsGenerator.Generate();

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

            _logger.LogInformation(JsonConvert.SerializeObject(bombardierResults, Formatting.Indented));

            Assert.NotNull(bombardierResults);
            Assert.Single(bombardierResults);
            Assert.Equal(@"-m POST https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 1 -H ""Content-Type: application/json"" -b ""{""id"":66,""name"":""my bike"",""brand"":""cannondale"",""BicycleType"":1}"" --http2 --timeout=30s --duration=1s", bombardierResults.FirstOrDefault().Command);
            Assert.True(bombardierResults.FirstOrDefault().Counter1xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter2xx > 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter3xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter4xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().Counter5xx == 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevLatency >= 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevRequestsPerSecond >= 0);
            Assert.True(bombardierResults.FirstOrDefault().TestStart.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.Subtract(bombardierResults.FirstOrDefault().TestStart).TotalSeconds == bombardierResults.FirstOrDefault().Duration);
        }
    }
}
