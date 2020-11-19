using QAToolKit.Engine.Bombardier.Interfaces;
using QAToolKit.Engine.Bombardier.Models;
using System;
using System.Collections.Generic;

namespace QAToolKit.Engine.Bombardier
{
    /// <summary>
    /// Bombardier test asserter
    /// </summary>
    public class BombardierTestAsserter : ILoadTestAsserter
    {
        private readonly BombardierResult _bombardierResult;
        private readonly List<AssertResult> _assertResults;

        /// <summary>
        /// Create new Bombardier asserter
        /// </summary>
        /// <param name="bombardierResult"></param>
        public BombardierTestAsserter(BombardierResult bombardierResult)
        {
            _bombardierResult = bombardierResult ?? throw new ArgumentNullException($"{nameof(bombardierResult)} is null.");
            _assertResults = new List<AssertResult>();
        }

        /// <summary>
        /// Assert average latency
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter AverageLatency(Func<decimal, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.AverageLatency);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(AverageLatency),
                Message = $"Average latency: '{_bombardierResult.AverageLatency}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert average requests per second
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter AverageRequestsPerSecond(Func<decimal, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.AverageRequestsPerSecond);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(AverageRequestsPerSecond),
                Message = $"Average requests per seconds: '{_bombardierResult.AverageRequestsPerSecond}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert maximum latency
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter MaximumLatency(Func<decimal, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.MaxLatency);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(MaximumLatency),
                Message = $"Maximum latency: '{_bombardierResult.MaxLatency}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert maximum requests per second
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter MaximumRequestsPerSecond(Func<decimal, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.MaxRequestsPerSecond);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(MaximumRequestsPerSecond),
                Message = $"Maximum requests per second: '{_bombardierResult.MaxRequestsPerSecond}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert number of 1xx responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter NumberOf1xxResponses(Func<long, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.Counter1xx);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(NumberOf1xxResponses),
                Message = $"Number of 1xx requests: '{_bombardierResult.Counter1xx}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert number of 2xx responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter NumberOf2xxResponses(Func<long, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.Counter2xx);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(NumberOf2xxResponses),
                Message = $"Number of 2xx requests: '{_bombardierResult.Counter2xx}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert number of 3xx responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter NumberOf3xxResponses(Func<long, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.Counter3xx);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(NumberOf3xxResponses),
                Message = $"Number of 3xx requests: '{_bombardierResult.Counter3xx}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert number of 4xx responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter NumberOf4xxResponses(Func<long, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.Counter4xx);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(NumberOf4xxResponses),
                Message = $"Number of 4xx requests: '{_bombardierResult.Counter4xx}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Assert number of 5xx responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        public ILoadTestAsserter NumberOf5xxResponses(Func<long, bool> predicateFunction)
        {
            var isTrue = predicateFunction.Invoke(_bombardierResult.Counter5xx);
            _assertResults.Add(new AssertResult()
            {
                Name = nameof(NumberOf5xxResponses),
                Message = $"Number of 5xx requests: '{_bombardierResult.Counter5xx}'.",
                IsTrue = isTrue
            });

            return this;
        }

        /// <summary>
        /// Return all Assert messages of the Asserter
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AssertResult> AssertAll()
        {
            return _assertResults;
        }
    }
}
