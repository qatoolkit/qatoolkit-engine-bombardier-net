using QAToolKit.Core.Interfaces;
using System;

namespace QAToolKit.Engine.Bombardier
{
    /// <summary>
    /// Results of Bombardier test
    /// </summary>
    public class BombardierResult : ILoadTestResult
    {
        /// <summary>
        /// Test start time
        /// </summary>
        public DateTime TestStart { get; set; }
        /// <summary>
        /// Test stop time
        /// </summary>
        public DateTime TestStop { get; set; }
        /// <summary>
        /// Test execution duration in seconds
        /// </summary>
        public double Duration { get; set; }
        /// <summary>
        /// Load test command
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Counter for 1xx HTTP status codes
        /// </summary>
        public int Counter1xx { get; set; }
        /// <summary>
        /// Counter for 2xx HTTP status codes
        /// </summary>
        public int Counter2xx { get; set; }
        /// <summary>
        /// Counter for 3xx HTTP status codes
        /// </summary>
        public int Counter3xx { get; set; }
        /// <summary>
        /// Counter for 4xx HTTP status codes
        /// </summary>
        public int Counter4xx { get; set; }
        /// <summary>
        /// Counter for 5xx HTTP status codes
        /// </summary>
        public int Counter5xx { get; set; }
        /// <summary>
        /// Average latency
        /// </summary>
        public decimal AverageLatency { get; set; }
        /// <summary>
        /// Average requests per second
        /// </summary>
        public decimal AverageRequestsPerSecond { get; set; }
        /// <summary>
        /// Maximum latency
        /// </summary>
        public decimal MaxLatency { get; set; }
        /// <summary>
        /// Maximum requests per second
        /// </summary>
        public decimal MaxRequestsPerSecond { get; set; }
        /// <summary>
        /// Standard deviation latency
        /// </summary>
        public decimal StdevLatency { get; set; }
        /// <summary>
        /// Standard deviation requests per second
        /// </summary>
        public decimal StdevRequestsPerSecond { get; set; }
        /// <summary>
        /// Set object to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"1xx - {Counter1xx}, 2xx - {Counter2xx}, 3xx - {Counter3xx}, 4xx - {Counter4xx}, 5xx - {Counter5xx}, avg lat {AverageLatency}ms, std lat {StdevLatency}ms, max lat {MaxLatency}ms, avg rps {AverageRequestsPerSecond}, std rps {StdevRequestsPerSecond}, max rps {MaxRequestsPerSecond}";
        }
    }
}
