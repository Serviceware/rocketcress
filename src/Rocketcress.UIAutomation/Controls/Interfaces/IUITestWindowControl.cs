using System.Windows;

namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a window UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestWindowControl : IUITestControl
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IUITestWindowControl"/> is maximized.
    /// </summary>
    bool Maximized { get; set; }

    /// <summary>
    /// Sets the size of the window.
    /// </summary>
    /// <param name="windowSize">Size of the window.</param>
    void SetWindowSize(Size windowSize);

    /// <summary>
    /// Sets the size of the window.
    /// </summary>
    /// <param name="windowSize">Size of the window.</param>
    /// <param name="moveCenter">If set to <c>true</c> the window is moved to the center of the current screen.</param>
    void SetWindowSize(Size windowSize, bool moveCenter);

    /// <summary>
    /// Moves this <see cref=" IUITestWindowControl"/> to the center of the current screen.
    /// </summary>
    void MoveToCenter();

    /// <summary>
    /// Sets the window title.
    /// </summary>
    /// <param name="titleText">The title text.</param>
    void SetWindowTitle(string titleText);

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <returns><c>true</c> if the window closed; otherwise <c>false</c>.</returns>
    bool Close();

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <returns><c>true</c> if the window closed; otherwise <c>false</c>.</returns>
    bool Close(int timeout);

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <param name="assert">If set to <c>true</c> an error is thrown when the window does not close.</param>
    /// <returns><c>true</c> if the window closed; otherwise <c>false</c>.</returns>
    bool Close(bool assert);

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="assert">If set to <c>true</c> an error is thrown when the window does not close.</param>
    /// <returns><c>true</c> if the window closed; otherwise <c>false</c>.</returns>
    bool Close(int timeout, bool assert);
}
