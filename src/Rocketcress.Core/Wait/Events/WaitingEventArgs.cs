namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a method that handles the WaitingStarting and WaitingEnded events of the Waiter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void WaitingEventHandler(object? sender, WaitingEventArgs e);

    /// <summary>
    /// Represents event arguments for the WaitingStarting and WaitingEnded events of the Waiter.
    /// </summary>
    public class WaitingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a data store you can use on another event on this waiting operation.
        /// </summary>
        public IDictionary<string, object> DataStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitingEventArgs"/> class.
        /// </summary>
        /// <param name="dataStore">A data store you can use on another event on this waiting operation.</param>
        public WaitingEventArgs(IDictionary<string, object> dataStore)
        {
            DataStore = dataStore;
        }
    }
}
