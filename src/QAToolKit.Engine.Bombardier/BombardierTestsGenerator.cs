using QAToolKit.Core.Interfaces;
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
    public class BombardierTestsGenerator : IGenerator<IEnumerable<HttpRequest>, IEnumerable<BombardierTest>>
    {
        private readonly BombardierGeneratorOptions _bombardierGeneratorOptions;

        /// <summary>
        /// Bombardier test generator constructor
        /// </summary>
        /// <param name="options"></param>
        public BombardierTestsGenerator(Action<BombardierGeneratorOptions> options = null)
        {
            _bombardierGeneratorOptions = new BombardierGeneratorOptions();
            options?.Invoke(_bombardierGeneratorOptions);
        }

        /// <summary>
        /// Generate a Bombardier script from requests
        /// </summary>
        /// <returns></returns>
        /// <param name="source"></param>
        public Task<IEnumerable<BombardierTest>> Generate(IEnumerable<HttpRequest> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var bombardierTests = new List<BombardierTest>();

            string bombardierFullPath;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bombardierFullPath = Path.Combine(Environment.CurrentDirectory, "bombardier", "win", "bombardier.exe");
            }
            else
            {
                bombardierFullPath = Path.Combine("./bombardier", "linux", "bombardier");
            }

            foreach (var request in source)
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

                bombardierTests.Add(new BombardierTest()
                {
                    Url = new Uri(HttpUrlHelper.GenerateUrlParameters(request, _bombardierGeneratorOptions), UriKind.Absolute),
                    Method = request.Method,
                    Command = scriptBuilder.ToString()
                });
            }

            return Task.FromResult(bombardierTests.AsEnumerable());
        }
    }
}
