using Newtonsoft.Json;
using QAToolKit.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QAToolKit.Engine.Bombardier.Test
{
    public class BombardierTestsGeneratorTests
    {
       /* [Fact]
        public async Task BombardierTestWithoutOptionsTest_Successfull()
        {

            var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
            {
                options.BombardierConcurrentUsers = 1;
                options.BombardierDuration = 1;
                options.BombardierTimeout = 30;
                options.BombardierUseHttp2 = true;
            });

            var content = File.ReadAllText("d:\\getPetById.json");
            var httpRequest = JsonConvert.DeserializeObject<IList<HttpTestRequest>>(content);

            var bombardierTests = await bombardierTestsGenerator.Generate(httpRequest);

            //Run Bombardier Tests
            var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
            {
                options.ObfuscateAuthenticationHeader = true;
            });
            var bombardierResults = await bombardierTestsRunner.Run();

            Assert.NotNull(bombardierResults);
        }*/
    }
}
