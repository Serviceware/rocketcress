#if !SLIM
namespace Rocketcress.Core.Base
{
    // TODO: Add XML Comments
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class MultipleTestExecutionControllerBase<TView, TTestMetadata> : TestObjectBase
        where TView : class
    {
        public abstract int DefaultExecutionCount { get; }

        public abstract void OnExecutionStart(TTestMetadata metadata, int testIndex);
        public abstract void OnExecutionEnded(TTestMetadata metadata, int testIndex, UnitTestOutcome outcome);
        public abstract void OnExecutionError(TTestMetadata metadata, int testIndex, Exception exception, string locationDescription);
        public abstract void ResetView(TView view);

        public List<Exception> Execute(TView view, TTestMetadata metadata, Action testAction, int timeout, bool continueOnError = true, bool failOnError = true)
        {
            return Execute(DefaultExecutionCount, view, metadata, testAction, timeout, continueOnError, failOnError);
        }

        public List<Exception> Execute(ICollection<TView> views, TTestMetadata metadata, Action testAction, int timeout, bool continueOnError = true, bool failOnError = true)
        {
            return Execute(DefaultExecutionCount, views, metadata, testAction, timeout, continueOnError, failOnError);
        }

        public List<Exception> Execute(int executionCount, TView view, TTestMetadata metadata, Action testAction, int timeout, bool continueOnError = true, bool failOnError = true)
        {
            return Execute(executionCount, new[] { view }, metadata, testAction, timeout, continueOnError, failOnError);
        }

        public List<Exception> Execute(int executionCount, ICollection<TView> views, TTestMetadata metadata, Action testAction, int timeout, bool continueOnError = true, bool failOnError = true)
        {
            var exceptions = new List<Exception>();
            if (executionCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(executionCount), "The executionCount has to be greater than 0");
            bool[] success = new bool[executionCount];
            bool isCanceled = false;

            for (int i = 0; i < executionCount && !isCanceled; i++)
            {
                try
                {
                    OnExecutionStart(metadata, i);

                    if (!TestHelper.RunWithTimeout(testAction, timeout))
                        Assert.Fail($"Test timed out after {timeout / 1000:0} seconds.");
                    else
                        success[i] = true;

                    OnExecutionEnded(metadata, i, UnitTestOutcome.Passed);
                }
                catch (Exception ex)
                {
                    OnExecutionEnded(metadata, i, UnitTestOutcome.Failed);
                    Logger.LogError("Error during test run #{0}: {1}", i + 1, ex);
                    exceptions.Add(new Exception("Error during test run #" + (i + 1), ex));
                    OnExecutionError(metadata, i, ex, "Run" + (i + 1));
                    if (!continueOnError)
                        break;
                }
                finally
                {
                    foreach (var view in views)
                    {
                        try
                        {
                            if (!TestHelper.RunWithTimeout(() => ResetView(view), TimeSpan.FromMinutes(5)))
                            {
                                isCanceled = true;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Error during test cleanup #{0}: {1}", i + 1, ex);
                            exceptions.Add(new Exception("Error during test cleanup #" + (i + 1), ex));
                            OnExecutionError(metadata, i, ex, "Cleanup" + (i + 1));
                        }
                    }
                }
            }

            Logger.LogInfo("{0}--------------------{0}--- Test Results ---{0}--------------------", Environment.NewLine);
            for (int i = 0; i < executionCount; i++)
            {
                Logger.LogInfo("Test Run #{0:00}: {1}", i + 1, success[i] ? "Passed" : "Failed");
            }

            if (failOnError)
                SetTestResult(exceptions, success);
            return exceptions;
        }

        protected virtual void SetTestResult(List<Exception> exceptions, bool[] success)
        {
            if (exceptions.Any())
            {
                throw new AggregateException(exceptions.Count + " Errors occured during TestRun", exceptions);
            }
            else if (success.Any(x => !x))
            {
                Assert.Fail("Some tests failed without exceptions (e.g. timeout). See earlier logs for more details.");
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
#endif