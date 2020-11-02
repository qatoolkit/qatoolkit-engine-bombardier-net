using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Core.Models;
using QAToolKit.Core.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
                options.AddReplacementValues(new ReplacementValue[] {
                    new ReplacementValue(){
                        Key = "id",
                        Value = 1000
                    },
                    new ReplacementValue(){
                        Key = "name",
                        Value = "MJ"
                    }
                });
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

        [Fact]
        public async Task GenerateBombardierTestWithOptionsVariation5Test_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierRateLimit = 20;
                options.BombardierNumberOfTotalRequests = 22;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsApiKeyTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddApiKey("1234");
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierRateLimit = 20;
                options.BombardierNumberOfTotalRequests = 22;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 -H \"ApiKey: 1234\" --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsBasicAuthTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddBasicAuthentication("user", "pass");
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierRateLimit = 20;
                options.BombardierNumberOfTotalRequests = 22;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            var authHeader = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("user:pass"));

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 -H \"Authorization: Basic {authHeader}\" --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestWithOptionsOAuth2Test_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddOAuth2Token("1234567890", AuthenticationType.Customer);
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
                options.BombardierRateLimit = 20;
                options.BombardierNumberOfTotalRequests = 22;
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 -H \"Authorization: Bearer 1234567890\" --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestDefaultBombardierOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator();

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPostTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new ReplacementValue[] {
                    new ReplacementValue(){
                        Key = "id",
                        Value = 1000
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

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://petstore3.swagger.io/api/v3/pet -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":1000,\""name\"":\""MJ\""}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPostWithExampleValuesTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new ReplacementValue[] {
                    new ReplacementValue(){
                        Key = "companyId",
                        Value = "1241451"
                    },
                    new ReplacementValue(){
                        Key = "companyName",
                        Value = "MJ"
                    }
                });
            });

            var content = File.ReadAllText("Assets/addPet.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://petstore3.swagger.io/api/v3/pet -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":999,\""name\"":\""my pet 999\""}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("/pet", bombardierTests.FirstOrDefault().Url.ToString());
        }
    }
}
