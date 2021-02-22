using System;

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the AppDomain class.
    /// </summary>
    public static class AppDomainExtensions
    {
        /// <summary>
        /// Creates a new instance of the specified type inside the appdomain.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="domain">The domain in which the instance should be created in.</param>
        /// <returns>Returns a proxy class to the instance in the target app domain</returns>
        public static T CreateInstance<T>(this AppDomain domain) where T:class
        {
            var result = (T)domain.CreateInstanceAndUnwrap(typeof(T).Assembly.GetName().Name, typeof(T).FullName);
            return result;
        }
    }
}
