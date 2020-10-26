using QAToolKit.Core.Interfaces;
using QAToolKit.Core.Models;
using QAToolKit.Engine.Bombardier.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QAToolKit.Engine.Bombardier
{
    public class BombardierTestsGenerator : IGenerator<IList<HttpTestRequest>, IEnumerable<BombardierTest>>
    {
        private readonly BombardierOptions _bombardierOptions;

        public BombardierTestsGenerator(Action<BombardierOptions> options)
        {
            _bombardierOptions = new BombardierOptions();
            options?.Invoke(_bombardierOptions);
        }

        /// <summary>
        /// Generate a Bombardier script
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

            foreach (var request in restRequests)
            {
                string authHeader = GeneratorHelper.GenerateAuthHeader(request, _bombardierOptions.CustomerAccessToken,
                    _bombardierOptions.AdministratorAccessToken, _bombardierOptions.ApiKey);

                scriptBuilder.AppendLine($"{bombardierFullPath} " +
                    $"-m {request.Method.ToString().ToUpper()} {GeneratorHelper.GenerateUrlParameters(request)} " +
                    $"-c {_bombardierOptions.BombardierConcurrentUsers} " +
                    $"{authHeader}" +
                    $"{GeneratorHelper.GenerateContentTypeHeader(request)}" +
                    $"{GeneratorHelper.GenerateJsonBody(request)}" +
                    $"--{(Convert.ToBoolean(_bombardierOptions.BombardierUseHttp2) ? "http2" : "http1")} " +
                    $"--timeout={_bombardierOptions.BombardierTimeout}s " +
                    $"--duration={_bombardierOptions.BombardierDuration}s " +
                    $"{GeneratorHelper.GenerateRateLimit(_bombardierOptions.BombardierRateLimit)}");

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
