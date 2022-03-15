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
        Logger.LogDebug("Test {0}", 4711);
        AssertLogMessage("DBG - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogInfo_Success()
    {
        Logger.LogInfo("Test {0}", 4711);
        AssertLogMessage("INF - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogWarning_Success()
    {
        Logger.LogWarning("Test {0}", 4711);
        AssertLogMessage("WRN - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogError_Success()
    {
        Logger.LogError("Test {0}", 4711);
        AssertLogMessage("ERR - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void LogCritical_Success()
    {
        Logger.LogCritical("Test {0}", 4711);
        AssertLogMessage("CRT - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_UnknownLogLevel_Success()
    {
        Logger.Log((LogLevel)4711, "Test {0}", 4711);
        AssertLogMessage("___ - Test 4711", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_NullParams_Success()
    {
        Logger.Log(LogLevel.Debug, "Test {0}", (object[])null);
        AssertLogMessage("DBG - Test {0}", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_NoParams_Success()
    {
        Logger.Log(LogLevel.Debug, "Test {0}");
        AssertLogMessage("DBG - Test {0}", _traceListener.Text.TrimEnd());
    }

    [TestMethod]
    public void Log_ErrorWhileFormat_LogErrorMessage()
    {
        Logger.Log(LogLevel.Debug, "Test {0} {1}", 4711);
        AssertLogMessage("ERR - Error while writing trace with message \"Test {0} {1}\" and parameters \"4711\": ", _traceListener.Text.TrimEnd(), Assert.StartsWith);
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

    private void AssertLogMessage(string expectedMessage, string actualLogMessage, Action<string, string> messageAssert = null)
    {
        var splitMessage = actualLogMessage.Split(':');
        var timespan = DateTime.ParseExact(string.Join(":", splitMessage.Take(3)), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        var message = string.Join(":", splitMessage.Skip(3)).TrimStart();

        Assert.IsBetween(DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1), timespan);
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
