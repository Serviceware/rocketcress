using System.Threading;

namespace Rocketcress.Core.Tests;

[TestClass]
public class ActionOnDisposeTests : TestClassBase
{
    [TestMethod]
    public void Empty()
    {
        var action = ActionOnDispose.Empty;
        action.Dispose();
    }

    [TestMethod]
    public void WithoutTime()
    {
        var actionMock = Mocks.Create<Action>(MockBehavior.Loose);
        var action = new ActionOnDispose(actionMock.Object);

        actionMock.Verify(x => x.Invoke(), Times.Never());
        action.Dispose();
        actionMock.Verify(x => x.Invoke(), Times.Once());
    }

    [TestMethod]
    public void WithTime()
    {
        var actionMock = Mocks.Create<Action<int>>(MockBehavior.Loose);
        var action = new ActionOnDispose(actionMock.Object);

        actionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never());
        Thread.Sleep(10);
        action.Dispose();
        actionMock.Verify(x => x.Invoke(It.Is<int>(i => i > 10)), Times.Once());
    }
}
