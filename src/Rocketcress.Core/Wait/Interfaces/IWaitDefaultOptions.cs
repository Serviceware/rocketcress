namespace Rocketcress.Core
{
    /// <summary>
    /// Represents an object with default <see cref="IWaitOptions"/>.
    /// </summary>
    internal interface IWaitDefaultOptions
    {
        /// <summary>
        /// Gets the default options.
        /// </summary>
        IWaitOptions DefaultOptions { get; }
    }
}
