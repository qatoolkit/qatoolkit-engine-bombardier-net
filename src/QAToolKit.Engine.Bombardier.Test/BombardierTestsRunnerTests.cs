using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Core.Models;
using QAToolKit.Core.Test;
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

        [Fact]
        public async Task BombardierGetTestWithOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

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

        [Fact]
        public async Task BombardierPostTestWithOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierNumberOfTotalRequests = 1;
                options.AddReplacementValues(new ReplacementValue[] {
                    new ReplacementValue(){
                        Key = "id",
                        Value = 1241451
                    },
                    new ReplacementValue(){
                        Key = "name",
                        Value = "MJ"
                    }
                });
            });

            var content = File.ReadAllText("Assets/addPet.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

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
    }
}
