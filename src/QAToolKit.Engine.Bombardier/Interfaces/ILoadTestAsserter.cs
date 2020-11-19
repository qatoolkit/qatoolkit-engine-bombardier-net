using QAToolKit.Engine.Bombardier.Models;
using System;
using System.Collections.Generic;

namespace QAToolKit.Engine.Bombardier.Interfaces
{
    /// <summary>
    /// Http test asserter interface
    /// </summary>
    public interface ILoadTestAsserter
    {
        /// <summary>
        /// Assert average latency
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter AverageLatency(Func<decimal, bool> predicateFunction);
        /// <summary>
        /// Assert average requests per second
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter AverageRequestsPerSecond(Func<decimal, bool> predicateFunction);
        /// <summary>
        /// Assert maximum latency
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter MaximumLatency(Func<decimal, bool> predicateFunction);
        /// <summary>
        /// Assert maximum requests per second
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter MaximumRequestsPerSecond(Func<decimal, bool> predicateFunction);
        /// <summary>
        /// Assert number of 1xx HTTP responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter NumberOf1xxResponses(Func<long, bool> predicateFunction);
        /// <summary>
        /// Assert number of 2xx HTTP responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter NumberOf2xxResponses(Func<long, bool> predicateFunction);
        /// <summary>
        /// Assert number of 3xx HTTP responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter NumberOf3xxResponses(Func<long, bool> predicateFunction);
        /// <summary>
        /// Assert number of 4xx HTTP responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter NumberOf4xxResponses(Func<long, bool> predicateFunction);
        /// <summary>
        /// Assert number of 5xx HTTP responses
        /// </summary>
        /// <param name="predicateFunction"></param>
        /// <returns></returns>
        ILoadTestAsserter NumberOf5xxResponses(Func<long, bool> predicateFunction);
        /// <summary>
        /// Return all Assert messages of the Asserter
        /// </summary>
        /// <returns></returns>
        IEnumerable<AssertResult> AssertAll();
    }
}
