using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;

namespace Rocketcress.Core.Tests
{
    [TestClass]
    public class LazyListTests : TestClassBase
    {
        [TestMethod]
        public void Indexer_SmallerZero()
        {
            var list = CreateMockedList(10, Times.Never(), out _);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[-1]);
        }

        [TestMethod]
        public void Indexer_TooLargeIndex()
        {
            var list = CreateMockedList(10, Times.Exactly(10), out _);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[10]);
        }

        [TestMethod]
        public void Indexer_CorrectIndex()
        {
            var list = CreateMockedList(10, Times.Exactly(4), out var mock);

            var item = list[3];

            Assert.AreEqual(3, item);
            for (int i = 0; i < 4; i++)
                mock.Verify(x => x(i), Times.Once());
        }

        [TestMethod]
        public void Indexer_UseCache()
        {
            var list = CreateMockedList(10, Times.Exactly(5), out var mock);

            var i1 = list[2];
            var i2 = list[4];
            var i3 = list[3];

            Assert.AreEqual(2, i1);
            Assert.AreEqual(4, i2);
            Assert.AreEqual(3, i3);
            for (int i = 0; i < 5; i++)
                mock.Verify(x => x(i), Times.Once());
        }

        [TestMethod]
        public void Count()
        {
            var list = CreateMockedList(10, Times.Exactly(10), out var mock);

            var count = list.Count;

            Assert.AreEqual(10, count);
            for (int i = 0; i < 10; i++)
                mock.Verify(x => x(i), Times.Once());
        }

        [TestMethod]
        public void Count_UseCache()
        {
            var list = CreateMockedList(10, Times.Exactly(10), out var mock);

            var c1 = list.Count;
            var c2 = list.Count;
            var c3 = list.Count;

            Assert.AreEqual(10, c1);
            Assert.AreEqual(10, c2);
            Assert.AreEqual(10, c3);
            for (int i = 0; i < 10; i++)
                mock.Verify(x => x(i), Times.Once());
        }

        [TestMethod]
        public void Reset()
        {
            var list = CreateMockedList(10, Times.Exactly(2), out var mock);

            var i1 = list[0];
            list.Reset();
            var i2 = list[0];

            mock.Verify(x => x(0), Times.Exactly(2));
        }

        [TestMethod]
        public void Reset_AfterFullIteration()
        {
            var list = CreateMockedList(10, Times.Exactly(20), out var mock);

            var i1 = list.Count;
            list.Reset();
            var i2 = list.Count;

            Assert.AreEqual(10, i1);
            Assert.AreEqual(10, i2);

            for (int i = 0; i < 10; i++)
                mock.Verify(x => x(0), Times.Exactly(2));
        }

        [TestMethod]
        public void Enumerator()
        {
            var list = CreateMockedList(10, Times.Exactly(10), out var mock);

            var i1 = 0;
            var enumerator = ((IEnumerable)list).GetEnumerator();
            while (enumerator.MoveNext())
            {
                Assert.AreEqual(i1, enumerator.Current);
                mock.Verify(x => x(i1), Times.Once());
                i1++;
            }

            Assert.AreEqual(10, i1);
        }

        [TestMethod]
        public void Enumerator_CurrentStart()
        {
            var list = CreateMockedList(10, Times.Never(), out var mock, i => i + 10);

            var enumerator = ((IEnumerable)list).GetEnumerator();

            Assert.AreEqual(0, enumerator.Current);
        }

        [TestMethod]
        public void Enumerator_Reset()
        {
            var list = CreateMockedList(10, Times.Exactly(3), out var mock);

            var enumerator = ((IEnumerable)list).GetEnumerator();
            for (int i = 0; i < 3; i++)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i, enumerator.Current);
            }

            enumerator.Reset();
            for (int i = 0; i < 3; i++)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i, enumerator.Current);
            }

            for (int i = 0; i < 3; i++)
                mock.Verify(x => x(i), Times.Once());
        }

        [TestMethod]
        public void EnumeratorT()
        {
            var list = CreateMockedList(10, Times.Exactly(10), out var mock);

            var i1 = 0;
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Assert.AreEqual(i1, enumerator.Current);
                mock.Verify(x => x(i1), Times.Once());
                i1++;
            }

            Assert.AreEqual(10, i1);
        }

        [TestMethod]
        public void EnumeratorT_Dispose()
        {
            var list = CreateMockedList(10, Times.Never(), out var mock);

            var enumerator = list.GetEnumerator();
            enumerator.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => enumerator.MoveNext());
            Assert.ThrowsException<ObjectDisposedException>(() => enumerator.Current);
            Assert.ThrowsException<ObjectDisposedException>(() => enumerator.Reset());
            Assert.ThrowsException<ObjectDisposedException>(() => enumerator.Dispose());
        }

        [TestMethod]
        public void EnumeratorT_CurrentStart()
        {
            var list = CreateMockedList(10, Times.Never(), out var mock, i => i + 10);

            var enumerator = list.GetEnumerator();

            Assert.AreEqual(0, enumerator.Current);
        }

        [TestMethod]
        public void EnumeratorT_Reset()
        {
            var list = CreateMockedList(10, Times.Exactly(3), out var mock);

            var enumerator = list.GetEnumerator();
            for (int i = 0; i < 3; i++)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i, enumerator.Current);
            }

            enumerator.Reset();
            for (int i = 0; i < 3; i++)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i, enumerator.Current);
            }

            for (int i = 0; i < 3; i++)
                mock.Verify(x => x(i), Times.Once());
        }

        private LazyList<int> CreateMockedList(int itemCount, Times expectedIterations, out Mock<Func<int, int>> getItemMock, Func<int, int> returnsFunc = null)
        {
            getItemMock = Mocks.Create<Func<int, int>>();
            getItemMock.Setup(x => x(It.IsAny<int>())).Returns(returnsFunc ?? new Func<int, int>(i => i)).Verifiable(Verifiables, expectedIterations);
            var enumerable = Enumerable.Range(0, itemCount).Select(getItemMock.Object);

            return new LazyList<int>(enumerable);
        }
    }
}
