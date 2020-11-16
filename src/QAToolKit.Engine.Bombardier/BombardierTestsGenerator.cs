using QAToolKit.Core.Models;
using QAToolKit.Engine.Bombardier.Helpers;
using QAToolKit.Engine.Bombardier.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QAToolKit.Engine.Bombardier
{
    /// <summary>
    /// Bombardier test generator
    /// </summary>
    public class BombardierTestsGenerator
    {
        private readonly IEnumerable<HttpRequest> _httpRequests;
        private readonly BombardierGeneratorOptions _bombardierGeneratorOptions;

        /// <summary>
        /// Bombardier test generator constructor
        /// </summary>
        /// <param name="httpRequests"></param>
        /// <param name="options"></param>
        public BombardierTestsGenerator(IEnumerable<HttpRequest> httpRequests, Action<BombardierGeneratorOptions> options = null)
        {
            _httpRequests = httpRequests ?? throw new ArgumentNullException(nameof(httpRequests));
            _bombardierGeneratorOptions = new BombardierGeneratorOptions();
            options?.Invoke(_bombardierGeneratorOptions);
        }

        /// <summary>
        /// Generate a Bombardier scripts from requests
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<BombardierTest>> Generate()
        {
            string bombardierFullPath = GetBombardierPath();

            var bombardierTests = new List<BombardierTest>();
            foreach (var request in _httpRequests)
            {
                bombardierTests.Add(GenerateScript(bombardierFullPath, request));
            }

            return Task.FromResult(bombardierTests.AsEnumerable());
        }

        private static string GetBombardierPath()
        {
            string bombardierFullPath;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bombardierFullPath = Path.Combine(Environment.CurrentDirectory, "bombardier", "win", "bombardier.exe");
            }
            else
            {
                bombardierFullPath = Path.Combine("./bombardier", "linux", "bombardier");
            }

            return bombardierFullPath;
        }

        private BombardierTest GenerateScript(string bombardierFullPath, HttpRequest request)
        {
            var scriptBuilder = new StringBuilder();
            scriptBuilder.Append($"{bombardierFullPath} " +
                $"-m {request.Method.ToString().ToUpper()} {HttpUrlHelper.GenerateUrlParameters(request, _bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateConcurrentSwitch(_bombardierGeneratorOptions)}" +
                $"{AuthorizationHeaderHelper.GenerateAuthHeader(request, _bombardierGeneratorOptions)}" +
                $"{ContentTypeHeaderHelper.GenerateContentTypeHeader(request, _bombardierGeneratorOptions.BombardierBodyContentType)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateBodySwitch(request, _bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateHttpProtocolSwitch(_bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateTimeoutSwitch(_bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateDurationSwitch(_bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateInsecureSwitch(_bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateRateLimitSwitch(_bombardierGeneratorOptions)}" +
                $"{BombardierSwitchGeneratorHelper.GenerateTotalRequestsSwitch(_bombardierGeneratorOptions)}");

            return new BombardierTest()
            {
                Url = new Uri(HttpUrlHelper.GenerateUrlParameters(request, _bombardierGeneratorOptions), UriKind.Absolute),
                Method = request.Method,
                Command = scriptBuilder.ToString(),
                OperationId = request.OperationId ?? null
            };
        }
    }
}
