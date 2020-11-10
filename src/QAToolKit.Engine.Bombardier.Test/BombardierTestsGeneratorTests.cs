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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            _logger.LogInformation(JsonConvert.SerializeObject(bombardierTests, Formatting.Indented));

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=1s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"id",1000},
                        {"name","MJ"}
                    });
            });

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=10s --insecure", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http1 --timeout=30s --duration=1s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=1s --rate=20", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains(" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 -H \"ApiKey: 1234\" --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            var authHeader = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("user:pass"));

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 -H \"Authorization: Basic {authHeader}\" --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
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
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 1 -H \"Authorization: Bearer 1234567890\" --http2 --timeout=30s --duration=1s --rate=20 --requests=22", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestDefaultBombardierOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator();

            var content = File.ReadAllText("Assets/getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($" -m GET https://petstore3.swagger.io/api/v3/pet/10 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet/10", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPostTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"id",1000},
                        {"name","MJ"}
                    });
            });

            var content = File.ReadAllText("Assets/addPet.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://petstore3.swagger.io/api/v3/pet -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":1000,\""name\"":\""MJ\""}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPostWithExampleValuesTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"companyId","1241451"},
                        {"companyName","MJ"}
                    });
            });

            var content = File.ReadAllText("Assets/addPet.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://petstore3.swagger.io/api/v3/pet -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":999,\""name\"":\""my pet 999\""}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/api/v3/pet", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetBikesReplacementTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"api-version","2"}
                    });
            });

            var content = File.ReadAllText("Assets/GetAllBikes.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=2 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=2", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetBikesExampleTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator();

            var content = File.ReadAllText("Assets/GetAllBikes.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetBikesExampleWithFilterTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"api-version","2"},
                        {"bicycleType","1"}
                    });
            });

            var content = File.ReadAllText("Assets/GetAllBikes.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://qatoolkitapi.azurewebsites.net/api/bicycles?bicycleType=1&api-version=2 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?bicycleType=1&api-version=2", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetBikesExampleWithFilterAndTotalRequestTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"api-version","1"},
                        {"bicycleType","1"}
                });
                options.BombardierNumberOfTotalRequests = 10;
            });

            var content = File.ReadAllText("Assets/GetAllBikes.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://qatoolkitapi.azurewebsites.net/api/bicycles?bicycleType=1&api-version=1 -c 3 --http2 --timeout=30s --duration=5s --requests=10", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?bicycleType=1&api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetBikesExampleWithFilterAndTotalRequestInsecureTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"api-version","2"},
                        {"bicycleType","1"}
                });
                options.BombardierNumberOfTotalRequests = 10;
                options.BombardierInsecure = true;
                options.BombardierUseHttp2 = false;
            });

            var content = File.ReadAllText("Assets/GetAllBikes.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://qatoolkitapi.azurewebsites.net/api/bicycles?bicycleType=1&api-version=2 -c 3 --http1 --timeout=30s --duration=5s --insecure --requests=10", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?bicycleType=1&api-version=2", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPostNewBikeTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"Bicycle",@"{""id"":66,""name"":""my bike"",""brand"":""cannondale"",""BicycleType"":1}"}
                });
            });

            var content = File.ReadAllText("Assets/AddBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":66,\""name\"":\""my bike\"",\""brand\"":\""cannondale\"",\""BicycleType\"":1}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPostNewBikeCaseInsensitiveTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"bicycle",@"{""id"":66,""name"":""my bike"",""brand"":""cannondale"",""BicycleType"":1}"}
                });
            });

            var content = File.ReadAllText("Assets/AddBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":66,\""name\"":\""my bike\"",\""brand\"":\""cannondale\"",\""BicycleType\"":1}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPutUpdateBikeTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"Bicycle",@"{""id"":66,""name"":""my bike"",""brand"":""cannondale"",""BicycleType"":1}"},
                        {"id","1" }
                });
            });

            var content = File.ReadAllText("Assets/UpdateBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m PUT https://qatoolkitapi.azurewebsites.net/api/bicycles/1?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":66,\""name\"":\""my bike\"",\""brand\"":\""cannondale\"",\""BicycleType\"":1}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Put, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles/1?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestPutUpdateBikeIntValueTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"Bicycle",@"{""id"":66,""name"":""my bike"",""brand"":""cannondale"",""BicycleType"":1}"},
                        {"id",1 }
                });
            });

            var content = File.ReadAllText("Assets/UpdateBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m PUT https://qatoolkitapi.azurewebsites.net/api/bicycles/1?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":66,\""name\"":\""my bike\"",\""brand\"":\""cannondale\"",\""BicycleType\"":1}}"" --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Put, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles/1?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestDeleteBikeTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"id",1 }
                });
            });

            var content = File.ReadAllText("Assets/DeleteBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m DELETE https://qatoolkitapi.azurewebsites.net/api/bicycles/1?api-version=1 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Delete, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles/1?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetSupportTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.AddReplacementValues(new Dictionary<string, object> {
                        {"CaseId",1}
                });
            });

            var content = File.ReadAllText("Assets/Support.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://petstore3.swagger.io/sales/support/v2/SupportTicket?CaseId=1 -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/sales/support/v2/SupportTicket?CaseId=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetSupportNoIdTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator();

            var content = File.ReadAllText("Assets/Support.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://petstore3.swagger.io/sales/support/v2/SupportTicket?CaseId={{CaseId}} -c 3 --http2 --timeout=30s --duration=5s", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://petstore3.swagger.io/sales/support/v2/SupportTicket?CaseId={CaseId}", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestGetBikesInsecureTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierNumberOfTotalRequests = 10;
                options.BombardierInsecure = true;
                options.BombardierUseHttp2 = false;
            });

            var content = File.ReadAllText("Assets/GetAllBikes.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m GET https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 --http1 --timeout=30s --duration=5s --insecure --requests=10", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Get, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestAddBikeInsecureTest_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierNumberOfTotalRequests = 10;
                options.BombardierInsecure = true;
                options.BombardierUseHttp2 = false;
            });

            var content = File.ReadAllText("Assets/AddBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":1,\""name\"":\""Foil\"",\""brand\"":\""Cannondale\""}}"" --http1 --timeout=30s --duration=5s --insecure --requests=10", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
        }

        [Fact]
        public async Task GenerateBombardierTestDoesNotEndWithNewLineTest1_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierNumberOfTotalRequests = 10;
                options.BombardierInsecure = true;
                options.BombardierUseHttp2 = false;
            });

            var content = File.ReadAllText("Assets/AddBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":1,\""name\"":\""Foil\"",\""brand\"":\""Cannondale\""}}"" --http1 --timeout=30s --duration=5s --insecure --requests=10", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
            Assert.DoesNotContain(Environment.NewLine, bombardierTests.FirstOrDefault().Command);
        }

        [Fact]
        public async Task GenerateBombardierTestDoesNotEndWithNewLineTest2_Successfull()
        {
            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierNumberOfTotalRequests = 10;
                options.BombardierInsecure = true;
                options.BombardierUseHttp2 = false;
            });

            var content = File.ReadAllText("Assets/AddBike.json");
            var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            Assert.NotNull(bombardierTests);
            Assert.Single(bombardierTests);
            Assert.Contains($@" -m POST https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1 -c 3 -H ""Content-Type: application/json"" -b ""{{\""id\"":1,\""name\"":\""Foil\"",\""brand\"":\""Cannondale\""}}"" --http1 --timeout=30s --duration=5s --insecure --requests=10", bombardierTests.FirstOrDefault().Command);
            Assert.Equal(HttpMethod.Post, bombardierTests.FirstOrDefault().Method);
            Assert.Equal("https://qatoolkitapi.azurewebsites.net/api/bicycles?api-version=1", bombardierTests.FirstOrDefault().Url.ToString());
            Assert.DoesNotContain(Environment.NewLine, bombardierTests.FirstOrDefault().Command);
        }
    }
}
