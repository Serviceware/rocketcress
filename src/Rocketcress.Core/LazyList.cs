using System.Collections;

#nullable disable

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a lazy list that gets items from an IEnumerable when they are needed and are then cached.
    /// </summary>
    /// <typeparam name="T">The type of the elements in this list.</typeparam>
    public class LazyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _cachedItems;
        private readonly IEnumerable<T> _enumerable;
        private IEnumerator<T> _currentEnumerator;
        private bool _enumeratorAtEnd = false;

        /// <summary>
        /// Gets the current enumerator of the underlying <see cref="IEnumerable{T}"/>.
        /// </summary>
        protected IEnumerator<T> CurrentEnumerator => _currentEnumerator ??= _enumerable.GetEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyList{T}"/> class.
        /// </summary>
        /// <param name="enumerable">The items that should be loaded with this lazy list.</param>
        public LazyList(IEnumerable<T> enumerable)
        {
            _cachedItems = new List<T>();
            _enumerable = enumerable;
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
                    if (CurrentEnumerator.MoveNext())
                        _cachedItems.Add(CurrentEnumerator.Current);
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
                    if (CurrentEnumerator.MoveNext())
                    {
                        count++;
                        _cachedItems.Add(CurrentEnumerator.Current);
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

        /// <summary>
        /// Resets this <see cref="LazyList{T}"/>. The underlying <see cref="IEnumerable{T}"/> is iterated from the start again.
        /// </summary>
        public void Reset()
        {
            _cachedItems.Clear();
            _currentEnumerator = null;
            _enumeratorAtEnd = false;
        }

        private class LazyListEnumerator : IEnumerator<T>
        {
            private LazyList<T> _lazyList;
            private int _currentIndex = -1;

            private LazyList<T> LazyList => _lazyList ?? throw new ObjectDisposedException(nameof(LazyListEnumerator));

            public LazyListEnumerator(LazyList<T> lazyList)
            {
                _lazyList = lazyList;
            }

            public T Current => _currentIndex < 0 || _currentIndex > LazyList._cachedItems.Count ? default : LazyList._cachedItems[_currentIndex];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                if (_lazyList == null)
                    throw new ObjectDisposedException(nameof(LazyListEnumerator));
                _lazyList = null;
            }

            public bool MoveNext()
            {
                _currentIndex++;
                if (_currentIndex < LazyList._cachedItems.Count)
                    return true;

                if (!LazyList._enumeratorAtEnd && LazyList.CurrentEnumerator.MoveNext())
                {
                    LazyList._cachedItems.Add(LazyList.CurrentEnumerator.Current);
                    return true;
                }
                else
                {
                    LazyList._enumeratorAtEnd = true;
                }

                return false;
            }

            public void Reset()
            {
                if (_lazyList == null)
                    throw new ObjectDisposedException(nameof(LazyListEnumerator));
                _currentIndex = -1;
            }
        }
    }
}
