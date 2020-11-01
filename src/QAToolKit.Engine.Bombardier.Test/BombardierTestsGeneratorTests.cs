using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Core.Models;
using QAToolKit.Core.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierTestsGeneratorTests
    {
        private readonly ILogger<BombardierTestsGeneratorTests> _logger;

        public BombardierTestsGeneratorTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<BombardierTestsGeneratorTests>();
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsVariation1Test_Successfull()
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

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=1s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsVariation2Test_Successfull()
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
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=10s --insecure", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsVariation3Test_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = false;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http1 --timeout=30s --duration=1s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsVariation4Test_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierRateLimit = 20;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=1s --rate=20", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }
    }
}
