using QAToolKit.Core.Interfaces;

namespace QAToolKit.Engine.Bombardier
{
    public class BombardierResult : ILoadTestResult
    {
        public string Command { get; set; }
        public int Counter1xx { get; set; }
        public int Counter2xx { get; set; }
        public int Counter3xx { get; set; }
        public int Counter4xx { get; set; }
        public int Counter5xx { get; set; }
        public decimal AverageLatency { get; set; }
        public decimal AverageRequestsPerSecond { get; set; }
        public decimal MaxLatency { get; set; }
        public decimal MaxRequestsPerSecond { get; set; }
        public decimal StdevLatency { get; set; }
        public decimal StdevRequestsPerSecond { get; set; }

        public override string ToString()
        {
            return $"1xx - {Counter1xx}, 2xx - {Counter2xx}, 3xx - {Counter3xx}, 4xx - {Counter4xx}, 5xx - {Counter5xx}, avg lat {AverageLatency}ms, std lat {StdevLatency}ms, max lat {MaxLatency}ms, avg rps {AverageRequestsPerSecond}, std rps {StdevRequestsPerSecond}, max rps {MaxRequestsPerSecond}";
        }
    }
}
