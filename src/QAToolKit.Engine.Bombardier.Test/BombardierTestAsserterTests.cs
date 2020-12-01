using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Core.Models;
using QAToolKit.Engine.Bombardier.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierTestAsserterTests
    {
        private readonly ILogger<BombardierTestAsserterTests> _logger;

        public BombardierTestAsserterTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<BombardierTestAsserterTests>();
        }

        [IgnoreOnGithubFact]
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

            var asserter = new BombardierTestAsserter(bombardierResults.FirstOrDefault());
            var assertResults = asserter
                .NumberOf1xxResponses(x => x == 0)
                .NumberOf2xxResponses(x => x >= 0)
                .NumberOf3xxResponses(x => x == 0)
                .NumberOf4xxResponses(x => x == 0)
                .NumberOf5xxResponses(x => x == 0)
                .AverageLatency(x => x >= 0)
                .AverageRequestsPerSecond((x) => x >= 0)
                .MaximumLatency(x => x >= 0)
                .MaximumRequestsPerSecond(x => x >= 0)
                .AssertAll();

            foreach (var result in assertResults)
            {
                Assert.True(result.IsTrue, result.Message);
            }
        }

        [IgnoreOnGithubFact]
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

            var asserter = new BombardierTestAsserter(bombardierResults.FirstOrDefault());
            var assertResults = asserter
                .NumberOf1xxResponses(x => x == 0)
                .NumberOf2xxResponses(x => x >= 0)
                .NumberOf3xxResponses(x => x == 0)
                .NumberOf4xxResponses(x => x == 0)
                .NumberOf5xxResponses(x => x == 0)
                .AverageLatency(x => x >= 0)
                .AverageRequestsPerSecond(x => x >= 0)
                .MaximumLatency(x => x >= 0)
                .MaximumRequestsPerSecond(x => x >= 0)
                .AssertAll();

            foreach (var result in assertResults)
            {
                Assert.True(result.IsTrue, result.Message);
            }
        }

        [IgnoreOnGithubFact]
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

            var asserter = new BombardierTestAsserter(bombardierResults.FirstOrDefault());
            var assertResults = asserter
                .NumberOf1xxResponses(x => x == 0)
                .NumberOf2xxResponses(x => x >= 0)
                .NumberOf3xxResponses(x => x == 0)
                .NumberOf4xxResponses(x => x == 0)
                .NumberOf5xxResponses(x => x == 0)
                .AverageLatency(x => x >= 0)
                .AverageRequestsPerSecond(x => x >= 0)
                .MaximumLatency(x => x >= 0)
                .MaximumRequestsPerSecond(x => x >= 0)
                .AssertAll();

            foreach (var result in assertResults)
            {
                Assert.True(result.IsTrue, result.Message);
            }
        }

        [Fact]
        public void BombardierAsserterNullObject_Successfull()
        {
            Assert.Throws<ArgumentNullException>(() => new BombardierTestAsserter(null));
        }

        [Fact]
        public void BombardierAsserterNullResults_Successfull()
        {
            var asserter = new BombardierTestAsserter(new BombardierResult());
            var assertResults = asserter
                .NumberOf1xxResponses(x => x == 0)
                .NumberOf2xxResponses(x => x >= 0)
                .NumberOf3xxResponses(x => x == 0)
                .NumberOf4xxResponses(x => x == 0)
                .NumberOf5xxResponses(x => x == 0)
                .AverageLatency(x => x >= 0)
                .AverageRequestsPerSecond((x) => x >= 0)
                .MaximumLatency(x => x >= 0)
                .MaximumRequestsPerSecond(x => x >= 0)
                .AssertAll();

            foreach (var result in assertResults)
            {
                Assert.True(result.IsTrue, result.Message);
            }
        }
    }
}
