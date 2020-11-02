﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QAToolKit.Core.HttpRequestTools;
using QAToolKit.Core.Models;
using System;
using System.Collections;
using System.Linq;
using System.Net.Http;

namespace QAToolKit.Engine.Bombardier.Helpers
{
    /// <summary>
    /// Http body helper
    /// </summary>
    public static class HttpBodyHelper
    {

        /// <summary>
        /// Generate JSON body
        /// </summary>
        /// <param name="request"></param>
        /// <param name="useContentType"></param>
        /// <param name="replacementValues"></param>
        /// <returns></returns>
        internal static string GenerateHttpRequestBody(HttpRequest request, ContentType.Enumeration useContentType, ReplacementValue[] replacementValues)
        {
            if (request.Method == HttpMethod.Get)
            {
                return String.Empty;
            }
            else
            {
                if (useContentType != ContentType.Enumeration.Json)
                    useContentType = ContentType.Enumeration.Json;

                var useRequest = request.RequestBodies.FirstOrDefault(content => content.ContentType == useContentType);

                if (useRequest == null)
                    return String.Empty;

                var replacer = new HttpRequestDataReplacer(request, replacementValues);

                return replacer.ReplaceRequestBodyModel(useContentType).ToString();
            }
        }
    }
}