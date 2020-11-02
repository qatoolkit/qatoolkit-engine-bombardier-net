﻿using QAToolKit.Core.Interfaces;
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
    /// <summary>
    /// Bombardier test generator
    /// </summary>
    public class BombardierTestsGenerator : IGenerator<IList<HttpRequest>, IEnumerable<BombardierTest>>
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
        public async Task<IEnumerable<BombardierTest>> Generate(IList<HttpRequest> restRequests)
        {
            var bombardierTests = new List<BombardierTest>();
            var scriptBuilder = new StringBuilder();

            string bombardierFullPath;

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
                scriptBuilder.AppendLine($"{bombardierFullPath} " +
                    $"-m {request.Method.ToString().ToUpper()} {GeneratorHelper.GenerateUrlParameters(request)}" +
                    $"{GeneratorHelper.GenerateConcurrentSwitch(request, _bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateAuthHeader(request, _bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateContentTypeHeader(request, _bombardierGeneratorOptions.BombardierBodyContentType)}" +
                    $"{GeneratorHelper.GenerateJsonBody(request, _bombardierGeneratorOptions.BombardierBodyContentType)}" +
                    $"{GeneratorHelper.GenerateHttpProtocolSwitch(_bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateTimeoutSwitch(_bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateDurationSwitch(_bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateInsecureSwitch(_bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateRateLimitSwitch(_bombardierGeneratorOptions)}" +
                    $"{GeneratorHelper.GenerateTotalRequestsSwitch(_bombardierGeneratorOptions)}");

                bombardierTests.Add(new BombardierTest()
                {
                    Url = new Uri(request.Path, UriKind.RelativeOrAbsolute),
                    Method = request.Method,
                    Command = scriptBuilder.ToString()
                });
            }

            return bombardierTests;
        }
    }
}
