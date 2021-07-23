using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rocketcress.Core
{
    public class WaitContext<T>
    {
        private readonly Stopwatch _stopwatch;

        public IReadOnlyWaitOptions Options { get; }
        public IDictionary<string, object> Data { get; }
        public bool ThrowOnFailure { get; }
        public string? ErrorMessage { get; }
        public WaitResultBuilder<T> Result { get; }
        public TimeSpan Duration => _stopwatch.Elapsed;

        internal WaitContext(IReadOnlyWaitOptions options, bool throwOnFailure, string? errorMessage)
        {
            _stopwatch = new Stopwatch();
            Data = new Dictionary<string, object>();
            Options = options;
            ThrowOnFailure = throwOnFailure;
            ErrorMessage = errorMessage;
            Result = new WaitResultBuilder<T>();

            _stopwatch.Start();
        }

        internal WaitResult<T> GetResult()
        {
            _stopwatch.Stop();
            return Result.Build(_stopwatch.Elapsed);
        }
    }
}
