using Newtonsoft.Json;
using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierTestsGeneratorTests
    {
        [Fact]
        public async Task GenerateBombardierTestWithOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpTestRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/1?petId=1 -c 1 --http2 --timeout=30s --duration=1s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsAlternativeTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 10;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierInsecure = true;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpTestRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/1?petId=1 -c 1 --http2 --timeout=30s --duration=10s --insecure", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/1", bombardierTests.FirstOrDefault().Url.ToString());
        }

       /* [Fact]
        public async Task BombardierTestWithOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpTestRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

            Assert.NotNull(bombardierResults);
            Assert.Single(bombardierResults);
            Assert.True(bombardierResults.FirstOrDefault().AverageLatency > 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxLatency > 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevLatency > 0);
            Assert.True(bombardierResults.FirstOrDefault().AverageRequestsPerSecond > 0);
            Assert.True(bombardierResults.FirstOrDefault().MaxRequestsPerSecond > 0);
            Assert.True(bombardierResults.FirstOrDefault().StdevRequestsPerSecond > 0);
            Assert.True(bombardierResults.FirstOrDefault().TestStart.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.AddMinutes(60) > DateTime.Now);
            Assert.True(bombardierResults.FirstOrDefault().TestStop.Subtract(bombardierResults.FirstOrDefault().TestStart).TotalSeconds == bombardierResults.FirstOrDefault().Duration);
        }*/
    }
}
