#if NETFRAMEWORK || (NETCOREAPP && !NET6_0_OR_GREATER)
using Rocketcress.Core.Utilities;

#nullable disable

namespace Rocketcress.Core;

/// <summary>
/// Provides methods that helps using seperate AppDomains.
/// </summary>
[Obsolete("Use Rocketcress.Core.Utilities.AppDomainUtility instead.")]
public static class AppDomainHelper
{
    /// <summary>
    /// Creates a new AppDomain with a given name.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <returns>Returns a new instance of the <see cref="AppDomain"/> class.</returns>
    public static AppDomain CreateAppDomain(string domainName)
        => AppDomainUtility.CreateAppDomain(domainName);

    /// <summary>
    /// Executes a given function inside a new seperate AppDomain.
    /// </summary>
    /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside a new AppDomain.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the new domain.</param>
    /// <returns>Returns the result of the <paramref name="providerFunc"/>.</returns>
    public static TResult ExecuteInNewDomain<TProvider, TResult>(Func<TProvider, TResult> providerFunc)
        => AppDomainUtility.ExecuteInNewDomain(providerFunc);

    /// <summary>
    /// Executes a given function inside another AppDomain.
    /// </summary>
    /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside the other AppDomain.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <param name="domain">The domain to execute the function in.</param>
    /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the other domain.</param>
    /// <returns>Returns the result of the <paramref name="providerFunc"/>.</returns>
    public static TResult ExecuteInDifferentDomain<TProvider, TResult>(AppDomain domain, Func<TProvider, TResult> providerFunc)
        => AppDomainUtility.ExecuteInDifferentDomain(domain, providerFunc);

    /// <summary>
    /// Executes a given function inside a new seperate AppDomain.
    /// </summary>
    /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside a new AppDomain.</typeparam>
    /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the new domain.</param>
    public static void ExecuteInNewDomain<TProvider>(Action<TProvider> providerFunc)
        => AppDomainUtility.ExecuteInNewDomain(providerFunc);

    /// <summary>
    /// Executes a given function inside another AppDomain.
    /// </summary>
    /// <typeparam name="TProvider">The type of the function provider which contains all functions to call inside the other AppDomain.</typeparam>
    /// <param name="domain">The domain to execute the function in.</param>
    /// <param name="providerFunc">The functions to execute. Only methods that are called on the <typeparamref name="TProvider"/> object are executed in the other domain.</param>
    public static void ExecuteInDifferentDomain<TProvider>(AppDomain domain, Action<TProvider> providerFunc)
        => AppDomainUtility.ExecuteInDifferentDomain(domain, providerFunc);

    /// <summary>
    /// This class is used internally.
    /// </summary>
    [Serializable]
    public class RemoteLoader : AppDomainUtility.RemoteLoader
    {
    }
}
#endif