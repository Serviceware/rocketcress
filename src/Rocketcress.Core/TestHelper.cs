using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides helper methods for Tests.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Enables or disables file system redirection for the calling thread.
        /// </summary>
        /// <param name="enable">Indicates the type of request for WOW64 system folder redirection. If TRUE, requests redirection be enabled; if FALSE, requests redirection be disabled.</param>
        /// <returns>Boolean value indicating whether the function succeeded. If TRUE, the function succeeded; if FALSE, the function failed.</returns>
        /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-wow64enablewow64fsredirection.</remarks>
        [DllImport("Kernel32.Dll", EntryPoint = "Wow64EnableWow64FsRedirection")]
        private static extern bool EnableWow64FSRedirection(bool enable);

#if DEBUG
        private static bool _isDebugConfiguration = true;
#else
        private static bool _isDebugConfiguration = false;
#endif

        /// <summary>
        /// Gets or sets a value indicating whether the current run is executed with debug assemblies.
        /// </summary>
        public static bool IsDebugConfiguration
        {
            get => _isDebugConfiguration;
            set => _isDebugConfiguration = value;
        }

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action returns true.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="catchExceptions">Determines if exceptions should be catched. If true, the action will be retried when an exception occurres; otherwise the exception will be rethrown.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryAction(Func<bool> action, int maxRetryCount, bool catchExceptions = true, int delayBetweenRetries = 0, bool exceptionTrace = true, Action<Exception> onException = null)
            => RetryActionCancelable(action, maxRetryCount, catchExceptions, delayBetweenRetries, exceptionTrace, onException == null ? (Func<Exception, bool>)null : CreateOnExceptionFunction(onException));

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action returns true.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="catchExceptions">Determines if exceptions should be catched. If true, the action will be retried when an exception occurres; otherwise the exception will be rethrown.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres. If the function returns true execution continues; otherwise the execution is stopped.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryActionCancelable(Func<bool> action, int maxRetryCount, bool catchExceptions = true, int delayBetweenRetries = 0, bool exceptionTrace = true, Func<Exception, bool> onException = null)
        {
            for (int i = 0; i <= maxRetryCount; i++)
            {
                try
                {
                    if (action())
                        return true;
                    if (delayBetweenRetries > 0)
                        Thread.Sleep(delayBetweenRetries);
                }
                catch (Exception ex)
                {
                    if (exceptionTrace)
                        Logger.LogInfo("Exception while RetryAction: {0}", ex.Message);
                    var @continue = onException?.Invoke(ex);
                    if (!catchExceptions)
                        throw new Exception("Exception while RetryAction", ex);
                    if (@continue == false)
                        return false;
                }

                if (delayBetweenRetries > 0 && i < maxRetryCount)
                    Thread.Sleep(delayBetweenRetries);
            }

            return false;
        }

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action does not throw an exception.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryAction(Action action, int maxRetryCount, int delayBetweenRetries = 0, bool exceptionTrace = true, Action<Exception> onException = null)
            => RetryActionCancelable(action, maxRetryCount, delayBetweenRetries, exceptionTrace, onException == null ? (Func<Exception, bool>)null : CreateOnExceptionFunction(onException));

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action does not throw an exception.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres. If the function returns true execution continues; otherwise the execution is stopped.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryActionCancelable(Action action, int maxRetryCount, int delayBetweenRetries = 0, bool exceptionTrace = true, Func<Exception, bool> onException = null)
        {
            for (int i = 0; i <= maxRetryCount; i++)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception ex)
                {
                    if (exceptionTrace)
                        Logger.LogInfo("Exception while RetryAction: {0}", ex.Message);
                    if (onException?.Invoke(ex) == false)
                        return false;
                }

                if (delayBetweenRetries > 0 && i < maxRetryCount)
                    Thread.Sleep(delayBetweenRetries);
            }

            return false;
        }

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action returns true. The number of retries is provided via the parameter.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="catchExceptions">Determines if exceptions should be catched. If true, the action will be retried when an exception occurres; otherwise the exception will be rethrown.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryAction(Func<int, bool> action, int maxRetryCount, bool catchExceptions = true, int delayBetweenRetries = 0, bool exceptionTrace = true, Action<Exception> onException = null)
            => RetryActionCancelable(action, maxRetryCount, catchExceptions, delayBetweenRetries, exceptionTrace, onException == null ? (Func<Exception, bool>)null : CreateOnExceptionFunction(onException));

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action returns true. The number of retries is provided via the parameter.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="catchExceptions">Determines if exceptions should be catched. If true, the action will be retried when an exception occurres; otherwise the exception will be rethrown.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres. If the function returns true execution continues; otherwise the execution is stopped.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryActionCancelable(Func<int, bool> action, int maxRetryCount, bool catchExceptions = true, int delayBetweenRetries = 0, bool exceptionTrace = true, Func<Exception, bool> onException = null)
        {
            for (int i = 0; i <= maxRetryCount; i++)
            {
                try
                {
                    if (action(i))
                        return true;
                }
                catch (Exception ex)
                {
                    if (exceptionTrace)
                        Logger.LogInfo("Exception while RetryAction: {0}", ex.Message);
                    var @continue = onException?.Invoke(ex);
                    if (!catchExceptions)
                        throw new Exception("Exception while RetryAction", ex);
                    if (@continue == false)
                        return false;
                }

                if (delayBetweenRetries > 0 && i < maxRetryCount)
                    Thread.Sleep(delayBetweenRetries);
            }

            return false;
        }

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action does not throw an exception. The number of retries is provided via the parameter.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryAction(Action<int> action, int maxRetryCount, int delayBetweenRetries = 0, bool exceptionTrace = true, Action<Exception> onException = null)
            => RetryActionCancelable(action, maxRetryCount, delayBetweenRetries, exceptionTrace, onException == null ? (Func<Exception, bool>)null : CreateOnExceptionFunction(onException));

        /// <summary>
        /// Retries an action until the action succeeds or the maximum of retries have been reached.
        /// </summary>
        /// <param name="action">The action to be retried. The retry succeeds if the action does not throw an exception. The number of retries is provided via the parameter.</param>
        /// <param name="maxRetryCount">The maximum number of retries.</param>
        /// <param name="delayBetweenRetries">The waiting time in miliseconds after the action failed.</param>
        /// <param name="exceptionTrace">Determines whether to trace exceptions that occure during execution.</param>
        /// <param name="onException">An action that is executed when an exception occurres. If the function returns true execution continues; otherwise the execution is stopped.</param>
        /// <returns>Returns true if the action has been successfully executed once; otherwise false.</returns>
        public static bool RetryActionCancelable(Action<int> action, int maxRetryCount, int delayBetweenRetries = 0, bool exceptionTrace = true, Func<Exception, bool> onException = null)
        {
            for (int i = 0; i <= maxRetryCount; i++)
            {
                try
                {
                    action(i);
                    return true;
                }
                catch (Exception ex)
                {
                    if (exceptionTrace)
                        Logger.LogInfo("Exception while RetryAction: {0}", ex.Message);
                    if (onException?.Invoke(ex) == false)
                        return false;
                }

                if (delayBetweenRetries > 0 && i < maxRetryCount)
                    Thread.Sleep(delayBetweenRetries);
            }

            return false;
        }

        /// <summary>
        /// Tries to execute an action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns true if the action completed successfully; otherwise false.</returns>
        public static bool Try(Action action) => Try(action, true);

        /// <summary>
        /// Tries to execute an action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="exceptionTrace">Determines wether to trace exceptions.</param>
        /// <returns>Returns true if the action completed successfully; otherwise false.</returns>
        public static bool Try(Action action, bool exceptionTrace)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                if (exceptionTrace)
                    Logger.LogInfo($"Ignored exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tries to execute an action.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns the result of the action if it succeeds; otherwise the default value of the result type.</returns>
        public static T Try<T>(Func<T> action) => Try(action, true, default);

        /// <summary>
        /// Tries to execute an action.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="exceptionTrace">Determines wether to trace exceptions.</param>
        /// <returns>Returns the result of the action if it succeeds; otherwise the default value of the result type.</returns>
        public static T Try<T>(Func<T> action, bool exceptionTrace) => Try(action, exceptionTrace, default);

        /// <summary>
        /// Tries to execute an action.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="resultOnError">The value that should be returned if the action fails.</param>
        /// <returns>Returns the result of the action if it succeeds; otherwise the resultOnError.</returns>
        public static T Try<T>(Func<T> action, T resultOnError) => Try(action, true, resultOnError);

        /// <summary>
        /// Tries to execute an action.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="exceptionTrace">Determines wether to trace exceptions.</param>
        /// <param name="resultOnError">The value that should be returned if the action fails.</param>
        /// <returns>Returns the result of the action if it succeeds; otherwise the resultOnError.</returns>
        public static T Try<T>(Func<T> action, bool exceptionTrace, T resultOnError)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                if (exceptionTrace)
                    Logger.LogInfo($"Ignored exception: {ex.Message}");
                return resultOnError;
            }
        }

        /// <summary>
        /// Runs one or more commands with PowerShell.
        /// </summary>
        /// <param name="command">The command or commands to be executed.</param>
        /// <returns>Returns the following results of the PowerShell execution: The Exit Code, the output lines, the error if one exists.</returns>
        public static (int ExitCode, string[] Output, string Error) RunPowerShell(string command) => RunPowerShell(command, false, false);

        /// <summary>
        /// Runs one or more commands with PowerShell.
        /// </summary>
        /// <param name="command">The command or commands to be executed.</param>
        /// <param name="runElevated">Determines wether to run PowerShell as an elevated process.</param>
        /// <returns>Returns the following results of the PowerShell execution: The Exit Code, the output lines, the error if one exists.</returns>
        public static (int ExitCode, string[] Output, string Error) RunPowerShell(string command, bool runElevated) => RunPowerShell(command, runElevated, false);

        /// <summary>
        /// Runs one or more commands with PowerShell.
        /// </summary>
        /// <param name="command">The command or commands to be executed.</param>
        /// <param name="runElevated">Determines wether to run PowerShell as an elevated process.</param>
        /// <param name="disableWow64">Determines wether to disable file system redirection for the PowerShell call.</param>
        /// <returns>Returns the following results of the PowerShell execution: The Exit Code, the output lines, the error if one exists.</returns>
        public static (int ExitCode, string[] Output, string Error) RunPowerShell(string command, bool runElevated, bool disableWow64)
        {
            var outFile = Path.GetTempFileName();
            var errorFile = Path.GetTempFileName();
            var combinedCommand = $"& {{{command}}} > \"{outFile}\" 2> \"{errorFile}\"";
            var encodedCommand = Convert.ToBase64String(Encoding.Unicode.GetBytes(combinedCommand));

            if (disableWow64)
                EnableWow64FSRedirection(false);

            Process process = null;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"powershell.exe",
                    Arguments = $"-EncodedCommand {encodedCommand}",
                    WindowStyle = ProcessWindowStyle.Hidden,
                };
                if (runElevated)
                    startInfo.Verb = "Runas";
                Logger.LogInfo($"RunPowerShell(@\"{combinedCommand}\", {runElevated}, {disableWow64}) ...");
                process = Process.Start(startInfo);
                process.WaitForExit();
            }
            finally
            {
                if (disableWow64)
                    EnableWow64FSRedirection(true);
            }

            var exitCode = process.ExitCode;
            var output = Array.Empty<string>();
            var errors = (string)null;

            try
            {
                errors = File.ReadAllText(errorFile);
                File.Delete(errorFile);
            }
            catch
            {
                // Ignore errors while retrieving the logs
            }

            try
            {
                output = File.ReadAllLines(outFile);
                File.Delete(outFile);
            }
            catch
            {
                // Ignore errors while retrieving the logs
            }

            if (output.Length > 0)
                Logger.LogInfo("Output:" + Environment.NewLine + string.Join(Environment.NewLine, output));
            if (!string.IsNullOrWhiteSpace(errors))
                Logger.LogWarning("Errors: " + errors);
            Logger.LogInfo("ExitCode: " + exitCode);

            return (exitCode, output, errors);
        }

        /// <summary>
        /// Runs multiple functions in different threads in parallel and waits for all to finish.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="functions">A list of functions to execute.</param>
        /// <returns>Returns all of the function results as an array. The results are ordered the same way as the functions.</returns>
        public static T[] LoopUntilAllFinished<T>(IReadOnlyList<Func<T>> functions) => LoopUntilAllFinished(functions, null);

        /// <summary>
        /// Runs multiple functions in different threads in parallel and waits for all to finish.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="functions">A list of functions to execute.</param>
        /// <param name="loopCancellationExpression">A function that is executed continuously. If this function returns false the wait is cancelled.</param>
        /// <returns>Returns all of the function results as an array. The results are ordered the same way as the functions.</returns>
        public static T[] LoopUntilAllFinished<T>(IReadOnlyList<Func<T>> functions, Func<IReadOnlyList<T>, bool> loopCancellationExpression)
        {
            var result = new T[functions.Count];
            var completed = new bool[functions.Count];
            var tasks = functions.Select(x => Task.Run(x)).ToArray();

            while (completed.Any(x => !x) && loopCancellationExpression?.Invoke(result) != true)
            {
                for (int i = 0; i < functions.Count; i++)
                {
                    if (tasks[i].IsCompleted)
                    {
                        completed[i] = true;
                        result[i] = tasks[i].Result;
                    }

                    if (tasks[i].IsCompleted || tasks[i].IsCanceled || tasks[i].IsFaulted)
                        tasks[i] = Task.Run(functions[i]);
                }

                Thread.Sleep(0);
            }

            return result;
        }

        /// <summary>
        /// Runs an action and stops it if the specified timeout is exceeded.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="timeout">The maximum execution time.</param>
        /// <returns>Returns true if the action executed without running in a timeout; otherwise false.</returns>
        public static bool RunWithTimeout(Action action, TimeSpan timeout) => RunWithTimeout(action, (int)timeout.TotalMilliseconds);

        /// <summary>
        /// Runs an action and stops it if the specified timeout is exceeded.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="timeout">The maximum execution time in miliseconds.</param>
        /// <returns>Returns true if the action executed without running in a timeout; otherwise false.</returns>
        public static bool RunWithTimeout(Action action, int timeout)
        {
            Exception exception = null;
            var testThread = new Thread(
                new ThreadStart(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }));

            testThread.Start();

            var result = testThread.Join(timeout);
            if (!result)
            {
                // TODO: Abort Thread. Thread.Abort is deprecated! Use CancellationToken instead.
            }
            else if (exception != null)
            {
                throw exception;
            }

            return result;
        }

        private static Func<Exception, bool> CreateOnExceptionFunction(Action<Exception> onException)
        {
            return ex =>
            {
                onException(ex);
                return true;
            };
        }
    }
}
