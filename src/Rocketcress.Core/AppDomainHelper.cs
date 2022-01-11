using System.Reflection;

#nullable disable

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides methods that helps using seperate AppDomains.
    /// </summary>
    public static class AppDomainHelper
    {
        /// <summary>
        /// Creates a new AppDomain with a given name.
        /// </summary>
        /// <param name="domainName">The name of the domain.</param>
        /// <returns>Returns a new instance of the <see cref="AppDomain"/> class.</returns>
        public static AppDomain CreateAppDomain(string domainName)
        {
#if NETFRAMEWORK
            AppDomain appDomain = AppDomain.CreateDomain(domainName, null, AppDomain.CurrentDomain.SetupInformation);
#else
            AppDomain appDomain = AppDomain.CreateDomain(domainName);
#endif
            appDomain.CreateInstanceAndUnwrap(typeof(RemoteLoader).Assembly.GetName().Name, typeof(RemoteLoader).FullName);
            LoadAssembliesInCurrentAppDomainIntoNew(appDomain);
            return appDomain;
        }

        private static void LoadAssembliesInCurrentAppDomainIntoNew(AppDomain appDomain)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    var proxyDomain = new RemoteLoader();
                    Assembly asm = proxyDomain.LoadAssembly(assembly.FullName);
                    appDomain.Load(asm.GetName());
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Error while loading assembly \"{0}\": {1}", assembly.FullName, ex.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// Executes a given function inside a new seperate AppDomain.
        /// </summary>
        /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside a new AppDomain.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the new domain.</param>
        /// <returns>Returns the result of the <paramref name="providerFunc"/>.</returns>
        public static TResult ExecuteInNewDomain<TProvider, TResult>(Func<TProvider, TResult> providerFunc)
        {
            AppDomain domain = null;
            try
            {
                domain = CreateAppDomain($"TestDomain{{{Guid.NewGuid()}}}");
                return ExecuteInDifferentDomain(domain, providerFunc);
            }
            finally
            {
                if (domain != null)
                    AppDomain.Unload(domain);
            }
        }

        /// <summary>
        /// Executes a given function inside another AppDomain.
        /// </summary>
        /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside the other AppDomain.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="domain">The domain to execute the function in.</param>
        /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the other domain.</param>
        /// <returns>Returns the result of the <paramref name="providerFunc"/>.</returns>
        public static TResult ExecuteInDifferentDomain<TProvider, TResult>(AppDomain domain, Func<TProvider, TResult> providerFunc)
        {
            var provider = (TProvider)domain.CreateInstanceAndUnwrap(typeof(TProvider).Assembly.FullName, typeof(TProvider).FullName);
            return providerFunc(provider);
        }

        /// <summary>
        /// Executes a given function inside a new seperate AppDomain.
        /// </summary>
        /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside a new AppDomain.</typeparam>
        /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the new domain.</param>
        public static void ExecuteInNewDomain<TProvider>(Action<TProvider> providerFunc)
        {
            AppDomain domain = null;
            try
            {
                domain = CreateAppDomain($"TestDomain{{{Guid.NewGuid()}}}");
                ExecuteInDifferentDomain(domain, providerFunc);
            }
            finally
            {
                if (domain != null)
                    AppDomain.Unload(domain);
            }
        }

        /// <summary>
        /// Executes a given function inside another AppDomain.
        /// </summary>
        /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside the other AppDomain.</typeparam>
        /// <param name="domain">The domain to execute the function in.</param>
        /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the other domain.</param>
        public static void ExecuteInDifferentDomain<TProvider>(AppDomain domain, Action<TProvider> providerFunc)
        {
            var provider = (TProvider)domain.CreateInstanceAndUnwrap(typeof(TProvider).Assembly.FullName, typeof(TProvider).FullName);
            providerFunc(provider);
        }

        /// <summary>
        /// This class is used internally.
        /// </summary>
        [Serializable]
        public class RemoteLoader : MarshalByRefObject
        {
            /// <summary>
            /// Loads an assembly into the current AppDomain.
            /// </summary>
            /// <param name="assemblyPath">The path to the assembly.</param>
            /// <returns>Returns the assembly that has been loaded.</returns>
            public Assembly LoadAssembly(string assemblyPath)
            {
                Assembly asm;
                try
                {
                    asm = AppDomain.CurrentDomain.Load(assemblyPath);
                }
                catch
                {
                    try
                    {
                        asm = Assembly.LoadFrom(assemblyPath);
                    }
                    catch
                    {
                        try
                        {
                            asm = Assembly.Load(assemblyPath);
                        }
                        catch
                        {
                            asm = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(assemblyPath));
                        }
                    }
                }

                return asm;
            }
        }
    }
}
