
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Linq;

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a container for services that can be created when needed.
    /// </summary>
    public class ServiceContext : ExportProvider, IServiceProvider
    {
        private static ServiceContext _instance;
        /// <summary>
        /// Gets the current singleton instance of the <see cref="ServiceContext"/> class.
        /// </summary>
        public static ServiceContext Instance => _instance ?? (_instance = new ServiceContext());

        private readonly Dictionary<string, object> _services = new Dictionary<string, object>();
        private readonly Dictionary<Type, Func<ServiceContext, object>> _createFunctions = new Dictionary<Type, Func<ServiceContext, object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContext"/> class.
        /// </summary>
        public ServiceContext()
        {
            var container = new CompositionContainer(this);
            AddInstance<CompositionContainer>(container);
            AddInstance<IServiceProvider>(this);
        }

        /// <summary>
        /// Registers a create function for a specific type that is called the first time this type is retrieved from this <see cref="ServiceContext"/>.
        /// If a create function already exists for the type, the old function is overwritten by this one.
        /// </summary>
        /// <typeparam name="T">The type for which the create function should be registered to.</typeparam>
        /// <param name="createFunction">The function that is called when the type is requested and no value exists in the <see cref="ServiceContext"/>.</param>
        public void RegisterCreateFunction<T>(Func<ServiceContext, T> createFunction) where T : class
        {
            _createFunctions[typeof(T)] = createFunction;
        }

        /// <summary>
        /// Determines wether a create function for the specified type exists.
        /// </summary>
        /// <typeparam name="T">The type for which to check if a create function is registered to.</typeparam>
        /// <returns>Returns true if a create function had been registered for the type; otherwise false.</returns>
        public bool HasCreateFunction<T>()
        {
            return _createFunctions.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Gets the instance of a specific type that is registered by a specific key.
        /// If the instance does not exists a new instance is created by the registered create function.
        /// If no create function exists an exception is thrown.
        /// </summary>
        /// <param name="serviceType">The type of the instance.</param>
        /// <param name="key">The key of the instance.</param>
        /// <returns>Returns the instance that is registered by the specified type and key.</returns>
        public object GetInstance(Type serviceType, string key)
        {
            var dictKey = GetKey(serviceType, key);
            if (!_services.ContainsKey(dictKey))
            {
                if (!_createFunctions.ContainsKey(serviceType))
                {
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "A create function of type '{0}' could not be found", serviceType.FullName));
                }
                var serviceInstance = _createFunctions[serviceType].Invoke(this);
                AddInstance(serviceType, key, serviceInstance);
            }
            return _services[dictKey];
        }

        /// <summary>
        /// Gets the instance of a specific type that is registered by a specific key.
        /// If the instance does not exists a new instance is created by the registered create function.
        /// If no create function exists an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="key">The key of the instance.</param>
        /// <returns>Returns the instance that is registered by the specified type and key.</returns>
        public T GetInstance<T>(string key)
            => (T)GetInstance(typeof(T), key);

        /// <summary>
        /// Adds or replaces the instance that is registered by a specific type and the null-key.
        /// </summary>
        /// <typeparam name="TService">The type to register the instance to.</typeparam>
        /// <param name="instance">The instance to add.</param>
        public void AddInstance<TService>(TService instance)
            => AddInstance(null, instance);
        /// <summary>
        /// Adds or replaces the instance that is registered by a specific type and key.
        /// </summary>
        /// <typeparam name="TService">The type to register the instance to.</typeparam>
        /// <param name="name">The key to register the instance to.</param>
        /// <param name="instance">The instance to add.</param>
        public void AddInstance<TService>(string name, TService instance)
            => _services[GetKey(typeof(TService), name)] = instance;
        /// <summary>
        /// Adds or replaces the instance that is registered by a specific type and the null-key.
        /// </summary>
        /// <param name="type">The type to register the instance to.</param>
        /// <param name="instance">The instance to add.</param>
        public void AddInstance(Type type, object instance)
            => AddInstance(type, null, instance);
        /// <summary>
        /// Adds or replaces the instance that is registered by a specific type and key.
        /// </summary>
        /// <param name="type">The type to register the instance to.</param>
        /// <param name="name">The key to register the instance to.</param>
        /// <param name="instance">The instance to add.</param>
        public void AddInstance(Type type, string name, object instance)
        {
            if (!type.IsInstanceOfType(instance))
                throw new ArgumentException($"The instance has to be an instance of {type.FullName}.", nameof(instance));
            _services[GetKey(type, name)] = instance;
        }

        /// <summary>
        /// Removes the instance that is registered by a specific type and the null-key if it exists.
        /// </summary>
        /// <typeparam name="TService">The type of the instance.</typeparam>
        public void RemoveInstance<TService>()
            => RemoveInstance(typeof(TService), null);
        /// <summary>
        /// Removes the instance that is registered by a specific type and key if it exists.
        /// </summary>
        /// <typeparam name="TService">The type of the instance.</typeparam>
        /// <param name="name">The key of the instance.</param>
        public void RemoveInstance<TService>(string name)
            => RemoveInstance(typeof(TService), name);
        /// <summary>
        /// Removes the instance that is registered by a specific type and the null-key if it exists.
        /// </summary>
        /// <param name="type">The type of the instance.</param>
        public void RemoveInstance(Type type)
            => RemoveInstance(type, null);
        /// <summary>
        /// Removes the instance that is registered by a specific type and key if it exists.
        /// </summary>
        /// <param name="type">The type of the instance.</param>
        /// <param name="name">The key of the instance.</param>
        public void RemoveInstance(Type type, string name)
        {
            var key = GetKey(type, name);
            if (_services.ContainsKey(key))
                _services.Remove(key);
        }

        /// <summary>
        /// Gets all the exports that match the constraint defined by the specified definition.
        /// </summary>
        /// <param name="definition">The object that defines the conditions of the <see cref="Export"/> objects to return.</param>
        /// <param name="atomicComposition">The transactional container for the composition.</param>
        /// <returns>A collection that contains all the exports that match the specified condition.</returns>
        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var type = Type.GetType(definition.ContractName);
            if (type == null)
            {
                type = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(definition.ContractName)).FirstOrDefault(x => x != null);
                if (type == null)
                    throw new Exception($"Type with name '{definition.ContractName}' could not be found.");
            }

            return new[] { new Export(definition.ContractName, () => GetInstance(type)) };
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }

        /// <summary>
        /// Resets the current instance.
        /// </summary>
        public static void ResetServiceContext() => ResetInstance();
        internal static void ResetInstance() => _instance = null;

        private static string GetKey(Type t, string name) => $"{t.FullName};{name}";

        #region IServiceLocator
        /// <summary>
        /// Get an instance of the given serviceType.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>The requested service instance.</returns>
        public object GetInstance(Type serviceType)
            => GetInstance(serviceType, null);

        /// <summary>
        /// Get all instances of the given serviceType currently registered in the container.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>A sequence of instances of the requested serviceType.</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var key = GetKey(serviceType, null);
            var keys = _services.Keys.Where(x => x.StartsWith(key, StringComparison.Ordinal)).ToArray();
            if (keys.Any())
                return keys.Select(x => _services[x]);
            else
            {
                if (!_createFunctions.ContainsKey(serviceType))
                    return new object[0];
                var serviceInstance = _createFunctions[serviceType].Invoke(this);
                AddInstance(serviceType, key, serviceInstance);
                return new[] { _services[key] };
            }
        }

        /// <summary>
        /// Get an instance of the given TService.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <returns>The requested service instance.</returns>
        public TService GetInstance<TService>()
            => GetInstance<TService>(null);

        /// <summary>
        /// Get all instances of the given TService currently registered in the container.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <returns>A sequence of instances of the requested TService.</returns>
        public IEnumerable<TService> GetAllInstances<TService>()
            => GetAllInstances(typeof(TService)).Select(x => (TService)x);
        #endregion
    }
}
