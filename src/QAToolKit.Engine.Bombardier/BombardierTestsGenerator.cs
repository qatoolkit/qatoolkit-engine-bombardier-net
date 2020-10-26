using QAToolKit.Core.Interfaces;
using QAToolKit.Core.Models;
using QAToolKit.Engine.Bombardier.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QAToolKit.Engine.Bombardier
{
    public class BombardierTestsGenerator : IGenerator<IList<HttpTestRequest>, IEnumerable<BombardierTest>>
    {
        private readonly BombardierGeneratorOptions _bombardierGeneratorOptions;

        public BombardierTestsGenerator(Action<BombardierGeneratorOptions> options)
        {
            _bombardierGeneratorOptions = new BombardierGeneratorOptions();
            options?.Invoke(_bombardierGeneratorOptions);
        }

        /// <summary>
        /// Generate a Bombardier script from requests
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BombardierTest>> Generate(IList<HttpTestRequest> restRequests)
        {
            var bombardierTests = new List<BombardierTest>();
            var scriptBuilder = new StringBuilder();

            var bombardierFullPath = String.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bombardierFullPath = Path.Combine(Environment.CurrentDirectory, "bombardier", "win", "bombardier.exe");
            }
            else
            {
                bombardierFullPath = Path.Combine("./bombardier", "linux", "bombardier");
            }

            foreach (var request in restRequests.Where(request => request.TestTypes.Contains(_bombardierGeneratorOptions.TestType)))
            {
                string authHeader = GeneratorHelper.GenerateAuthHeader(request, _bombardierGeneratorOptions);

                scriptBuilder.AppendLine($"{bombardierFullPath} " +
                    $"-m {request.Method.ToString().ToUpper()} {GeneratorHelper.GenerateUrlParameters(request)} " +
                    $"-c {_bombardierGeneratorOptions.BombardierConcurrentUsers} " +
                    $"{authHeader}" +
                    $"{GeneratorHelper.GenerateContentTypeHeader(request)}" +
                    $"{GeneratorHelper.GenerateJsonBody(request)}" +
                    $"--{(Convert.ToBoolean(_bombardierGeneratorOptions.BombardierUseHttp2) ? "http2" : "http1")} " +
                    $"--timeout={_bombardierGeneratorOptions.BombardierTimeout}s " +
                    $"--duration={_bombardierGeneratorOptions.BombardierDuration}s " +
                    $"{GeneratorHelper.GenerateRateLimit(_bombardierGeneratorOptions.BombardierRateLimit)}");

                bombardierTests.Add(new BombardierTest()
                {
                    Url = request.Path,
                    Method = request.Method.ToString(),
                    Command = scriptBuilder.ToString()
                });
            }

            return bombardierTests;
        }
    }
}
