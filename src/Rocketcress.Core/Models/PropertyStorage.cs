using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable

namespace Rocketcress.Core.Models
{
    /// <summary>
    /// Class that stores property values.
    /// </summary>
    public class PropertyStorage
    {
        private readonly object _lock = new object();

        private readonly IDictionary<string, object> _propertyValues = new Dictionary<string, object>();

        /// <summary>
        /// Retrieves the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>Returns the value of the property.</returns>
        public virtual T GetProperty<T>([CallerMemberName] string propertyName = null)
            => GetProperty<T>(null, propertyName);

        /// <summary>
        /// Retrieves the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="initializer">The function that is executed when a value for the property does not exist.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>Returns the value of the property.</returns>
        public virtual T GetProperty<T>(Func<T> initializer, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                return default;
            lock (_lock)
            {
                if (!_propertyValues.ContainsKey(propertyName))
                    _propertyValues[propertyName] = initializer == null ? default : initializer();
                return (T)_propertyValues[propertyName];
            }
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="value">The new property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        public virtual void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                return;
            lock (_lock)
                _propertyValues[propertyName] = value;
        }

        /// <summary>
        /// Resets the value of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public virtual void ResetProperty([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                return;
            lock (_lock)
            {
                if (_propertyValues.ContainsKey(propertyName))
                    _propertyValues.Remove(propertyName);
            }
        }

        /// <summary>
        /// Checks if the store has a value for a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>Returns wether a value for the given property exists.</returns>
        public virtual bool HasProperty([CallerMemberName] string propertyName = null)
        {
            lock (_lock)
                return _propertyValues.ContainsKey(propertyName);
        }

        /// <summary>
        /// Clears the store.
        /// </summary>
        public virtual void Clear()
        {
            lock (_lock)
                _propertyValues.Clear();
        }
    }
}
