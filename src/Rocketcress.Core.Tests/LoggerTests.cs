using System.Globalization;
using System.Text;

namespace Rocketcress.Core.Tests;

[TestClass]
public class LoggerTests : TestClassBase
{
    private TestTraceListener _traceListener;

    [TestMethod]
    public void LogDebug_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.LogDebug("Test {0}", 4711);
        AssertLogMessage(logStartTime, "DBG - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogInfo_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.LogInfo("Test {0}", 4711);
        AssertLogMessage(logStartTime, "INF - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogWarning_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.LogWarning("Test {0}", 4711);
        AssertLogMessage(logStartTime, "WRN - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogError_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.LogError("Test {0}", 4711);
        AssertLogMessage(logStartTime, "ERR - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogCritical_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.LogCritical("Test {0}", 4711);
        AssertLogMessage(logStartTime, "CRT - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_UnknownLogLevel_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.Log((LogLevel)4711, "Test {0}", 4711);
        AssertLogMessage(logStartTime, "___ - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_NullParams_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.Log(LogLevel.Debug, "Test {0}", (object[])null);
        AssertLogMessage(logStartTime, "DBG - Test {0}", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_NoParams_Success()
    {
        var logStartTime = DateTime.Now;
        Logger.Log(LogLevel.Debug, "Test {0}");
        AssertLogMessage(logStartTime, "DBG - Test {0}", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_ErrorWhileFormat_LogErrorMessage()
    {
        var logStartTime = DateTime.Now;
        Logger.Log(LogLevel.Debug, "Test {0} {1}", 4711);
        AssertLogMessage(logStartTime, "ERR - Error while writing trace with message \"Test {0} {1}\" and parameters \"4711\": ", _traceListener.Text.TrimEnd(), Assert.StartsWith);
    }

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        _traceListener = new TestTraceListener();
        Trace.Listeners.Add(_traceListener);
    }

    protected override void OnCleanupTest()
    {
        Trace.Listeners.Remove(_traceListener);
        _traceListener.Dispose();
        base.OnCleanupTest();
    }

    private void AssertLogMessage(DateTime logStartTime, string expectedMessage, string actualLogMessage, Action<string, string> messageAssert = null)
    {
        logStartTime = new DateTime(logStartTime.Year, logStartTime.Month, logStartTime.Day, logStartTime.Hour, logStartTime.Minute, logStartTime.Second);

        var splitMessage = actualLogMessage.Split(':');
        var timespan = DateTime.ParseExact(string.Join(":", splitMessage.Take(3)), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        var message = string.Join(":", splitMessage.Skip(3)).TrimStart();

        Assert.IsBetween(logStartTime, logStartTime.AddSeconds(1), timespan, isMinInclusive: true, isMaxInclusive: true);
        (messageAssert ?? Assert.AreEqual)(expectedMessage, message);
    }

    private class TestTraceListener : TraceListener
    {
        private readonly StringBuilder _builder = new();

        public string Text => _builder.ToString();

        public override void Write(string message) => _builder.Append(message);
        public override void WriteLine(string message) => _builder.AppendLine(message);
    }
}
