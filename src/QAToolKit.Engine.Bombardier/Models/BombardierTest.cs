﻿using System;
using System.Net.Http;

namespace QAToolKit.Engine.Bombardier.Models
{
    /// <summary>
    /// Bombardier test
    /// </summary>
    public class BombardierTest
    {
        /// <summary>
        /// Bombardier HTTP request method
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// Bombardier HTTP request Url
        /// </summary>
        public Uri Url { get; set; }
        /// <summary>
        /// Bombardier command
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Operation Id
        /// </summary>
        public string OperationId { get; set; }
    }
}
