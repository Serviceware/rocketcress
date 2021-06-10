using System.Collections;
using System.Collections.Generic;

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a lazy list that gets items from an IEnumerable when they are needed and are then cached.
    /// </summary>
    /// <typeparam name="T">The type of the elements in this list.</typeparam>
    public class LazyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _cachedItems;
        private readonly IEnumerator<T> _enumerator;
        private bool _enumeratorAtEnd = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyList{T}"/> class.
        /// </summary>
        /// <param name="enumerable">The items that should be loaded with this lazy list.</param>
        public LazyList(IEnumerable<T> enumerable)
        {
            _cachedItems = new List<T>();
            _enumerator = enumerable.GetEnumerator();
        }

        /// <summary>
        /// Gets the element at the specified index in the list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the list.</returns>
        public T this[int index]
        {
            get
            {
                if (index < _cachedItems.Count)
                    return _cachedItems[index];
                while (!_enumeratorAtEnd && index >= _cachedItems.Count)
                {
                    if (_enumerator.MoveNext())
                        _cachedItems.Add(_enumerator.Current);
                    else
                        _enumeratorAtEnd = true;
                }

                return _cachedItems[index];
            }
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                var count = _cachedItems.Count;
                while (!_enumeratorAtEnd)
                {
                    if (_enumerator.MoveNext())
                    {
                        count++;
                        _cachedItems.Add(_enumerator.Current);
                    }
                    else
                    {
                        _enumeratorAtEnd = true;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => new LazyListEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new LazyListEnumerator(this);

        private class LazyListEnumerator : IEnumerator<T>
        {
            private LazyList<T> _lazyList;
            private int _currentIndex = -1;

            public LazyListEnumerator(LazyList<T> lazyList)
            {
                _lazyList = lazyList;
            }

            public T Current => _currentIndex < 0 || _currentIndex > _lazyList._cachedItems.Count ? default : _lazyList._cachedItems[_currentIndex];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _lazyList = null;
            }

            public bool MoveNext()
            {
                _currentIndex++;
                if (_currentIndex < _lazyList._cachedItems.Count)
                    return true;

                if (!_lazyList._enumeratorAtEnd && _lazyList._enumerator.MoveNext())
                {
                    _lazyList._cachedItems.Add(_lazyList._enumerator.Current);
                    return true;
                }
                else
                {
                    _lazyList._enumeratorAtEnd = true;
                }

                return false;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }
        }
    }
}
